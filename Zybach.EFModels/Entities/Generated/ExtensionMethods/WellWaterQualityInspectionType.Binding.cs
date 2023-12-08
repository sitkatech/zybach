//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellWaterQualityInspectionType]
namespace Zybach.EFModels.Entities
{
    public partial class WellWaterQualityInspectionType
    {
        public int PrimaryKey => WellWaterQualityInspectionTypeID;
        public WaterQualityInspectionType WaterQualityInspectionType => WaterQualityInspectionType.AllLookupDictionary[WaterQualityInspectionTypeID];

        public static class FieldLengths
        {

        }
    }
}