//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellSensorMeasurement]
namespace Zybach.EFModels.Entities
{
    public partial class WellSensorMeasurement
    {
        public MeasurementType MeasurementType => MeasurementType.AllLookupDictionary[MeasurementTypeID];
    }
}