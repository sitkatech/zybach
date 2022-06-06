//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SupportTicketStatus]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class SupportTicketStatusExtensionMethods
    {
        public static SupportTicketStatusDto AsDto(this SupportTicketStatus supportTicketStatus)
        {
            var supportTicketStatusDto = new SupportTicketStatusDto()
            {
                SupportTicketStatusID = supportTicketStatus.SupportTicketStatusID,
                SupportTicketStatusName = supportTicketStatus.SupportTicketStatusName,
                SupportTicketStatusDisplayName = supportTicketStatus.SupportTicketStatusDisplayName,
                SortOrder = supportTicketStatus.SortOrder
            };
            DoCustomMappings(supportTicketStatus, supportTicketStatusDto);
            return supportTicketStatusDto;
        }

        static partial void DoCustomMappings(SupportTicketStatus supportTicketStatus, SupportTicketStatusDto supportTicketStatusDto);

        public static SupportTicketStatusSimpleDto AsSimpleDto(this SupportTicketStatus supportTicketStatus)
        {
            var supportTicketStatusSimpleDto = new SupportTicketStatusSimpleDto()
            {
                SupportTicketStatusID = supportTicketStatus.SupportTicketStatusID,
                SupportTicketStatusName = supportTicketStatus.SupportTicketStatusName,
                SupportTicketStatusDisplayName = supportTicketStatus.SupportTicketStatusDisplayName,
                SortOrder = supportTicketStatus.SortOrder
            };
            DoCustomSimpleDtoMappings(supportTicketStatus, supportTicketStatusSimpleDto);
            return supportTicketStatusSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(SupportTicketStatus supportTicketStatus, SupportTicketStatusSimpleDto supportTicketStatusSimpleDto);
    }
}