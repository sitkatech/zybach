//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SupportTicketStatus]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class SupportTicketStatusDto
    {
        public int SupportTicketStatusID { get; set; }
        public string SupportTicketStatusName { get; set; }
        public string SupportTicketStatusDisplayName { get; set; }
        public int SortOrder { get; set; }
    }

    public partial class SupportTicketStatusSimpleDto
    {
        public int SupportTicketStatusID { get; set; }
        public string SupportTicketStatusName { get; set; }
        public string SupportTicketStatusDisplayName { get; set; }
        public int SortOrder { get; set; }
    }

}