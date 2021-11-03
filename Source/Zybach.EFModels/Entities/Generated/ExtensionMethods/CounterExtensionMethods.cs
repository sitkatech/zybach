//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [HangFire].[Counter]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class CounterExtensionMethods
    {
        public static CounterDto AsDto(this Counter counter)
        {
            var counterDto = new CounterDto()
            {
                Key = counter.Key,
                Value = counter.Value,
                ExpireAt = counter.ExpireAt
            };
            DoCustomMappings(counter, counterDto);
            return counterDto;
        }

        static partial void DoCustomMappings(Counter counter, CounterDto counterDto);

    }
}