using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using zybach.API.Services.Notifications;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Services.Notifications
{
    public class SupportTicketAssigneeChanged : NotificationEmailBase
    {
        public SupportTicketAssigneeChanged(SupportTicketNotificationService notificationService, ZybachDbContext dbContext, IOptions<ZybachConfiguration> zybachConfiguration, SitkaSmtpClientService sitkaSmtpClient) 
            : base(notificationService, dbContext, zybachConfiguration, sitkaSmtpClient)
        { }

        public async Task QueueNotification(SupportTicketDetailDto supportTicket, UserDto currentUser)
        {

            var mailMessage = GenerateMailMessage(supportTicket, currentUser);
            await _notificationService.SendNotification(mailMessage, supportTicket.SupportTicketID);
            
        }

        private MailMessage GenerateMailMessage(SupportTicketDetailDto supportTicket, UserDto currentUser)
        {
            var ticketDetailUrl = $"{_zybachConfiguration.WEB_URL}/{supportTicket.GetUrlFragment()}";
            var messageBody = $@"Ticket {supportTicket.SupportTicketID} {supportTicket.SupportTicketTitle} for Well  {supportTicket.Well.WellRegistrationID} 
                has been assigned to {supportTicket.AssigneeUser.FullName} by {currentUser.FullName}.<br><br>
                <a href=""{ticketDetailUrl}"">View ticket details</a>";

            var mailMessage = new MailMessage
            {
                Subject = $"Zybach System Notification: Ticket {supportTicket.SupportTicketID} {supportTicket.SupportTicketTitle} Assignee Changed",
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