using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("OpenETWaterMeasurement")]
    public partial class OpenETWaterMeasurement
    {
        [Key]
        public int OpenETWaterMeasurementID { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string WellTPID { get; set; }
        public int OpenETDataTypeID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ReportedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime TransactionDate { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal ReportedValueInches { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal ReportedValueAcreFeet { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal IrrigationUnitArea { get; set; }
    }
}
