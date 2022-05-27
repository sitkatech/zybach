using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API
{
    public class GETUpdateStatusOfNonTerminalRunJob : ScheduledBackgroundJobBase<GETUpdateStatusOfNonTerminalRunJob>
    {
        private readonly GETService _GETService;
        public const string JobName = "GET Update Status Of Non-Terminal Run Every 6 Hours";

        public GETUpdateStatusOfNonTerminalRunJob(IWebHostEnvironment webHostEnvironment, ILogger<GETUpdateStatusOfNonTerminalRunJob> logger,
            ZybachDbContext zybachDbContext, GETService GETService) : base(
            JobName, logger, webHostEnvironment, zybachDbContext)
        {
            _GETService = GETService;
        }

        public override List<RunEnvironment> RunEnvironments => new List<RunEnvironment>
            {RunEnvironment.Development, RunEnvironment.Staging, RunEnvironment.Production};

        protected override void RunJobImplementation()
        {
            try
            {
                Task.WaitAll(_GETService.UpdateCurrentlyRunningRunStatus());
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception($"{JobName} encountered an error", e);
            }
        }
    }
}