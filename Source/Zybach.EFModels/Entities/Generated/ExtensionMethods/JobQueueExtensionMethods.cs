//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [HangFire].[JobQueue]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class JobQueueExtensionMethods
    {
        public static JobQueueDto AsDto(this JobQueue jobQueue)
        {
            var jobQueueDto = new JobQueueDto()
            {
                Id = jobQueue.Id,
                JobId = jobQueue.JobId,
                Queue = jobQueue.Queue,
                FetchedAt = jobQueue.FetchedAt
            };
            DoCustomMappings(jobQueue, jobQueueDto);
            return jobQueueDto;
        }

        static partial void DoCustomMappings(JobQueue jobQueue, JobQueueDto jobQueueDto);

    }
}