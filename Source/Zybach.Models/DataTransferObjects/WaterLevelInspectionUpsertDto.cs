using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zybach.Models.DataTransferObjects
{
    public class WaterLevelInspectionUpsertDto
    {
        [Required]
        public string WellRegistrationID { get; set; }
        [Required]
        public DateTime? InspectionDate { get; set; }
        [Required]
        public int? InspectorUserID { get; set; }
        [RegularExpression(@"^\d{0,8}\.?\d{0,4}|\.\d{0,4}$", ErrorMessage = "Measurement must be a number between 0 and 99999999.9999, with at most 4 digits after the decimal")]
        [Range(typeof(decimal), "0", "99999999.9999", ErrorMessage = "Measurement must be a number between 0 and 99999999.9999, with at most 4 digits after the decimal")]
        public decimal? Measurement { get; set; }
        public string? MeasuringEquipment { get; set; }
        [Required]
        public bool HasOil { get; set; }
        [Required]
        public bool HasBrokenTape { get; set; }
        [StringLength(100, ErrorMessage = "Inspection Nickname cannot exceed 100 characters.")]
        public string InspectionNickname { get; set; }
        [StringLength(500, ErrorMessage = "Inspection Notes cannot exceed 500 characters.")]
        public string InspectionNotes { get; set; }
    }
}
