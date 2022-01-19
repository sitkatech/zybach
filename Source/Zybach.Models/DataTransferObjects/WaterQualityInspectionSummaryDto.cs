using System;

namespace Zybach.Models.DataTransferObjects
{
    public class WaterQualityInspectionSummaryDto
    {
        public int WaterQualityInspectionID { get; set; }
        public DateTime InspectionDate { get; set; }
        public string InspectionType { get; set; }
        public string? InspectionNotes { get; set; }
    }
}

