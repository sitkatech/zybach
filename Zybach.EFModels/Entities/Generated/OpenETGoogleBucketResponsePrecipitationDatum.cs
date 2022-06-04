using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("OpenETGoogleBucketResponsePrecipitationDatum")]
    public partial class OpenETGoogleBucketResponsePrecipitationDatum
    {
        [Key]
        public int OpenETGoogleBucketResponsePrecipitationDatumID { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string WellTPID { get; set; }
        public int WaterMonth { get; set; }
        public int WaterYear { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? PrecipitationAcreFeet { get; set; }
        [Column(TypeName = "decimal(20, 4)")]
        public decimal? PrecipitationInches { get; set; }
    }
}
