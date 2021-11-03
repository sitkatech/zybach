//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [HangFire].[JobParameter]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class JobParameterExtensionMethods
    {
        public static JobParameterDto AsDto(this JobParameter jobParameter)
        {
            var jobParameterDto = new JobParameterDto()
            {
                Job = jobParameter.Job.AsDto(),
                Name = jobParameter.Name,
                Value = jobParameter.Value
            };
            DoCustomMappings(jobParameter, jobParameterDto);
            return jobParameterDto;
        }

        static partial void DoCustomMappings(JobParameter jobParameter, JobParameterDto jobParameterDto);

    }
}