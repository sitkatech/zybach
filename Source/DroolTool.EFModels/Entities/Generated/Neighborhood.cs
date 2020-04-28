using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace DroolTool.EFModels.Entities
{
    public partial class Neighborhood
    {
        public Neighborhood()
        {
            BackboneSegment = new HashSet<BackboneSegment>();
            InverseOCSurveyDownstreamNeighborhood = new HashSet<Neighborhood>();
            RawDroolMetric = new HashSet<RawDroolMetric>();
        }

        [Key]
        public int NeighborhoodID { get; set; }
        [Required]
        [StringLength(10)]
        public string DrainID { get; set; }
        [Required]
        [StringLength(100)]
        public string Watershed { get; set; }
        [Required]
        [Column(TypeName = "geometry")]
        public Geometry NeighborhoodGeometry { get; set; }
        public int OCSurveyNeighborhoodID { get; set; }
        public int? OCSurveyDownstreamNeighborhoodID { get; set; }
        [Column(TypeName = "geometry")]
        public Geometry NeighborhoodGeometry4326 { get; set; }

        public virtual Neighborhood OCSurveyDownstreamNeighborhood { get; set; }
        [InverseProperty("Neighborhood")]
        public virtual ICollection<BackboneSegment> BackboneSegment { get; set; }
        public virtual ICollection<Neighborhood> InverseOCSurveyDownstreamNeighborhood { get; set; }
        public virtual ICollection<RawDroolMetric> RawDroolMetric { get; set; }
    }
}
