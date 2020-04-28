using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;

namespace DroolTool.EFModels.Entities
{
    public partial class vGeoServerWatershed
    {
        public int WatershedID { get; set; }
        [StringLength(50)]
        public string WatershedName { get; set; }
        [Column(TypeName = "geometry")]
        public Geometry WatershedGeometry { get; set; }
    }
}
