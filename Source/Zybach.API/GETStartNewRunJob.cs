using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using Zybach.API.Services;
using Zybach.EFModels.Entities;

namespace Zybach.API
{
    public class GETStartNewRunJob : ScheduledBackgroundJobBase<GETStartNewRunJob>
    {
        private readonly GETService _GETService;
        public const string JobName = "GET Start New Run Manual";
        
        public GETStartNewRunJob(IWebHostEnvironment webHostEnvironment, ILogger<GETStartNewRunJob> logger,
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
                Task.WaitAll(_GETService.StartNewRobustReviewScenarioRun());
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw new Exception($"{JobName} encountered an error", e);
            }
        }
    }
}