//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[Well]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WellExtensionMethods
    {
        public static WellDto AsDto(this Well well)
        {
            var wellDto = new WellDto()
            {
                WellID = well.WellID,
                WellRegistrationID = well.WellRegistrationID,
                StreamflowZone = well.StreamflowZone?.AsDto(),
                CreateDate = well.CreateDate,
                LastUpdateDate = well.LastUpdateDate
            };
            DoCustomMappings(well, wellDto);
            return wellDto;
        }

        static partial void DoCustomMappings(Well well, WellDto wellDto);

    }
}