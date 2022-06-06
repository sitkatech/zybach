using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.API.Services.Authorization;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Controllers
{
    [ApiController]
    public class SupportTicketController : SitkaController<SupportTicketController>
    {
        public SupportTicketController(ZybachDbContext dbContext, ILogger<SupportTicketController> logger,
            KeystoneService keystoneService, IOptions<ZybachConfiguration> zybachConfiguration) : base(dbContext,
            logger, keystoneService, zybachConfiguration)
        {
        }

        [HttpGet("/supportTickets/statuses")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<SupportTicketStatusDto>> GetSupportTicketStatuses()
        {
            var supportTicketStatusDtos = SupportTicketStatus.AllAsDto.OrderBy(x => x.SortOrder);
            return Ok(supportTicketStatusDtos);
        }

        [HttpGet("/supportTickets/priorities")]
        [ZybachViewFeature]
        public ActionResult<IEnumerable<SupportTicketPriorityDto>> GetSupportTicketPriorities()
        {
            var supportTicketPriorityDtos = SupportTicketPriority.AllAsDto.OrderBy(x => x.SortOrder);
            return Ok(supportTicketPriorityDtos);
        }

        [HttpGet("/supportTickets")]
        [ZybachViewFeature]
        public ActionResult<List<SupportTicketSimpleDto>> List()
        {
            var supportTickets = SupportTickets.ListAsSimpleDto(_dbContext);
            return Ok(supportTickets);
        }

        [HttpPost("/supportTickets")]
        [ZybachViewFeature]
        public ActionResult<SupportTicketDetailDto> Create([FromBody] SupportTicketUpsertDto supportTicketUpsertDto)
        {
            var well = _dbContext.Wells.SingleOrDefault(x =>
                x.WellRegistrationID == supportTicketUpsertDto.WellRegistrationID);
            if (well == null)
            {
                ModelState.AddModelError("Well Registration ID", $"Well with Well Registration ID '{supportTicketUpsertDto.WellRegistrationID}' not found!");
                return BadRequest(ModelState);
            }
            supportTicketUpsertDto.WellID = well.WellID;
            var supportTicket = SupportTickets.CreateNewSupportTicket(_dbContext, supportTicketUpsertDto);
            return Ok(supportTicket);
        }
        
        [HttpGet("/supportTickets/{supportTicketID}")]
        [ZybachViewFeature]
        public ActionResult<SupportTicketDetailDto> GetByID([FromRoute] int supportTicketID)
        {
            if (GetSupportTicketAndThrowIfNotFound(supportTicketID, out var supportTicket, out var actionResult)) return actionResult;
            
            return Ok(supportTicket.AsDetailDto());
        }

        [HttpPut("/supportTickets/{supportTicketID}")]
        [ZybachViewFeature]
        public ActionResult<SupportTicketDetailDto> UpdateByID([FromRoute] int supportTicketID, [FromBody] SupportTicketUpsertDto supportTicketUpsertDto)
        {
            if (GetSupportTicketWithTrackingAndThrowIfNotFound(supportTicketID, out var supportTicket, out var actionResult)) return actionResult;
            var well = _dbContext.Wells.SingleOrDefault(x =>
                x.WellRegistrationID == supportTicketUpsertDto.WellRegistrationID);
            if (well == null)
            {
                ModelState.AddModelError("Well Registration ID", $"Well with Well Registration ID '{supportTicketUpsertDto.WellRegistrationID}' not found!");
                return BadRequest(ModelState);
            }
            supportTicketUpsertDto.WellID = well.WellID;
            var updatedSupportTicket = SupportTickets.UpdateSupportTicket(_dbContext, supportTicket, supportTicketUpsertDto);
            return Ok(updatedSupportTicket);
        }

        [HttpDelete("/supportTickets/{supportTicketID}")]
        [ZybachViewFeature]
        public ActionResult DeleteByID([FromRoute] int supportTicketID)
        {
            if (GetSupportTicketWithTrackingAndThrowIfNotFound(supportTicketID, out var supportTicket, out var actionResult)) return actionResult;

            _dbContext.SupportTicketComments.RemoveRange(
                _dbContext.SupportTicketComments.Where(x => x.SupportTicketID == supportTicketID));
            _dbContext.SupportTickets.Remove(supportTicket);
            _dbContext.SaveChanges();
            return Ok();
        }

        private bool GetSupportTicketAndThrowIfNotFound(int supportTicketID, out SupportTicket supportTicket, out ActionResult actionResult)
        {
            supportTicket = SupportTickets.GetByID(_dbContext, supportTicketID);
            return ThrowNotFound(supportTicket, "SupportTicket", supportTicketID, out actionResult);
        }

        private bool GetSupportTicketWithTrackingAndThrowIfNotFound(int supportTicketID, out SupportTicket supportTicket, out ActionResult actionResult)
        {
            supportTicket = SupportTickets.GetByIDWithTracking(_dbContext, supportTicketID);
            return ThrowNotFound(supportTicket, "SupportTicket", supportTicketID, out actionResult);
        }


    }

}