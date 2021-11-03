//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [HangFire].[Hash]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class HashExtensionMethods
    {
        public static HashDto AsDto(this Hash hash)
        {
            var hashDto = new HashDto()
            {
                Key = hash.Key,
                Field = hash.Field,
                Value = hash.Value,
                ExpireAt = hash.ExpireAt
            };
            DoCustomMappings(hash, hashDto);
            return hashDto;
        }

        static partial void DoCustomMappings(Hash hash, HashDto hashDto);

    }
}