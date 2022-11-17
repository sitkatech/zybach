using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Zybach.EFModels.Entities;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Zybach.API.Services;
using System;
using System.Net.Http;

namespace Zybach.API
{

    public class OpenETRetrieveFromBucketJob : ScheduledBackgroundJobBase<OpenETRetrieveFromBucketJob>,
        IOpenETRetrieveFromBucketJob
    {
        private readonly IOpenETService _openETService;
        private readonly HttpClient _httpClient;

        public OpenETRetrieveFromBucketJob(ILogger<OpenETRetrieveFromBucketJob> logger,
            IWebHostEnvironment webHostEnvironment, ZybachDbContext zybachDbContext,
            IOptions<ZybachConfiguration> zybachConfiguration, IOpenETService openETService, IHttpClientFactory httpClientFactory, SitkaSmtpClientService sitkaSmtpClientService) : base(JobName, logger, webHostEnvironment,
            zybachDbContext, zybachConfiguration, sitkaSmtpClientService)
        {
            _openETService = openETService;
            _httpClient = httpClientFactory.CreateClient("GenericClient");
        }

        public override List<RunEnvironment> RunEnvironments => new() { RunEnvironment.Production };

        public const string JobName = "OpenET Retrieve from Google Bucket and Update Usage Data";

        protected override void RunJobImplementation()
        {
            if (!_zybachConfiguration.AllowOpenETSync)
            {
                return;
            }

            var inProgressSyncs = _dbContext.OpenETSyncHistories.Where(x => x.OpenETSyncResultTypeID == (int)OpenETSyncResultTypeEnum.InProgress).ToList();
            foreach (var openEtSyncHistory in inProgressSyncs)
            {
                _openETService.ProcessOpenETData(openEtSyncHistory.OpenETSyncHistoryID, _httpClient, openEtSyncHistory.OpenETDataType.ToEnum);
            }

            //Fail any created syncs that have been in a created state for longer than 15 minutes
            var createdSyncs = _dbContext.OpenETSyncHistories
                .Where(x => x.OpenETSyncResultTypeID == (int)OpenETSyncResultTypeEnum.Created).ToList();
            if (createdSyncs.Any())
            {
                createdSyncs.ForEach(x =>
                {
                    if (DateTime.UtcNow.Subtract(x.CreateDate).Minutes > 15)
                    {
                        OpenETSyncHistory.UpdateOpenETSyncEntityByID(_dbContext, x.OpenETSyncHistoryID,
                            OpenETSyncResultTypeEnum.Failed,
                            "Request never exited the Created state. Please try again.");
                    }
                });
            }
        }
    }

    public interface IOpenETRetrieveFromBucketJob
    {
        void RunJob(IJobCancellationToken token);
    }
}
