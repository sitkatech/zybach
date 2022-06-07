//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SupportTicketPriority]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class SupportTicketPriorityExtensionMethods
    {
        public static SupportTicketPriorityDto AsDto(this SupportTicketPriority supportTicketPriority)
        {
            var supportTicketPriorityDto = new SupportTicketPriorityDto()
            {
                SupportTicketPriorityID = supportTicketPriority.SupportTicketPriorityID,
                SupportTicketPriorityName = supportTicketPriority.SupportTicketPriorityName,
                SupportTicketPriorityDisplayName = supportTicketPriority.SupportTicketPriorityDisplayName,
                SortOrder = supportTicketPriority.SortOrder
            };
            DoCustomMappings(supportTicketPriority, supportTicketPriorityDto);
            return supportTicketPriorityDto;
        }

        static partial void DoCustomMappings(SupportTicketPriority supportTicketPriority, SupportTicketPriorityDto supportTicketPriorityDto);

        public static SupportTicketPrioritySimpleDto AsSimpleDto(this SupportTicketPriority supportTicketPriority)
        {
            var supportTicketPrioritySimpleDto = new SupportTicketPrioritySimpleDto()
            {
                SupportTicketPriorityID = supportTicketPriority.SupportTicketPriorityID,
                SupportTicketPriorityName = supportTicketPriority.SupportTicketPriorityName,
                SupportTicketPriorityDisplayName = supportTicketPriority.SupportTicketPriorityDisplayName,
                SortOrder = supportTicketPriority.SortOrder
            };
            DoCustomSimpleDtoMappings(supportTicketPriority, supportTicketPrioritySimpleDto);
            return supportTicketPrioritySimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(SupportTicketPriority supportTicketPriority, SupportTicketPrioritySimpleDto supportTicketPrioritySimpleDto);
    }
}