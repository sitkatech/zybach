//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WaterQualityInspection]
namespace Zybach.EFModels.Entities
{
    public partial class WaterQualityInspection
    {
        public WaterQualityInspectionType WaterQualityInspectionType => WaterQualityInspectionType.AllLookupDictionary[WaterQualityInspectionTypeID];
    }
}