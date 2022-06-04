using System;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.ReportTemplates.Models
{
    public class ReportTemplateWaterQualityInspectionModel : ReportTemplateBaseModel
    {
        public int WaterQualityInspectionID { get; set; }
        public string WaterQualityInspectionTypeName { get; set; }
        public int InspectionYear { get; set; }
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

        public string CropTypeName { get; set; }

        public string InspectionNotes { get; set; }
        public string InspectionNickname { get; set; }

        public ReportTemplateWaterQualityInspectionModel(WaterQualityInspectionSimpleDto waterQualityInspection)
        {
            WaterQualityInspectionID = waterQualityInspection.WaterQualityInspectionID;
            WaterQualityInspectionTypeName = waterQualityInspection.WaterQualityInspectionTypeName;
            InspectionDate = waterQualityInspection.InspectionDate;
            InspectionYear = waterQualityInspection.InspectionYear;
            InspectorUserID = waterQualityInspection.InspectorUserID;

            Temperature = waterQualityInspection.Temperature;
            PH = waterQualityInspection.PH;
            Conductivity = waterQualityInspection.Conductivity;
            FieldAlkilinity = waterQualityInspection.FieldAlkilinity;
            FieldNitrates = waterQualityInspection.FieldNitrates;
            LabNitrates = waterQualityInspection.LabNitrates;
            Salinity = waterQualityInspection.Salinity;
            MV = waterQualityInspection.MV;
            Sodium = waterQualityInspection.Sodium;
            Calcium = waterQualityInspection.Calcium;
            Magnesium = waterQualityInspection.Magnesium;
            Potassium = waterQualityInspection.Potassium;
            HydrogenCarbonate = waterQualityInspection.HydrogenCarbonate;
            CalciumCarbonate = waterQualityInspection.CalciumCarbonate;
            Sulfate = waterQualityInspection.Sulfate;
            Chloride = waterQualityInspection.Chloride;
            SiliconDioxide = waterQualityInspection.SiliconDioxide;
            CropTypeName = waterQualityInspection.CropTypeName;
            InspectionNotes = waterQualityInspection.InspectionNotes;
            InspectionNickname = waterQualityInspection.InspectionNickname;
        }
    }
}