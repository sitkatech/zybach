using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DroolTool.API.Services;
using DroolTool.API.Services.Authorization;
using DroolTool.EFModels.Entities;
using DroolTool.Models.DataTransferObjects.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.DependencyInjection;

namespace DroolTool.API.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DroolToolDbContext _dbContext;
        private readonly ILogger<UserController> _logger;
        private readonly KeystoneService _keystoneService;
        private readonly DroolToolConfiguration _drooltoolConfiguration;

        public UserController(DroolToolDbContext dbContext, ILogger<UserController> logger, KeystoneService keystoneService, IOptions<DroolToolConfiguration> drooltoolConfigurationOptions)
        {
            _dbContext = dbContext;
            _logger = logger;
            _keystoneService = keystoneService;
            _drooltoolConfiguration = drooltoolConfigurationOptions.Value;
        }

        [HttpPost("/users/invite")]
        [UserManageFeature]
        public IActionResult InviteUser([FromBody] UserInviteDto inviteDto)
        {
            if (inviteDto.RoleID.HasValue)
            {
                var role = Role.GetByRoleID(_dbContext, inviteDto.RoleID.Value);
                if (role == null)
                {
                    return NotFound($"Could not find a Role with the ID {inviteDto.RoleID}");
                }
            }
            else
            {
                return BadRequest("Role ID is required.");
            }

            const string applicationName = "Rosedale-DroolTool Bravo Water Accounting Platform";
            const string drooltoolBravoWaterStorageDistrict = "Rosedale-DroolTool Bravo Water Storage District";
            var inviteModel = new KeystoneService.KeystoneInviteModel
            {
                FirstName = inviteDto.FirstName,
                LastName = inviteDto.LastName,
                Email = inviteDto.Email,
                Subject = $"Invitation to the {applicationName}",
                WelcomeText = $"You are receiving this notification because an administrator of the {applicationName}, an online service of {drooltoolBravoWaterStorageDistrict}, has invited you to create an account.",
                SiteName = applicationName,
                SignatureBlock = $"{drooltoolBravoWaterStorageDistrict}<br /><a href='mailto:admin@rrbwsd.com'>admin@rrbwsd.com</a><br />(661) 589-6045<br /><a href='https://www.rrbwsd.com'>https://www.rrbwsd.com</a>",
                RedirectURL = _drooltoolConfiguration.KEYSTONE_REDIRECT_URL
            };

            var response = _keystoneService.Invite(inviteModel);
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

        [HttpPost("users")]
        [LoggedInUnclassifiedFeature]
        public ActionResult<UserDto> CreateUser([FromBody] UserCreateDto userUpsertDto)
        {
            var user = EFModels.Entities.User.CreateNewUser(_dbContext, userUpsertDto, userUpsertDto.LoginName,
                userUpsertDto.UserGuid);

            var smtpClient = HttpContext.RequestServices.GetRequiredService<SitkaSmtpClientService>();
            var mailMessage = GenerateUserCreatedEmail(_drooltoolConfiguration.DROOLTOOL_WEB_URL, user, _dbContext);
            SitkaSmtpClientService.AddCcRecipientsToEmail(mailMessage,
                        EFModels.Entities.User.GetEmailAddressesForAdminsThatReceiveSupportEmails(_dbContext));
            SendEmailMessage(smtpClient, mailMessage);

            return Ok(user);
        }

        [HttpGet("users")]
        [UserManageFeature]
        public ActionResult<IEnumerable<UserDetailedDto>> List()
        {
            var userDtos = EFModels.Entities.User.List(_dbContext);
            return Ok(userDtos);
        }

        [HttpGet("users/unassigned-report")]
        [UserManageFeature]
        public ActionResult<UnassignedUserReportDto> GetUnassignedUserReport()
        {
            var report = new UnassignedUserReportDto
                {Count = _dbContext.User.Count(x => x.RoleID == (int) RoleEnum.Unassigned)};
            return Ok(report);
        }

        [HttpGet("users/{userID}")]
        [UserViewFeature]
        public ActionResult<UserDto> GetByUserID([FromRoute] int userID)
        {
            var userDto = EFModels.Entities.User.GetByUserID(_dbContext, userID);
            if (userDto == null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }

        [HttpGet("user-claims/{globalID}")]
        public ActionResult<UserDto> GetByGlobalID([FromRoute] string globalID)
        {
            var isValidGuid = Guid.TryParse(globalID, out var globalIDAsGuid);
            if (!isValidGuid)
            {
                return BadRequest();
            }

            var userDto = DroolTool.EFModels.Entities.User.GetByUserGuid(_dbContext, globalIDAsGuid);
            if (userDto == null)
            {
                return NotFound();
            }

            return Ok(userDto);
        }

        [HttpPut("users/{userID}")]
        [UserManageFeature]
        public ActionResult<UserDto> UpdateUser([FromRoute] int userID, [FromBody] UserUpsertDto userUpsertDto)
        {
            var userDto = EFModels.Entities.User.GetByUserID(_dbContext, userID);
            if (userDto == null)
            {
                return NotFound();
            }

            var validationMessages = DroolTool.EFModels.Entities.User.ValidateUpdate(_dbContext, userUpsertDto, userDto.UserID);
            validationMessages.ForEach(vm => {
                ModelState.AddModelError(vm.Type, vm.Message);
            });

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = Role.GetByRoleID(_dbContext, userUpsertDto.RoleID.GetValueOrDefault());
            if (role == null)
            {
                return NotFound($"Could not find a System Role with the ID {userUpsertDto.RoleID}");
            }

            var updatedUserDto = DroolTool.EFModels.Entities.User.UpdateUserEntity(_dbContext, userID, userUpsertDto);
            return Ok(updatedUserDto);
        }

        private static MailMessage GenerateUserCreatedEmail(string drooltoolUrl, UserDto user, DroolToolDbContext dbContext)
        {
            var messageBody = $@"A new user has signed up to the Rosedale-DroolTool Bravo Water Accounting Platform: <br/><br/>
 {user.FullName} ({user.Email}) <br/><br/>
As an administrator of the Water Accounting Platform, you can assign them a role and associate them with a Billing Account by following <a href='{drooltoolUrl}/users/{user.UserID}'>this link</a>. <br/><br/>
{SitkaSmtpClientService.GetSupportNotificationEmailSignature()}";

            var mailMessage = new MailMessage
            {
                Subject = $"New User in Rosedale-DroolTool Bravo Water Accounting Platform",
                Body = $"Hello,<br /><br />{messageBody}",
            };

            mailMessage.To.Add(SitkaSmtpClientService.GetDefaultEmailFrom());
            return mailMessage;
        }

        private void SendEmailMessage(SitkaSmtpClientService smtpClient, MailMessage mailMessage)
        {
            mailMessage.IsBodyHtml = true;
            mailMessage.From = SitkaSmtpClientService.GetDefaultEmailFrom();
            SitkaSmtpClientService.AddReplyToEmail(mailMessage);
            smtpClient.Send(mailMessage);
        }
    }
}
