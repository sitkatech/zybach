//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[AgHubIrrigationUnitOpenETDatum]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class AgHubIrrigationUnitOpenETDatumExtensionMethods
    {
        public static AgHubIrrigationUnitOpenETDatumDto AsDto(this AgHubIrrigationUnitOpenETDatum agHubIrrigationUnitOpenETDatum)
        {
            var agHubIrrigationUnitOpenETDatumDto = new AgHubIrrigationUnitOpenETDatumDto()
            {
                AgHubIrrigationUnitOpenETDatumID = agHubIrrigationUnitOpenETDatum.AgHubIrrigationUnitOpenETDatumID,
                AgHubIrrigationUnit = agHubIrrigationUnitOpenETDatum.AgHubIrrigationUnit.AsDto(),
                OpenETDataType = agHubIrrigationUnitOpenETDatum.OpenETDataType.AsDto(),
                ReportedDate = agHubIrrigationUnitOpenETDatum.ReportedDate,
                ReportedValueInches = agHubIrrigationUnitOpenETDatum.ReportedValueInches,
                ReportedValueAcreInches = agHubIrrigationUnitOpenETDatum.ReportedValueAcreInches
            };
            DoCustomMappings(agHubIrrigationUnitOpenETDatum, agHubIrrigationUnitOpenETDatumDto);
            return agHubIrrigationUnitOpenETDatumDto;
        }

        static partial void DoCustomMappings(AgHubIrrigationUnitOpenETDatum agHubIrrigationUnitOpenETDatum, AgHubIrrigationUnitOpenETDatumDto agHubIrrigationUnitOpenETDatumDto);

        public static AgHubIrrigationUnitOpenETDatumSimpleDto AsSimpleDto(this AgHubIrrigationUnitOpenETDatum agHubIrrigationUnitOpenETDatum)
        {
            var agHubIrrigationUnitOpenETDatumSimpleDto = new AgHubIrrigationUnitOpenETDatumSimpleDto()
            {
                AgHubIrrigationUnitOpenETDatumID = agHubIrrigationUnitOpenETDatum.AgHubIrrigationUnitOpenETDatumID,
                AgHubIrrigationUnitID = agHubIrrigationUnitOpenETDatum.AgHubIrrigationUnitID,
                OpenETDataTypeID = agHubIrrigationUnitOpenETDatum.OpenETDataTypeID,
                ReportedDate = agHubIrrigationUnitOpenETDatum.ReportedDate,
                ReportedValueInches = agHubIrrigationUnitOpenETDatum.ReportedValueInches,
                ReportedValueAcreInches = agHubIrrigationUnitOpenETDatum.ReportedValueAcreInches
            };
            DoCustomSimpleDtoMappings(agHubIrrigationUnitOpenETDatum, agHubIrrigationUnitOpenETDatumSimpleDto);
            return agHubIrrigationUnitOpenETDatumSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(AgHubIrrigationUnitOpenETDatum agHubIrrigationUnitOpenETDatum, AgHubIrrigationUnitOpenETDatumSimpleDto agHubIrrigationUnitOpenETDatumSimpleDto);
    }
}