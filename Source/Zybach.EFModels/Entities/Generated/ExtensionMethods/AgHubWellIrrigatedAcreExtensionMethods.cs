//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubWellIrrigatedAcre]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AgHubWellIrrigatedAcreExtensionMethods
    {
        public static AgHubWellIrrigatedAcreDto AsDto(this AgHubWellIrrigatedAcre agHubWellIrrigatedAcre)
        {
            var agHubWellIrrigatedAcreDto = new AgHubWellIrrigatedAcreDto()
            {
                AgHubWellIrrigatedAcreID = agHubWellIrrigatedAcre.AgHubWellIrrigatedAcreID,
                AgHubWell = agHubWellIrrigatedAcre.AgHubWell.AsDto(),
                IrrigationYear = agHubWellIrrigatedAcre.IrrigationYear,
                Acres = agHubWellIrrigatedAcre.Acres,
            };
            DoCustomMappings(agHubWellIrrigatedAcre, agHubWellIrrigatedAcreDto);
            return agHubWellIrrigatedAcreDto;
        }

        static partial void DoCustomMappings(AgHubWellIrrigatedAcre agHubWellIrrigatedAcre, AgHubWellIrrigatedAcreDto agHubWellIrrigatedAcreDto);

    }
}