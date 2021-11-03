//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [HangFire].[AggregatedCounter]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AggregatedCounterExtensionMethods
    {
        public static AggregatedCounterDto AsDto(this AggregatedCounter aggregatedCounter)
        {
            var aggregatedCounterDto = new AggregatedCounterDto()
            {
                Key = aggregatedCounter.Key,
                Value = aggregatedCounter.Value,
                ExpireAt = aggregatedCounter.ExpireAt
            };
            DoCustomMappings(aggregatedCounter, aggregatedCounterDto);
            return aggregatedCounterDto;
        }

        static partial void DoCustomMappings(AggregatedCounter aggregatedCounter, AggregatedCounterDto aggregatedCounterDto);

    }
}