//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SupportTicketNotification]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class SupportTicketNotificationExtensionMethods
    {
        public static SupportTicketNotificationDto AsDto(this SupportTicketNotification supportTicketNotification)
        {
            var supportTicketNotificationDto = new SupportTicketNotificationDto()
            {
                SupportTicketNotificationID = supportTicketNotification.SupportTicketNotificationID,
                SupportTicket = supportTicketNotification.SupportTicket.AsDto(),
                EmailAddresses = supportTicketNotification.EmailAddresses,
                EmailSubject = supportTicketNotification.EmailSubject,
                EmailBody = supportTicketNotification.EmailBody,
                SentDate = supportTicketNotification.SentDate
            };
            DoCustomMappings(supportTicketNotification, supportTicketNotificationDto);
            return supportTicketNotificationDto;
        }

        static partial void DoCustomMappings(SupportTicketNotification supportTicketNotification, SupportTicketNotificationDto supportTicketNotificationDto);

        public static SupportTicketNotificationSimpleDto AsSimpleDto(this SupportTicketNotification supportTicketNotification)
        {
            var supportTicketNotificationSimpleDto = new SupportTicketNotificationSimpleDto()
            {
                SupportTicketNotificationID = supportTicketNotification.SupportTicketNotificationID,
                SupportTicketID = supportTicketNotification.SupportTicketID,
                EmailAddresses = supportTicketNotification.EmailAddresses,
                EmailSubject = supportTicketNotification.EmailSubject,
                EmailBody = supportTicketNotification.EmailBody,
                SentDate = supportTicketNotification.SentDate
            };
            DoCustomSimpleDtoMappings(supportTicketNotification, supportTicketNotificationSimpleDto);
            return supportTicketNotificationSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(SupportTicketNotification supportTicketNotification, SupportTicketNotificationSimpleDto supportTicketNotificationSimpleDto);
    }
}