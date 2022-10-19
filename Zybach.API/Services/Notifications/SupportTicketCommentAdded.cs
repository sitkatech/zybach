using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using zybach.API.Services.Notifications;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Services.Notifications
{
    public class SupportTicketCommentAdded : NotificationEmailBase
    {
        public SupportTicketCommentAdded(SupportTicketNotificationService supportTicketNotificationService, ZybachDbContext dbContext, IOptions<ZybachConfiguration> zybachConfiguration, SitkaSmtpClientService sitkaSmtpClient) 
            : base(supportTicketNotificationService, dbContext, zybachConfiguration, sitkaSmtpClient)
        { }

        public async Task QueueNotification(int supportTicketID, UserDto currentUser)
        {
            var supportTicket = SupportTickets.GetByID(_dbContext, supportTicketID).AsSimpleDto();

            var mailMessage = GenerateMailMessage(supportTicket, currentUser);
            await _notificationService.SendNotification(mailMessage, supportTicketID);
        }

        private MailMessage GenerateMailMessage(SupportTicketSimpleDto supportTicket, UserDto currentUser)
        {
            var ticketDetailUrl = $"{_zybachConfiguration.WEB_URL}/{supportTicket.GetUrlFragment()}";
            var messageBody = $@"A new comment has been added to Ticket {supportTicket.SupportTicketID} {supportTicket.SupportTicketTitle} for Well  {supportTicket.Well.WellRegistrationID}.<br><br>
                This ticket is currently {(supportTicket.AssigneeUser != null ? $"assigned to {supportTicket.AssigneeUser.FullName}" : "unassigned")} | <a href=""{ticketDetailUrl}"">View ticket details</a>";

            var mailMessage = new MailMessage
            {
                Subject = $"Zybach System Notification: Ticket {supportTicket.SupportTicketID} {supportTicket.SupportTicketTitle} Comment Added",
                Body = $"Hello, <br/><br/>{messageBody}",
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(supportTicket.CreatorUser.Email));
            if (supportTicket.AssigneeUser != null)
            {
                mailMessage.CC.Add(new MailAddress(supportTicket.AssigneeUser.Email));
            }
            mailMessage.CC.Add(new MailAddress(currentUser.Email));

            return mailMessage;
        }
    }
}