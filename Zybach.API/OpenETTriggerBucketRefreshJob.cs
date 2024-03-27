using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Zybach.EFModels.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using System.Threading;
using System;
using Microsoft.EntityFrameworkCore;

namespace Zybach.API
{

    public class OpenETTriggerBucketRefreshJob : ScheduledBackgroundJobBase<OpenETTriggerBucketRefreshJob>,
        IOpenETTriggerBucketRefreshJob
    {
        private readonly OpenETService _openETService;

        public OpenETTriggerBucketRefreshJob(ILogger<OpenETTriggerBucketRefreshJob> logger,
            IWebHostEnvironment webHostEnvironment, ZybachDbContext zybachDbContext,
            IOptions<ZybachConfiguration> zybachConfiguration, OpenETService openETService, SitkaSmtpClientService sitkaSmtpClientService) : base(JobName, logger, webHostEnvironment,
            zybachDbContext, zybachConfiguration, sitkaSmtpClientService)
        {
            _openETService = openETService;
        }

        public override List<RunEnvironment> RunEnvironments => new() { RunEnvironment.Production };

        public const string JobName = "OpenET Trigger Google Bucket Update";

        protected override async void RunJobImplementation()
        {
            // we need to create any missing OpenETSync year month combos from 2020 on
            var today = DateTime.Today;
            var currentYear = today.Year;
            var newOpenETSyncs = new List<OpenETSync>();
            {
                var existingOpenETSyncs = _dbContext.OpenETSyncs.ToDictionary(x => $"{x.Year}_{x.Month}_{x.OpenETDataTypeID}");
                for (var year = 2020; year <= currentYear; year++)
                {
                    var finalMonth = year == currentYear ? today.Month - 1 : 12;
                    for (var month = 1; month <= finalMonth; month++)
                    {
                        foreach (var openETDataType in OpenETDataType.All)
                        {
                            var openETDataTypeID = openETDataType.OpenETDataTypeID;
                            if (!existingOpenETSyncs.ContainsKey($"{year}_{month}_{openETDataTypeID}"))
                            {
                                var openETSync = new OpenETSync()
                                {
                                    Year = year,
                                    Month = month,
                                    OpenETDataTypeID = openETDataTypeID
                                };
                                newOpenETSyncs.Add(openETSync);
                            }
                        }
                    }
                }
            }

            if (newOpenETSyncs.Any())
            {
                _dbContext.OpenETSyncs.AddRange(newOpenETSyncs);
                await _dbContext.SaveChangesAsync();
            }

            var openETSyncs = _dbContext.OpenETSyncs.AsNoTracking().Where(x => !x.FinalizeDate.HasValue);
            if (!openETSyncs.Any())
            {
                return;
            }

            foreach (var openETSync in openETSyncs)
            {
                await _openETService.TriggerOpenETGoogleBucketRefresh(openETSync.Year, openETSync.Month, openETSync.OpenETDataTypeID);
                Thread.Sleep(1000); // intentional sleep here to make sure we don't hit the maximum rate limit
            }
        }
    }

    public interface IOpenETTriggerBucketRefreshJob
    {
        void RunJob(IJobCancellationToken token);
    }
}
