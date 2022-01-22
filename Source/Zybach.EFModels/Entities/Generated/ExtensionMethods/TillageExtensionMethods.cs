//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[Tillage]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class TillageExtensionMethods
    {
        public static TillageDto AsDto(this Tillage tillage)
        {
            var tillageDto = new TillageDto()
            {
                TillageID = tillage.TillageID,
                TillageName = tillage.TillageName,
                TillageDisplayName = tillage.TillageDisplayName
            };
            DoCustomMappings(tillage, tillageDto);
            return tillageDto;
        }

        static partial void DoCustomMappings(Tillage tillage, TillageDto tillageDto);

        public static TillageSimpleDto AsSimpleDto(this Tillage tillage)
        {
            var tillageSimpleDto = new TillageSimpleDto()
            {
                TillageID = tillage.TillageID,
                TillageName = tillage.TillageName,
                TillageDisplayName = tillage.TillageDisplayName
            };
            DoCustomSimpleDtoMappings(tillage, tillageSimpleDto);
            return tillageSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(Tillage tillage, TillageSimpleDto tillageSimpleDto);
    }
}