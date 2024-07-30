//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigationUnitRunoff]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AgHubIrrigationUnitRunoffExtensionMethods
    {
        public static AgHubIrrigationUnitRunoffDto AsDto(this AgHubIrrigationUnitRunoff agHubIrrigationUnitRunoff)
        {
            var agHubIrrigationUnitRunoffDto = new AgHubIrrigationUnitRunoffDto()
            {
                AgHubIrrigationUnitRunoffID = agHubIrrigationUnitRunoff.AgHubIrrigationUnitRunoffID,
                AgHubIrrigationUnit = agHubIrrigationUnitRunoff.AgHubIrrigationUnit.AsDto(),
                Year = agHubIrrigationUnitRunoff.Year,
                Month = agHubIrrigationUnitRunoff.Month,
                Day = agHubIrrigationUnitRunoff.Day,
                CropType = agHubIrrigationUnitRunoff.CropType,
                Tillage = agHubIrrigationUnitRunoff.Tillage,
                CurveNumber = agHubIrrigationUnitRunoff.CurveNumber,
                Precipitation = agHubIrrigationUnitRunoff.Precipitation,
                Area = agHubIrrigationUnitRunoff.Area,
                RunoffDepth = agHubIrrigationUnitRunoff.RunoffDepth,
                RunoffVolume = agHubIrrigationUnitRunoff.RunoffVolume
            };
            DoCustomMappings(agHubIrrigationUnitRunoff, agHubIrrigationUnitRunoffDto);
            return agHubIrrigationUnitRunoffDto;
        }

        static partial void DoCustomMappings(AgHubIrrigationUnitRunoff agHubIrrigationUnitRunoff, AgHubIrrigationUnitRunoffDto agHubIrrigationUnitRunoffDto);

        public static AgHubIrrigationUnitRunoffSimpleDto AsSimpleDto(this AgHubIrrigationUnitRunoff agHubIrrigationUnitRunoff)
        {
            var agHubIrrigationUnitRunoffSimpleDto = new AgHubIrrigationUnitRunoffSimpleDto()
            {
                AgHubIrrigationUnitRunoffID = agHubIrrigationUnitRunoff.AgHubIrrigationUnitRunoffID,
                AgHubIrrigationUnitID = agHubIrrigationUnitRunoff.AgHubIrrigationUnitID,
                Year = agHubIrrigationUnitRunoff.Year,
                Month = agHubIrrigationUnitRunoff.Month,
                Day = agHubIrrigationUnitRunoff.Day,
                CropType = agHubIrrigationUnitRunoff.CropType,
                Tillage = agHubIrrigationUnitRunoff.Tillage,
                CurveNumber = agHubIrrigationUnitRunoff.CurveNumber,
                Precipitation = agHubIrrigationUnitRunoff.Precipitation,
                Area = agHubIrrigationUnitRunoff.Area,
                RunoffDepth = agHubIrrigationUnitRunoff.RunoffDepth,
                RunoffVolume = agHubIrrigationUnitRunoff.RunoffVolume
            };
            DoCustomSimpleDtoMappings(agHubIrrigationUnitRunoff, agHubIrrigationUnitRunoffSimpleDto);
            return agHubIrrigationUnitRunoffSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(AgHubIrrigationUnitRunoff agHubIrrigationUnitRunoff, AgHubIrrigationUnitRunoffSimpleDto agHubIrrigationUnitRunoffSimpleDto);
    }
}