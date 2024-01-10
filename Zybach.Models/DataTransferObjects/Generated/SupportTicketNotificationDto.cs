//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SupportTicketNotification]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class SupportTicketNotificationDto
    {
        public int SupportTicketNotificationID { get; set; }
        public SupportTicketDto SupportTicket { get; set; }
        public string EmailAddresses { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public DateTime SentDate { get; set; }
    }

    public partial class SupportTicketNotificationSimpleDto
    {
        public int SupportTicketNotificationID { get; set; }
        public System.Int32 SupportTicketID { get; set; }
        public string EmailAddresses { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public DateTime SentDate { get; set; }
    }

}