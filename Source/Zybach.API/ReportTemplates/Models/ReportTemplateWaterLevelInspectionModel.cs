using System;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.ReportTemplates.Models
{
    public class ReportTemplateWaterLevelInspectionModel : ReportTemplateBaseModel
    {
        public int WaterLevelInspectionID { get; set; }
        public DateTime InspectionDate { get; set; }
        public int InspectorUserID { get; set; }

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

        public string CropTypeName { get; set; }

        public string InspectionNotes { get; set; }
        public string InspectionNickname { get; set; }

        public ReportTemplateWaterLevelInspectionModel(WaterLevelInspectionSimpleDto waterLevelInspection)
        {
            WaterLevelInspectionID = waterLevelInspection.WaterLevelInspectionID;
            InspectionDate = waterLevelInspection.InspectionDate;
            InspectorUserID = waterLevelInspection.InspectorUserID;

            WaterLevelInspectionStatus = waterLevelInspection.WaterLevelInspectionStatus;
            MeasuringEquipment = waterLevelInspection.MeasuringEquipment;
            HasOil = waterLevelInspection.HasOil;
            HasBrokenTape = waterLevelInspection.HasBrokenTape;
            Crop = waterLevelInspection.Crop;
            Accuracy = waterLevelInspection.Accuracy;

            SourceAgency = waterLevelInspection.SourceAgency;
            SourceCode = waterLevelInspection.SourceCode;
            TimeDatumCode = waterLevelInspection.TimeDatumCode;
            TimeDatumReliability = waterLevelInspection.TimeDatumReliability;
            LevelTypeCode = waterLevelInspection.LevelTypeCode;
            AgencyCode = waterLevelInspection.AgencyCode;
            Access = waterLevelInspection.Access;

            Hold = waterLevelInspection.Hold;
            Cut = waterLevelInspection.Cut;
            MP = waterLevelInspection.MP;

            Measurement = waterLevelInspection.Measurement;

            IsPrimary = waterLevelInspection.IsPrimary;
            WaterLevel = waterLevelInspection.WaterLevel;

            CropTypeName = waterLevelInspection.CropTypeName;
            InspectionNotes = waterLevelInspection.InspectionNotes;
            InspectionNickname = waterLevelInspection.InspectionNickname;
        }
    }
}
