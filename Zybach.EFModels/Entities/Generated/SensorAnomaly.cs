using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("SensorAnomaly")]
    public partial class SensorAnomaly
    {
        [Key]
        public int SensorAnomalyID { get; set; }
        public int SensorID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime EndDate { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string Notes { get; set; }

        [ForeignKey("SensorID")]
        [InverseProperty("SensorAnomalies")]
        public virtual Sensor Sensor { get; set; }
    }
}
