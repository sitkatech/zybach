using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API
{
    public class GETStartNewRunJob : ScheduledBackgroundJobBase<GETStartNewRunJob>
    {
        private readonly GETService _GETService;
        public const string JobName = "GET Start New Run Manual";
        
        public GETStartNewRunJob(IWebHostEnvironment webHostEnvironment, ILogger<GETStartNewRunJob> logger,
            ZybachDbContext zybachDbContext, IOptions<ZybachConfiguration> zybachConfiguration, GETService GETService, SitkaSmtpClientService sitkaSmtpClientService) : base(
            JobName, logger, webHostEnvironment, zybachDbContext, zybachConfiguration, sitkaSmtpClientService)
        {
            _GETService = GETService;
        }

        public override List<RunEnvironment> RunEnvironments => new List<RunEnvironment>
            {RunEnvironment.Development, RunEnvironment.Staging, RunEnvironment.Production};

        protected override void RunJobImplementation()
        {
            Task.WaitAll(_GETService.StartNewRobustReviewScenarioRun());
        }
    }
}