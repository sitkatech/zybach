using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Zybach.EFModels.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Zybach.API.Services;

namespace Zybach.API
{

    public class OpenETTriggerBucketRefreshJob : ScheduledBackgroundJobBase<OpenETTriggerBucketRefreshJob>,
        IOpenETTriggerBucketRefreshJob
    {
        private readonly ZybachConfiguration _zybachConfiguration;
        private IBackgroundJobClient _backgroundJobClient;
        private readonly IOpenETService _openETService;

        public OpenETTriggerBucketRefreshJob(ILogger<OpenETTriggerBucketRefreshJob> logger,
            IWebHostEnvironment webHostEnvironment, ZybachDbContext zybachDbContext,
            IOptions<ZybachConfiguration> zybachConfiguration, IBackgroundJobClient backgroundJobClient, IOpenETService openETService) : base(JobName, logger, webHostEnvironment,
            zybachDbContext)
        {
            _zybachConfiguration = zybachConfiguration.Value;
            _backgroundJobClient = backgroundJobClient;
            _openETService = openETService;
        }

        public override List<RunEnvironment> RunEnvironments => new() { RunEnvironment.Staging, RunEnvironment.Production };

        public const string JobName = "OpenET Trigger Google Bucket Update";

        protected override void RunJobImplementation()
        {
            if (!_zybachConfiguration.AllowOpenETSync || !_openETService.IsOpenETAPIKeyValid())
            {
                return;
            }

            var nonFinalizedWaterYearMonths = _dbContext.WaterYearMonths.Where(x => !x.FinalizeDate.HasValue);
            if (!nonFinalizedWaterYearMonths.Any())
            {
                return;
            }

            var openETDataTypes = OpenETDataType.All;

            nonFinalizedWaterYearMonths.ToList().ForEach(x =>
                {
                    openETDataTypes.ForEach(y =>
                            _openETService.TriggerOpenETGoogleBucketRefresh(x.WaterYearMonthID, y)
                    );
                });
        }
    }

    public interface IOpenETTriggerBucketRefreshJob
    {
        void RunJob(IJobCancellationToken token);
    }
}
