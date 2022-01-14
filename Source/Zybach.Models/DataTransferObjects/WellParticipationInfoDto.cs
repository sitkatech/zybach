using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zybach.Models.DataTransferObjects
{
    public class WellParticipationInfoDto
    {
        public string WellRegistrationID { get; set; }
        public int? WellParticipationID { get; set; }
        public string WellParticipationName { get; set; }
        public int? WellUseID { get; set; }
        public string WellUseName { get; set; }
        [Required]
        public bool RequiresChemigation { get; set; }
        [Required]
        public bool RequiresWaterLevelInspection { get; set; }
        [Required]
        public List<int> WaterQualityInspectionTypeIDs { get; set; }
        public bool IsReplacement { get; set; }
        [RegularExpression(@"^\d{0,6}\.?\d{0,4}|\.\d{0,4}$", ErrorMessage = "Well Depth must be a number between 0 and 999999.9999, with at most 4 digits after the decimal")]
        [Range(typeof(decimal), "0", "999999.9999", ErrorMessage = "Well Depth must be a number between 0 and 999999.9999, with at most 4 digits after the decimal")]
        public decimal? WellDepth { get; set; }
        [StringLength(100, ErrorMessage = "Clearinghouse cannot exceed 100 characters. ")]
        public string Clearinghouse { get; set; }
        [RegularExpression(@"^\d+$", ErrorMessage = "Page number must be a positive number")]
        [Range(typeof(int), "0", "2147483647", ErrorMessage = "Page number must be a positive number")]
        public int? PageNumber { get; set; }
        [StringLength(100, ErrorMessage = "Site Name cannot exceed 100 characters. ")]
        public string SiteName { get; set; }
        [StringLength(100, ErrorMessage = "Site Number cannot exceed 100 characters. ")]
        public string SiteNumber { get; set; }
        [StringLength(100, ErrorMessage = "Screen Interval cannot exceed 100 characters. ")]
        public string ScreenInterval { get; set; }
        [RegularExpression(@"^\d{0,6}\.?\d{0,4}|\.\d{0,4}$", ErrorMessage = "Screen Depth must be a number between 0 and 999999.9999, with at most 4 digits after the decimal")]
        [Range(typeof(decimal), "0", "999999.9999", ErrorMessage = "Screen Depth must be a number between 0 and 999999.9999, with at most 4 digits after the decimal")]
        public decimal? ScreenDepth { get; set; }
    }
}
