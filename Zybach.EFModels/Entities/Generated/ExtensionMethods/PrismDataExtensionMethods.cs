//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PrismData]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class PrismDataExtensionMethods
    {
        public static PrismDataDto AsDto(this PrismDatum prismData)
        {
            var prismDataDto = new PrismDataDto()
            {
                PrismDataID = prismData.PrismDataID,
                ElementType = prismData.ElementType,
                Date = prismData.Date,
                X = prismData.X,
                Y = prismData.Y,
                Value = prismData.Value
            };
            DoCustomMappings(prismData, prismDataDto);
            return prismDataDto;
        }

        static partial void DoCustomMappings(PrismDatum prismData, PrismDataDto prismDataDto);
         
        public static PrismDataSimpleDto AsSimpleDto(this PrismDatum prismData)
        {
            var prismDataSimpleDto = new PrismDataSimpleDto()
            {
                PrismDataID = prismData.PrismDataID,
                ElementType = prismData.ElementType,
                Date = prismData.Date,
                X = prismData.X,
                Y = prismData.Y,
                Value = prismData.Value
            };
            DoCustomSimpleDtoMappings(prismData, prismDataSimpleDto);
            return prismDataSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(PrismDatum prismData, PrismDataSimpleDto prismDataSimpleDto);
    }
}