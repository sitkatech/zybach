//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WaterQualityInspection]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class WaterQualityInspectionDto
    {
        public int WaterQualityInspectionID { get; set; }
        public WellDto Well { get; set; }
        public WaterQualityInspectionTypeDto WaterQualityInspectionType { get; set; }
        public DateTime InspectionDate { get; set; }
        public UserDto InspectorUser { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? PH { get; set; }
        public decimal? Conductivity { get; set; }
        public decimal? FieldAlkilinity { get; set; }
        public decimal? FieldNitrates { get; set; }
        public decimal? LabNitrates { get; set; }
        public decimal? Salinity { get; set; }
        public decimal? MV { get; set; }
        public decimal? Sodium { get; set; }
        public decimal? Calcium { get; set; }
        public decimal? Magnesium { get; set; }
        public decimal? Potassium { get; set; }
        public decimal? HydrogenCarbonate { get; set; }
        public decimal? CalciumCarbonate { get; set; }
        public decimal? Sulfate { get; set; }
        public decimal? Chloride { get; set; }
        public decimal? SiliconDioxide { get; set; }
        public CropTypeDto CropType { get; set; }
        public decimal? PreWaterLevel { get; set; }
        public decimal? PostWaterLevel { get; set; }
        public string InspectionNotes { get; set; }
        public string InspectionNickname { get; set; }
    }

    public partial class WaterQualityInspectionSimpleDto
    {
        public int WaterQualityInspectionID { get; set; }
        public int WellID { get; set; }
        public int WaterQualityInspectionTypeID { get; set; }
        public DateTime InspectionDate { get; set; }
        public int InspectorUserID { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? PH { get; set; }
        public decimal? Conductivity { get; set; }
        public decimal? FieldAlkilinity { get; set; }
        public decimal? FieldNitrates { get; set; }
        public decimal? LabNitrates { get; set; }
        public decimal? Salinity { get; set; }
        public decimal? MV { get; set; }
        public decimal? Sodium { get; set; }
        public decimal? Calcium { get; set; }
        public decimal? Magnesium { get; set; }
        public decimal? Potassium { get; set; }
        public decimal? HydrogenCarbonate { get; set; }
        public decimal? CalciumCarbonate { get; set; }
        public decimal? Sulfate { get; set; }
        public decimal? Chloride { get; set; }
        public decimal? SiliconDioxide { get; set; }
        public int? CropTypeID { get; set; }
        public decimal? PreWaterLevel { get; set; }
        public decimal? PostWaterLevel { get; set; }
        public string InspectionNotes { get; set; }
        public string InspectionNickname { get; set; }
    }
}