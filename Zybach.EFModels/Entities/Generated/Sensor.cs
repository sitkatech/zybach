using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("Sensor")]
    [Index("SensorName", Name = "AK_Sensor_SensorName", IsUnique = true)]
    public partial class Sensor
    {
        public Sensor()
        {
            SensorAnomalies = new HashSet<SensorAnomaly>();
            SupportTickets = new HashSet<SupportTicket>();
        }

        [Key]
        public int SensorID { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string SensorName { get; set; }
        public int SensorTypeID { get; set; }
        public int? WellID { get; set; }
        public bool InGeoOptix { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime LastUpdateDate { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RetirementDate { get; set; }

        [ForeignKey("WellID")]
        [InverseProperty("Sensors")]
        public virtual Well Well { get; set; }
        [InverseProperty("Sensor")]
        public virtual ICollection<SensorAnomaly> SensorAnomalies { get; set; }
        [InverseProperty("Sensor")]
        public virtual ICollection<SupportTicket> SupportTickets { get; set; }
    }
}
