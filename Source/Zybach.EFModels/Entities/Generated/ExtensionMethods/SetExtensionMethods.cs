//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [HangFire].[Set]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class SetExtensionMethods
    {
        public static SetDto AsDto(this Set set)
        {
            var setDto = new SetDto()
            {
                Key = set.Key,
                Score = set.Score,
                Value = set.Value,
                ExpireAt = set.ExpireAt
            };
            DoCustomMappings(set, setDto);
            return setDto;
        }

        static partial void DoCustomMappings(Set set, SetDto setDto);

    }
}