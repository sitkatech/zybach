//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellSensorMeasurementStaging]
namespace Zybach.EFModels.Entities
{
    public partial class WellSensorMeasurementStaging
    {
        public MeasurementType MeasurementType => MeasurementType.AllLookupDictionary[MeasurementTypeID];
    }
}