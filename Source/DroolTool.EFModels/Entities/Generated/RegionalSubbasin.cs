using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace DroolTool.EFModels.Entities
{
    public partial class RegionalSubbasin
    {
        public RegionalSubbasin()
        {
            InverseOCSurveyDownstreamCatchment = new HashSet<RegionalSubbasin>();
        }

        [Key]
        public int RegionalSubbasinID { get; set; }
        [StringLength(10)]
        public string DrainID { get; set; }
        [StringLength(100)]
        public string Watershed { get; set; }
        [Required]
        [Column(TypeName = "geometry")]
        public Geometry CatchmentGeometry { get; set; }
        public int OCSurveyCatchmentID { get; set; }
        public int? OCSurveyDownstreamCatchmentID { get; set; }
        [Column(TypeName = "geometry")]
        public Geometry CatchmentGeometry4326 { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? LastUpdate { get; set; }

        public virtual RegionalSubbasin OCSurveyDownstreamCatchment { get; set; }
        public virtual ICollection<RegionalSubbasin> InverseOCSurveyDownstreamCatchment { get; set; }
    }
}
