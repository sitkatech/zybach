using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Zybach.Models.DataTransferObjects;
using Microsoft.Extensions.DependencyInjection;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class UserController : SitkaController<UserController>
    {
        public UserController(ZybachDbContext dbContext, ILogger<UserController> logger, KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext, logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpPost("/api/users/invite")]
        [AdminFeature]
        public async Task<IActionResult> InviteUser([FromBody] UserInviteDto inviteDto)
        {
            if (inviteDto.RoleID.HasValue)
            {
                var role = Role.GetByRoleID(_dbContext, inviteDto.RoleID.Value);
                if (role == null)
                {
                    return BadRequest($"Could not find a Role with the ID {inviteDto.RoleID}");
                }
            }
            else
            {
                return BadRequest("Role ID is required.");
            }

            var applicationName = $"{_zybachConfiguration.PlatformLongName}";
            var leadOrganizationLongName = $"{_zybachConfiguration.LeadOrganizationLongName}";
            var inviteModel = new KeystoneService.KeystoneInviteModel
            {
                FirstName = inviteDto.FirstName,
                LastName = inviteDto.LastName,
                Email = inviteDto.Email,
                Subject = $"Invitation to the {applicationName}",
                WelcomeText = $"You are receiving this notification because an administrator of the {applicationName}, an online service of the {leadOrganizationLongName}, has invited you to create an account.",
                SiteName = applicationName,
                SignatureBlock = $"{leadOrganizationLongName}<br /><a href='mailto:{_zybachConfiguration.LeadOrganizationEmail}'>{_zybachConfiguration.LeadOrganizationEmail}</a><a href='{_zybachConfiguration.LeadOrganizationHomeUrl}'>{_zybachConfiguration.LeadOrganizationHomeUrl}</a>",
                RedirectURL = _zybachConfiguration.KEYSTONE_REDIRECT_URL
            };

            var response = await _keystoneService.Invite(inviteModel);
            if (response.StatusCode != HttpStatusCode.OK || response.Error != null)
            {
                ModelState.AddModelError("Email", $"There was a problem inviting the user to Keystone: {response.Error.Message}.");
                if (response.Error.ModelState != null)
                {
                    foreach (var modelStateKey in response.Error.ModelState.Keys)
                    {
                        foreach (var err in response.Error.ModelState[modelStateKey])
                        {
                            ModelState.AddModelError(modelStateKey, err);
                        }
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var keystoneUser = response.Payload.Claims;
            var existingUser = EFModels.Entities.User.GetByEmail(_dbContext, inviteDto.Email);
            if (existingUser != null)
            {
                existingUser = EFModels.Entities.User.UpdateUserGuid(_dbContext, existingUser.UserID, keystoneUser.UserGuid);
                return Ok(existingUser);
            }

            var newUser = new UserUpsertDto
            {
                FirstName = keystoneUser.FirstName,
                LastName = keystoneUser.LastName,
                OrganizationName = keystoneUser.OrganizationName,
                Email = keystoneUser.Email,
                PhoneNumber = keystoneUser.PrimaryPhone,
                RoleID = inviteDto.RoleID.Value
            };

            var user = EFModels.Entities.User.CreateNewUser(_dbContext, newUser, keystoneUser.LoginName,
                keystoneUser.UserGuid);
            return Ok(user);
        }

        [HttpPost("/api/users")]
        [LoggedInUnclassifiedFeature]
        public ActionResult<UserDto> CreateUser([FromBody] UserCreateDto userUpsertDto)
        {
            var user = EFModels.Entities.User.CreateNewUser(_dbContext, userUpsertDto, userUpsertDto.LoginName,
                userUpsertDto.UserGuid);

            var smtpClient = HttpContext.RequestServices.GetRequiredService<SitkaSmtpClientService>();
            var mailMessage = GenerateUserCreatedEmail(_zybachConfiguration.WEB_URL, user, _dbContext, smtpClient);
            SitkaSmtpClientService.AddCcRecipientsToEmail(mailMessage,
                        EFModels.Entities.User.GetEmailAddressesForAdminsThatReceiveSupportEmails(_dbContext));
            SendEmailMessage(smtpClient, mailMessage);

            return Ok(user);
        }

        [HttpGet("/api/users")]
        [AdminFeature]
        public ActionResult<IEnumerable<UserDto>> List()
        {
            var userDtos = EFModels.Entities.User.List(_dbContext);
            return Ok(userDtos);
        }

        [HttpGet("/api/users/unassigned-report")]
        [AdminFeature]
        public ActionResult<UnassignedUserReportDto> GetUnassignedUserReport()
        {
            var report = new UnassignedUserReportDto
                {Count = _dbContext.Users.Count(x => x.RoleID == (int) RoleEnum.Unassigned)};
            return Ok(report);
        }

        [HttpGet("/api/users/{userID}")]
        [UserViewFeature]
        public ActionResult<UserDto> GetByUserID([FromRoute] int userID)
        {
            var userDto = EFModels.Entities.User.GetByUserID(_dbContext, userID);
            return RequireNotNullThrowNotFound(userDto, "User", userID);
        }

        [HttpGet("/api/users/user-claims/{globalID}")]
        public ActionResult<UserDto> GetByGlobalID([FromRoute] string globalID)
        {
            var isValidGuid = Guid.TryParse(globalID, out var globalIDAsGuid);
            if (!isValidGuid)
            {
                return BadRequest();
            }

            var userDto = EFModels.Entities.User.GetByUserGuid(_dbContext, globalIDAsGuid);
            if (userDto == null)
            {
                var notFoundMessage = $"User with GUID {globalIDAsGuid} does not exist!";
                _logger.LogError(notFoundMessage);
                return NotFound(notFoundMessage);
            }

            return Ok(userDto);
        }

        [HttpPut("/api/users/{userID}")]
        [AdminFeature]
        public ActionResult<UserDto> UpdateUser([FromRoute] int userID, [FromBody] UserUpsertDto userUpsertDto)
        {
            var userDto = EFModels.Entities.User.GetByUserID(_dbContext, userID);
            if (ThrowNotFound(userDto, "User", userID, out var actionResult))
            {
                return actionResult;
            }

            var validationMessages =
                Zybach.EFModels.Entities.User.ValidateUpdate(_dbContext, userUpsertDto, userDto.UserID);
            validationMessages.ForEach(vm => { ModelState.AddModelError(vm.Type, vm.Message); });

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = Role.GetByRoleID(_dbContext, userUpsertDto.RoleID.GetValueOrDefault());
            if (role == null)
            {
                return BadRequest($"Could not find a System Role with the ID {userUpsertDto.RoleID}");
            }

            var updatedUserDto = Zybach.EFModels.Entities.User.UpdateUserEntity(_dbContext, userID, userUpsertDto);
            return Ok(updatedUserDto);
        }

        [HttpPut("/api/users/set-disclaimer-acknowledged-date")]
        public ActionResult<UserDto> SetDisclaimerAcknowledgedDate([FromBody] int userID)
        {
            var userDto = EFModels.Entities.User.GetByUserID(_dbContext, userID);
            if (ThrowNotFound(userDto, "User", userID, out var actionResult))
            {
                return actionResult;
            }

            var updatedUserDto = Zybach.EFModels.Entities.User.SetDisclaimerAcknowledgedDate(_dbContext, userID);
            return Ok(updatedUserDto);
        }


        private MailMessage GenerateUserCreatedEmail(string zybachUrl, UserDto user, ZybachDbContext dbContext,
            SitkaSmtpClientService smtpClient)
        {
            var messageBody = $@"A new user has signed up to the {_zybachConfiguration.PlatformLongName}: <br/><br/>
 {user.FullName} ({user.Email}) <br/><br/>
As an administrator of the {_zybachConfiguration.PlatformShortName}, you can assign them a role and associate them with a Billing Account by following <a href='{zybachUrl}/users/{user.UserID}'>this link</a>. <br/><br/>
{smtpClient.GetSupportNotificationEmailSignature()}";

            var mailMessage = new MailMessage
            {
                Subject = $"New User in {_zybachConfiguration.PlatformLongName}",
                Body = $"Hello,<br /><br />{messageBody}",
            };

            mailMessage.To.Add(smtpClient.GetDefaultEmailFrom());
            return mailMessage;
        }

        private void SendEmailMessage(SitkaSmtpClientService smtpClient, MailMessage mailMessage)
        {
            mailMessage.IsBodyHtml = true;
            mailMessage.From = smtpClient.GetDefaultEmailFrom();
            mailMessage.ReplyToList.Add(!String.IsNullOrWhiteSpace(_zybachConfiguration.LeadOrganizationEmail) ? _zybachConfiguration.LeadOrganizationEmail : "donotreply@sitkatech.com");
            smtpClient.Send(mailMessage);
        }
    }
}
