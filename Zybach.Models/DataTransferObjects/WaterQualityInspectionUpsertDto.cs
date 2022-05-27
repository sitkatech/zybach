using System;
using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class WaterQualityInspectionUpsertDto
    {
        [Display(Name = "Registration ID")]
        [Required]
        public string WellRegistrationID { get; set; }
        [Display(Name = "Inspection Type")]
        [Required]
        public int WaterQualityInspectionTypeID { get; set; }
        [Display(Name = "Inspection Date")]
        [Required]
        public DateTime InspectionDate { get; set; }
        [Display(Name = "Inspector")]
        [Required]
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