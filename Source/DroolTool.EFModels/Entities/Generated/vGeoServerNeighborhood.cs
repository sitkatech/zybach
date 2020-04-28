using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace DroolTool.EFModels.Entities
{
    public partial class vGeoServerNeighborhood
    {
        public int NeighborhoodID { get; set; }
        public int OCSurveyNeighborhoodID { get; set; }
        public int? OCSurveyDownstreamNeighborhoodID { get; set; }
        [Required]
        [StringLength(10)]
        public string DrainID { get; set; }
        [Required]
        [StringLength(100)]
        public string Watershed { get; set; }
        [Column(TypeName = "geometry")]
        public Geometry NeighborhoodGeometry { get; set; }
        public double? Area { get; set; }
    }
}
