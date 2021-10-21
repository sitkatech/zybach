//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellIrrigatedAcreStaging]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class WellIrrigatedAcreStagingExtensionMethods
    {
        public static WellIrrigatedAcreStagingDto AsDto(this WellIrrigatedAcreStaging wellIrrigatedAcreStaging)
        {
            var wellIrrigatedAcreStagingDto = new WellIrrigatedAcreStagingDto()
            {
                WellIrrigatedAcreStagingID = wellIrrigatedAcreStaging.WellIrrigatedAcreStagingID,
                WellRegistrationID = wellIrrigatedAcreStaging.WellRegistrationID,
                IrrigationYear = wellIrrigatedAcreStaging.IrrigationYear,
                Acres = wellIrrigatedAcreStaging.Acres
            };
            DoCustomMappings(wellIrrigatedAcreStaging, wellIrrigatedAcreStagingDto);
            return wellIrrigatedAcreStagingDto;
        }

        static partial void DoCustomMappings(WellIrrigatedAcreStaging wellIrrigatedAcreStaging, WellIrrigatedAcreStagingDto wellIrrigatedAcreStagingDto);

    }
}