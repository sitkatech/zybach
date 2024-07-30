//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigationUnitCurveNumber]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AgHubIrrigationUnitCurveNumberExtensionMethods
    {
        public static AgHubIrrigationUnitCurveNumberDto AsDto(this AgHubIrrigationUnitCurveNumber agHubIrrigationUnitCurveNumber)
        {
            var agHubIrrigationUnitCurveNumberDto = new AgHubIrrigationUnitCurveNumberDto()
            {
                AgHubIrrigationUnitCurveNumberID = agHubIrrigationUnitCurveNumber.AgHubIrrigationUnitCurveNumberID,
                AgHubIrrigationUnit = agHubIrrigationUnitCurveNumber.AgHubIrrigationUnit.AsDto(),
                HydrologicSoilGroup = agHubIrrigationUnitCurveNumber.HydrologicSoilGroup,
                MTillCurveNumber = agHubIrrigationUnitCurveNumber.MTillCurveNumber,
                STillCurveNumber = agHubIrrigationUnitCurveNumber.STillCurveNumber,
                NTillCurveNumber = agHubIrrigationUnitCurveNumber.NTillCurveNumber,
                CTillCurveNumber = agHubIrrigationUnitCurveNumber.CTillCurveNumber,
                UndefinedTillCurveNumber = agHubIrrigationUnitCurveNumber.UndefinedTillCurveNumber
            };
            DoCustomMappings(agHubIrrigationUnitCurveNumber, agHubIrrigationUnitCurveNumberDto);
            return agHubIrrigationUnitCurveNumberDto;
        }

        static partial void DoCustomMappings(AgHubIrrigationUnitCurveNumber agHubIrrigationUnitCurveNumber, AgHubIrrigationUnitCurveNumberDto agHubIrrigationUnitCurveNumberDto);

        public static AgHubIrrigationUnitCurveNumberSimpleDto AsSimpleDto(this AgHubIrrigationUnitCurveNumber agHubIrrigationUnitCurveNumber)
        {
            var agHubIrrigationUnitCurveNumberSimpleDto = new AgHubIrrigationUnitCurveNumberSimpleDto()
            {
                AgHubIrrigationUnitCurveNumberID = agHubIrrigationUnitCurveNumber.AgHubIrrigationUnitCurveNumberID,
                AgHubIrrigationUnitID = agHubIrrigationUnitCurveNumber.AgHubIrrigationUnitID,
                HydrologicSoilGroup = agHubIrrigationUnitCurveNumber.HydrologicSoilGroup,
                MTillCurveNumber = agHubIrrigationUnitCurveNumber.MTillCurveNumber,
                STillCurveNumber = agHubIrrigationUnitCurveNumber.STillCurveNumber,
                NTillCurveNumber = agHubIrrigationUnitCurveNumber.NTillCurveNumber,
                CTillCurveNumber = agHubIrrigationUnitCurveNumber.CTillCurveNumber,
                UndefinedTillCurveNumber = agHubIrrigationUnitCurveNumber.UndefinedTillCurveNumber
            };
            DoCustomSimpleDtoMappings(agHubIrrigationUnitCurveNumber, agHubIrrigationUnitCurveNumberSimpleDto);
            return agHubIrrigationUnitCurveNumberSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(AgHubIrrigationUnitCurveNumber agHubIrrigationUnitCurveNumber, AgHubIrrigationUnitCurveNumberSimpleDto agHubIrrigationUnitCurveNumberSimpleDto);
    }
}