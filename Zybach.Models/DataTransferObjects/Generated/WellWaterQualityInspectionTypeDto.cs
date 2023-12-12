//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WellWaterQualityInspectionType]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class WellWaterQualityInspectionTypeDto
    {
        public int WellWaterQualityInspectionTypeID { get; set; }
        public WellDto Well { get; set; }
        public WaterQualityInspectionTypeDto WaterQualityInspectionType { get; set; }
    }

    public partial class WellWaterQualityInspectionTypeSimpleDto
    {
        public int WellWaterQualityInspectionTypeID { get; set; }
        public System.Int32 WellID { get; set; }
        public System.Int32 WaterQualityInspectionTypeID { get; set; }
    }

}