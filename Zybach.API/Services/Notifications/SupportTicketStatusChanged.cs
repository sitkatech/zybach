using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using zybach.API.Services.Notifications;
using Zybach.EFModels.Entities;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Services.Notifications
{
    public class SupportTicketStatusChanged : NotificationEmailBase
    {
        public SupportTicketStatusChanged(SupportTicketNotificationService notificationService, ZybachDbContext dbContext, IOptions<ZybachConfiguration> zybachConfiguration, SitkaSmtpClientService sitkaSmtpClient) 
            : base(notificationService, dbContext, zybachConfiguration, sitkaSmtpClient)
        { }

        public async Task QueueNotification(SupportTicketDetailDto supportTicket, UserDto currentUser, int oldStatusID)
        {
            var oldStatus = SupportTicketStatus.ToType(oldStatusID);
            var mailMessage = GenerateMailMessage(supportTicket, currentUser, oldStatus);
            await _notificationService.SendNotification(mailMessage, supportTicket.SupportTicketID);
        }

        private MailMessage GenerateMailMessage(SupportTicketDetailDto supportTicket, UserDto currentUser, SupportTicketStatus oldStatus)
        {
            var ticketDetailUrl = $"{_zybachConfiguration.WEB_URL}/{supportTicket.GetUrlFragment()}";
            var messageBody = $@"Ticket {supportTicket.SupportTicketID} {supportTicket.SupportTicketTitle} status has changed from '{oldStatus.SupportTicketStatusDisplayName}' to '{supportTicket.Status.SupportTicketStatusDisplayName}'
                for Well  {supportTicket.Well.WellRegistrationID}.<br><br>
                This ticket is currently {(supportTicket.AssigneeUser != null ? $"assigned to {supportTicket.AssigneeUser.FullName}" : "unassigned")} | <a href=""{ticketDetailUrl}"">View ticket details</a>";

            var mailMessage = new MailMessage
            {
                Subject = $"Zybach System Notification: Ticket {supportTicket.SupportTicketID} {supportTicket.SupportTicketTitle} Status Changed",
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