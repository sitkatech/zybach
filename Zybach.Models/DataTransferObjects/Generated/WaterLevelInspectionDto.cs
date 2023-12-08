//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[WaterLevelInspection]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class WaterLevelInspectionDto
    {
        public int WaterLevelInspectionID { get; set; }
        public WellDto Well { get; set; }
        public DateTime InspectionDate { get; set; }
        public UserDto InspectorUser { get; set; }
        public string WaterLevelInspectionStatus { get; set; }
        public string MeasuringEquipment { get; set; }
        public string Crop { get; set; }
        public bool HasOil { get; set; }
        public bool HasBrokenTape { get; set; }
        public string Accuracy { get; set; }
        public string Method { get; set; }
        public string Party { get; set; }
        public string SourceAgency { get; set; }
        public string SourceCode { get; set; }
        public string TimeDatumCode { get; set; }
        public string TimeDatumReliability { get; set; }
        public string LevelTypeCode { get; set; }
        public string AgencyCode { get; set; }
        public string Access { get; set; }
        public decimal? Hold { get; set; }
        public decimal? Cut { get; set; }
        public decimal? MP { get; set; }
        public decimal? Measurement { get; set; }
        public bool? IsPrimary { get; set; }
        public decimal? WaterLevel { get; set; }
        public CropTypeDto CropType { get; set; }
        public string InspectionNotes { get; set; }
        public string InspectionNickname { get; set; }
    }

    public partial class WaterLevelInspectionSimpleDto
    {
        public int WaterLevelInspectionID { get; set; }
        public System.Int32 WellID { get; set; }
        public DateTime InspectionDate { get; set; }
        public System.Int32 InspectorUserID { get; set; }
        public string WaterLevelInspectionStatus { get; set; }
        public string MeasuringEquipment { get; set; }
        public string Crop { get; set; }
        public bool HasOil { get; set; }
        public bool HasBrokenTape { get; set; }
        public string Accuracy { get; set; }
        public string Method { get; set; }
        public string Party { get; set; }
        public string SourceAgency { get; set; }
        public string SourceCode { get; set; }
        public string TimeDatumCode { get; set; }
        public string TimeDatumReliability { get; set; }
        public string LevelTypeCode { get; set; }
        public string AgencyCode { get; set; }
        public string Access { get; set; }
        public decimal? Hold { get; set; }
        public decimal? Cut { get; set; }
        public decimal? MP { get; set; }
        public decimal? Measurement { get; set; }
        public bool? IsPrimary { get; set; }
        public decimal? WaterLevel { get; set; }
        public System.Int32? CropTypeID { get; set; }
        public string InspectionNotes { get; set; }
        public string InspectionNickname { get; set; }
    }

}