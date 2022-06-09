//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SupportTicket]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class SupportTicketExtensionMethods
    {
        public static SupportTicketDto AsDto(this SupportTicket supportTicket)
        {
            var supportTicketDto = new SupportTicketDto()
            {
                SupportTicketID = supportTicket.SupportTicketID,
                DateCreated = supportTicket.DateCreated,
                DateUpdated = supportTicket.DateUpdated,
                CreatorUser = supportTicket.CreatorUser.AsDto(),
                AssigneeUser = supportTicket.AssigneeUser?.AsDto(),
                Well = supportTicket.Well.AsDto(),
                Sensor = supportTicket.Sensor?.AsDto(),
                SupportTicketStatus = supportTicket.SupportTicketStatus.AsDto(),
                SupportTicketPriority = supportTicket.SupportTicketPriority.AsDto(),
                SupportTicketTitle = supportTicket.SupportTicketTitle,
                SupportTicketDescription = supportTicket.SupportTicketDescription
            };
            DoCustomMappings(supportTicket, supportTicketDto);
            return supportTicketDto;
        }

        static partial void DoCustomMappings(SupportTicket supportTicket, SupportTicketDto supportTicketDto);

        public static SupportTicketSimpleDto AsSimpleDto(this SupportTicket supportTicket)
        {
            var supportTicketSimpleDto = new SupportTicketSimpleDto()
            {
                SupportTicketID = supportTicket.SupportTicketID,
                DateCreated = supportTicket.DateCreated,
                DateUpdated = supportTicket.DateUpdated,
                CreatorUserID = supportTicket.CreatorUserID,
                AssigneeUserID = supportTicket.AssigneeUserID,
                WellID = supportTicket.WellID,
                SensorID = supportTicket.SensorID,
                SupportTicketStatusID = supportTicket.SupportTicketStatusID,
                SupportTicketPriorityID = supportTicket.SupportTicketPriorityID,
                SupportTicketTitle = supportTicket.SupportTicketTitle,
                SupportTicketDescription = supportTicket.SupportTicketDescription
            };
            DoCustomSimpleDtoMappings(supportTicket, supportTicketSimpleDto);
            return supportTicketSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(SupportTicket supportTicket, SupportTicketSimpleDto supportTicketSimpleDto);
    }
}