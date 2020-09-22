using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zybach.EFModels.Entities
{
    public partial class GeoOptixAccessToken
    {
        [Key]
        public int GeoOptixAccessTokenID { get; set; }
        [Required]
        [StringLength(2048)]
        public string GeoOptixAccessTokenValue { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime GeoOptixAccessTokenExpiryDate { get; set; }
    }
}
