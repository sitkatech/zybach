using Microsoft.Extensions.Options;
using Zybach.API.Services.Notifications;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace zybach.API.Services.Notifications
{
    public abstract class NotificationEmailBase
    {
        protected readonly SupportTicketNotificationService _notificationService;
        protected readonly ZybachDbContext _dbContext;
        protected readonly ZybachConfiguration _zybachConfiguration;
        protected readonly SitkaSmtpClientService _sitkaSmtpClient;


        protected NotificationEmailBase(SupportTicketNotificationService notificationService, ZybachDbContext dbContext, IOptions<ZybachConfiguration> zybachConfiguration, SitkaSmtpClientService sitkaSmtpClient)
        {
            _notificationService = notificationService;
            _dbContext = dbContext;
            _zybachConfiguration = zybachConfiguration.Value;
            _sitkaSmtpClient = sitkaSmtpClient;
        }

    }
}