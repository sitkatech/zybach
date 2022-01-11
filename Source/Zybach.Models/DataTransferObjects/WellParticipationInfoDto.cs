using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "decimal(10, 4)")]
        public decimal? WellDepth { get; set; }
        [StringLength(100, ErrorMessage = "Clearinghouse cannot exceed 100 characters. ")]
        public string? Clearinghouse { get; set; }
        [Column(TypeName = "int")]
        public int? PageNumber { get; set; }
        [StringLength(100, ErrorMessage = "Site Name cannot exceed 100 characters. ")]
        public string? SiteName { get; set; }
        [StringLength(100, ErrorMessage = "Site Number cannot exceed 100 characters. ")]
        public string? SiteNumber { get; set; }
        [StringLength(100, ErrorMessage = "Screen Interval cannot exceed 100 characters. ")]
        public string? ScreenInterval { get; set; }
        [Column(TypeName = "decimal(10, 4)")]
        public decimal? ScreenDepth { get; set; }
    }
}
