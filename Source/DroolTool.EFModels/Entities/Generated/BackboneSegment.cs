using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace DroolTool.EFModels.Entities
{
    public partial class BackboneSegment
    {
        public BackboneSegment()
        {
            InverseDownstreamBackboneSegment = new HashSet<BackboneSegment>();
        }

        [Key]
        public int BackboneSegmentID { get; set; }
        [Required]
        [Column(TypeName = "geometry")]
        public Geometry BackboneSegmentGeometry { get; set; }
        public int CatchIDN { get; set; }
        public int BackboneSegmentTypeID { get; set; }
        public int? DownstreamBackboneSegmentID { get; set; }
        public string StreamName { get; set; }
        [Column(TypeName = "geometry")]
        public Geometry BackboneSegmentGeometry4326 { get; set; }
        public int? NeighborhoodID { get; set; }

        [ForeignKey(nameof(BackboneSegmentTypeID))]
        [InverseProperty("BackboneSegment")]
        public virtual BackboneSegmentType BackboneSegmentType { get; set; }
        [ForeignKey(nameof(DownstreamBackboneSegmentID))]
        [InverseProperty(nameof(BackboneSegment.InverseDownstreamBackboneSegment))]
        public virtual BackboneSegment DownstreamBackboneSegment { get; set; }
        [ForeignKey(nameof(NeighborhoodID))]
        [InverseProperty("BackboneSegment")]
        public virtual Neighborhood Neighborhood { get; set; }
        [InverseProperty(nameof(BackboneSegment.DownstreamBackboneSegment))]
        public virtual ICollection<BackboneSegment> InverseDownstreamBackboneSegment { get; set; }
    }
}
