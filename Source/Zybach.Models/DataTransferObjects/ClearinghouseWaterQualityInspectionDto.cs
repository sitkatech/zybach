using System;

namespace Zybach.Models.DataTransferObjects
{
    public class ClearinghouseWaterQualityInspectionDto
    {
        public string WellRegistrationID { get; set; }
        public string? WellNickname { get; set; }
        public string Clearinghouse { get; set; }
        public string Contaminant { get; set; }
        public DateTime SamplingDate { get; set; }
        public decimal? LabConcentration { get; set; }
        public decimal? FieldConcentration { get; set; }
        public decimal ReportingLimit { get; set; }
        public string Laboratory { get; set; }
        public bool IsReplacement { get; set; }
        public bool RequiresChemigationInspection { get; set; }
        public bool RequiresWaterQualityInspection { get; set; }
        public string TownshipRangeSection { get; set; }
        public decimal? WellDepth { get; set; }
        public string? WellParticipation { get; set; }
        public string? WellUse { get; set; }
        public decimal? WellPreLevel { get; set; }
        public decimal? WellPostLevel { get; set; }
        public string ScreenInterval { get; set; }
    }
}
