using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class SupportTicketExtensionMethods
    {
        static partial void DoCustomSimpleDtoMappings(SupportTicket supportTicket, SupportTicketSimpleDto supportTicketSimpleDto)
        {
            supportTicketSimpleDto.Well = supportTicket.Well.AsMinimalDto();
            supportTicketSimpleDto.Sensor = supportTicket.Sensor?.AsSimpleDto();
            supportTicketSimpleDto.CreatorUser = supportTicket.CreatorUser.AsSimpleDto();
            supportTicketSimpleDto.AssigneeUser = supportTicket.AssigneeUser?.AsSimpleDto();
            supportTicketSimpleDto.Status = supportTicket.SupportTicketStatus.AsSimpleDto();
            supportTicketSimpleDto.Priority = supportTicket.SupportTicketPriority.AsSimpleDto();
        }

        public static SupportTicketDetailDto AsDetailDto(this SupportTicket supportTicket)
        {
            var supportTicketDetailDto = new SupportTicketDetailDto()
            {
                SupportTicketID = supportTicket.SupportTicketID,
                DateCreated = supportTicket.DateCreated,
                DateUpdated = supportTicket.DateUpdated,
                SupportTicketTitle = supportTicket.SupportTicketTitle,
                SupportTicketDescription = supportTicket.SupportTicketDescription,
                Well = supportTicket.Well.AsMinimalDto(),
                Sensor = supportTicket.Sensor?.AsSimpleDto(),
                CreatorUser = supportTicket.CreatorUser.AsSimpleDto(),
                AssigneeUser = supportTicket.AssigneeUser?.AsSimpleDto(),
                Status = supportTicket.SupportTicketStatus.AsSimpleDto(),
                Priority = supportTicket.SupportTicketPriority.AsSimpleDto(),
                Comments = supportTicket.SupportTicketComments.Select(x => x.AsSimpleDto()).OrderByDescending(x => x.DateCreated).ToList()
            };

            return supportTicketDetailDto;
        }
    }
}