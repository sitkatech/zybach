//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [HangFire].[List]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ListExtensionMethods
    {
        public static ListDto AsDto(this List list)
        {
            var listDto = new ListDto()
            {
                Id = list.Id,
                Key = list.Key,
                Value = list.Value,
                ExpireAt = list.ExpireAt
            };
            DoCustomMappings(list, listDto);
            return listDto;
        }

        static partial void DoCustomMappings(List list, ListDto listDto);

    }
}