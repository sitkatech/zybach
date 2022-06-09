//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SupportTicket]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class SupportTicketDto
    {
        public int SupportTicketID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public UserDto CreatorUser { get; set; }
        public UserDto AssigneeUser { get; set; }
        public WellDto Well { get; set; }
        public SensorDto Sensor { get; set; }
        public SupportTicketStatusDto SupportTicketStatus { get; set; }
        public SupportTicketPriorityDto SupportTicketPriority { get; set; }
        public string SupportTicketTitle { get; set; }
        public string SupportTicketDescription { get; set; }
    }

    public partial class SupportTicketSimpleDto
    {
        public int SupportTicketID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int CreatorUserID { get; set; }
        public int? AssigneeUserID { get; set; }
        public int WellID { get; set; }
        public int? SensorID { get; set; }
        public int SupportTicketStatusID { get; set; }
        public int SupportTicketPriorityID { get; set; }
        public string SupportTicketTitle { get; set; }
        public string SupportTicketDescription { get; set; }
    }

}