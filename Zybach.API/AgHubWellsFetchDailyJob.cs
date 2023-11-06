using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API
{
    public class AgHubWellsFetchDailyJob : ScheduledBackgroundJobBase<AgHubWellsFetchDailyJob>
    {
        private readonly AgHubService _agHubService;
        public const string JobName = "AgHub Well Fetch Daily";
        //        private static readonly List<string> ProblemWellRegistrationIDs = new List<string>{ "G-012886", "G-017908", "G-018992", "G-033855", "G-052662", "G-128363" };

        public AgHubWellsFetchDailyJob(IWebHostEnvironment webHostEnvironment, ILogger<AgHubWellsFetchDailyJob> logger,
            ZybachDbContext zybachDbContext, IOptions<ZybachConfiguration> zybachConfiguration, AgHubService agHubService, SitkaSmtpClientService sitkaSmtpClientService) : base(
            JobName, logger, webHostEnvironment, zybachDbContext, zybachConfiguration, sitkaSmtpClientService)
        {
            _agHubService = agHubService;
        }

        public override List<RunEnvironment> RunEnvironments => new() {RunEnvironment.Production, RunEnvironment.Staging}; 

        protected override async void RunJobImplementation()
        {
            await AgHubWellsTask.SyncForAllWells(_dbContext, _agHubService, DateTime.Today.AddDays(-30));
        }
    }
}