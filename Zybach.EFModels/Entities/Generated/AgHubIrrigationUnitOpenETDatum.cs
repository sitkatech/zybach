using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("AgHubIrrigationUnitOpenETDatum")]
    public partial class AgHubIrrigationUnitOpenETDatum
    {
        [Key]
        public int AgHubIrrigationUnitOpenETDatumID { get; set; }
        public int AgHubIrrigationUnitID { get; set; }
        public int OpenETDataTypeID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ReportedDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime TransactionDate { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? ReportedValueInches { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? ReportedValueAcreInches { get; set; }

        [ForeignKey("AgHubIrrigationUnitID")]
        [InverseProperty("AgHubIrrigationUnitOpenETData")]
        public virtual AgHubIrrigationUnit AgHubIrrigationUnit { get; set; }
    }
}
