//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubWellIrrigatedAcreStaging]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AgHubWellIrrigatedAcreStagingExtensionMethods
    {
        public static AgHubWellIrrigatedAcreStagingDto AsDto(this AgHubWellIrrigatedAcreStaging agHubWellIrrigatedAcreStaging)
        {
            var agHubWellIrrigatedAcreStagingDto = new AgHubWellIrrigatedAcreStagingDto()
            {
                AgHubWellIrrigatedAcreStagingID = agHubWellIrrigatedAcreStaging.AgHubWellIrrigatedAcreStagingID,
                WellRegistrationID = agHubWellIrrigatedAcreStaging.WellRegistrationID,
                IrrigationYear = agHubWellIrrigatedAcreStaging.IrrigationYear,
                Acres = agHubWellIrrigatedAcreStaging.Acres
            };
            DoCustomMappings(agHubWellIrrigatedAcreStaging, agHubWellIrrigatedAcreStagingDto);
            return agHubWellIrrigatedAcreStagingDto;
        }

        static partial void DoCustomMappings(AgHubWellIrrigatedAcreStaging agHubWellIrrigatedAcreStaging, AgHubWellIrrigatedAcreStagingDto agHubWellIrrigatedAcreStagingDto);

    }
}