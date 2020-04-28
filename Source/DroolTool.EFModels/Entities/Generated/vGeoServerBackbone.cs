using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace DroolTool.EFModels.Entities
{
    public partial class vGeoServerBackbone
    {
        public int BackboneSegmentID { get; set; }
        public int CatchIDN { get; set; }
        public int? NeighborhoodID { get; set; }
        public int BackboneSegmentTypeID { get; set; }
        public int? DownstreamBackboneSegmentID { get; set; }
        public string StreamName { get; set; }
        [Column(TypeName = "geometry")]
        public Geometry BackboneSegmentGeometry { get; set; }
        [Required]
        [StringLength(20)]
        public string BackboneSegmentType { get; set; }
    }
}
