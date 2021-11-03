//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [HangFire].[Job]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class JobExtensionMethods
    {
        public static JobDto AsDto(this Job job)
        {
            var jobDto = new JobDto()
            {
                Id = job.Id,
                StateId = job.StateId,
                StateName = job.StateName,
                InvocationData = job.InvocationData,
                Arguments = job.Arguments,
                CreatedAt = job.CreatedAt,
                ExpireAt = job.ExpireAt
            };
            DoCustomMappings(job, jobDto);
            return jobDto;
        }

        static partial void DoCustomMappings(Job job, JobDto jobDto);

    }
}