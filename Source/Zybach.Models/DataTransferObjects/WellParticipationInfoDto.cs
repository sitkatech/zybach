using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class WellParticipationInfoDto
    {
        public int? WellParticipationID { get; set; }
        public string? WellParticipationName { get; set; }
        public int? WellUseID { get; set; }
        public string? WellUseName { get; set; }
        [Required]
        public bool RequiresChemigation { get; set; }
        [Required]
        public bool RequiresWaterLevelInspection { get; set; }
        [Required]
        public List<int> WaterQualityInspectionTypeIDs { get; set; }
        public bool IsReplacement { get; set; }
        public decimal? WellDepth { get; set; }
        public string? Clearinghouse { get; set; }
        public int? PageNumber { get; set; }
        public string? SiteName { get; set; }
        public string? SiteNumber { get; set; }
        public string? ScreenInterval { get; set; }
        public decimal? ScreenDepth { get; set; }
    }
}
