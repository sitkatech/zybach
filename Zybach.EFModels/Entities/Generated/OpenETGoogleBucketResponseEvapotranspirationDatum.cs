using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("OpenETGoogleBucketResponseEvapotranspirationDatum")]
    public partial class OpenETGoogleBucketResponseEvapotranspirationDatum
    {
        [Key]
        public int OpenETGoogleBucketResponseEvapotranspirationDatumID { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string WellTPID { get; set; }
        public int WaterMonth { get; set; }
        public int WaterYear { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? EvapotranspirationInches { get; set; }
    }
}
