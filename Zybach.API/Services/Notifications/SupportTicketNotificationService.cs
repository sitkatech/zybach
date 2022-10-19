using System;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Zybach.EFModels.Entities;

namespace Zybach.API.Services.Notifications
{

    public class SupportTicketNotificationService 
    {
        private readonly ZybachDbContext _dbContext;
        private readonly SitkaSmtpClientService _sitkaSmtpClient;

        public SupportTicketNotificationService(ZybachDbContext dbContext, SitkaSmtpClientService sitkaSmtpClient)
        {
            _dbContext = dbContext;
            _sitkaSmtpClient = sitkaSmtpClient;
        }

        public async Task SendNotification(MailMessage mailMessage, int supportTicketID)
        {
            await _sitkaSmtpClient.Send(mailMessage);

            var emailAddresses = string.Join(", ", mailMessage.To.Select(x => x.Address).Union(mailMessage.CC.Select(x => x.Address)));
            var supportTicketNotification = new SupportTicketNotification()
            {
                SupportTicketID = supportTicketID,
                EmailAddresses = emailAddresses,
                EmailBody = mailMessage.Body,
                EmailSubject = mailMessage.Subject,
                SentDate = DateTime.UtcNow,
            };  

            _dbContext.SupportTicketNotifications.Add(supportTicketNotification);
            await _dbContext.SaveChangesAsync();
        }
    }
}
