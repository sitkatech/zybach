//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SupportTicketPriority]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class SupportTicketPriorityDto
    {
        public int SupportTicketPriorityID { get; set; }
        public string SupportTicketPriorityName { get; set; }
        public string SupportTicketPriorityDisplayName { get; set; }
        public int SortOrder { get; set; }
    }

    public partial class SupportTicketPrioritySimpleDto
    {
        public int SupportTicketPriorityID { get; set; }
        public string SupportTicketPriorityName { get; set; }
        public string SupportTicketPriorityDisplayName { get; set; }
        public int SortOrder { get; set; }
    }

}