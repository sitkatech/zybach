using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API
{
    public class GETUpdateStatusOfNonTerminalRunJob : ScheduledBackgroundJobBase<GETUpdateStatusOfNonTerminalRunJob>
    {
        private readonly GETService _GETService;
        public const string JobName = "GET Update Status Of Non-Terminal Run Every 6 Hours";

        public GETUpdateStatusOfNonTerminalRunJob(IWebHostEnvironment webHostEnvironment, ILogger<GETUpdateStatusOfNonTerminalRunJob> logger,
            ZybachDbContext zybachDbContext, IOptions<ZybachConfiguration> zybachConfiguration, GETService GETService, SitkaSmtpClientService sitkaSmtpClientService) : base(
            JobName, logger, webHostEnvironment, zybachDbContext, zybachConfiguration, sitkaSmtpClientService)
        {
            _GETService = GETService;
        }

        public override List<RunEnvironment> RunEnvironments => new List<RunEnvironment>
            {RunEnvironment.Development, RunEnvironment.Staging, RunEnvironment.Production};

        protected override void RunJobImplementation()
        {
            Task.WaitAll(_GETService.UpdateCurrentlyRunningRunStatus());
        }
    }
}