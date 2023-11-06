using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Zybach.EFModels.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using System.Threading;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;

namespace Zybach.API
{

    public class OpenETTriggerBucketRefreshJob : ScheduledBackgroundJobBase<OpenETTriggerBucketRefreshJob>,
        IOpenETTriggerBucketRefreshJob
    {
        private IBackgroundJobClient _backgroundJobClient;
        private readonly OpenETService _openETService;

        public OpenETTriggerBucketRefreshJob(ILogger<OpenETTriggerBucketRefreshJob> logger,
            IWebHostEnvironment webHostEnvironment, ZybachDbContext zybachDbContext,
            IOptions<ZybachConfiguration> zybachConfiguration, IBackgroundJobClient backgroundJobClient, OpenETService openETService, SitkaSmtpClientService sitkaSmtpClientService) : base(JobName, logger, webHostEnvironment,
            zybachDbContext, zybachConfiguration, sitkaSmtpClientService)
        {
            _backgroundJobClient = backgroundJobClient;
            _openETService = openETService;
        }

        public override List<RunEnvironment> RunEnvironments => new() { RunEnvironment.Production };

        public const string JobName = "OpenET Trigger Google Bucket Update";

        protected override void RunJobImplementation()
        {
            if (!_zybachConfiguration.AllowOpenETSync)
            {
                return;
            }

            var nonFinalizedOpenETSyncs = _dbContext.OpenETSyncs.Where(x => !x.FinalizeDate.HasValue);
            if (!nonFinalizedOpenETSyncs.Any())
            {
                return;
            }

            var openETDataTypes = OpenETDataType.All;

            nonFinalizedOpenETSyncs.ToList().ForEach(x =>
                {
                    openETDataTypes.ForEach(async y =>
                    {
                        await _openETService.TriggerOpenETGoogleBucketRefresh(x.Year, x.Month, y.OpenETDataTypeID);
                        Thread.Sleep(1000); // intentional sleep here to make sure we don't hit the maximum rate limit
                    });
                });
        }
    }

    public interface IOpenETTriggerBucketRefreshJob
    {
        void RunJob(IJobCancellationToken token);
    }
}
