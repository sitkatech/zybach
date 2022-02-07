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
        public DateTime InspectionDate { get; set; }
        [Required]
        public int InspectorUserID { get; set; }
        public decimal Measurement { get; set; }
        [Required]
        public bool HasOil { get; set; }
        [Required]
        public bool HasBrokenTape { get; set; }
        [StringLength(500, ErrorMessage = "Inspection Notes cannot exceed 500 characters.")]
        public string InspectionNotes { get; set; }
    }
}
