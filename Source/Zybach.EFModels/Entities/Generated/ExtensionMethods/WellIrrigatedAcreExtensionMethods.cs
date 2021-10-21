//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellIrrigatedAcre]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WellIrrigatedAcreExtensionMethods
    {
        public static WellIrrigatedAcreDto AsDto(this WellIrrigatedAcre wellIrrigatedAcre)
        {
            var wellIrrigatedAcreDto = new WellIrrigatedAcreDto()
            {
                WellIrrigatedAcreID = wellIrrigatedAcre.WellIrrigatedAcreID,
                Well = wellIrrigatedAcre.Well.AsDto(),
                IrrigationYear = wellIrrigatedAcre.IrrigationYear,
                Acres = wellIrrigatedAcre.Acres
            };
            DoCustomMappings(wellIrrigatedAcre, wellIrrigatedAcreDto);
            return wellIrrigatedAcreDto;
        }

        static partial void DoCustomMappings(WellIrrigatedAcre wellIrrigatedAcre, WellIrrigatedAcreDto wellIrrigatedAcreDto);

    }
}