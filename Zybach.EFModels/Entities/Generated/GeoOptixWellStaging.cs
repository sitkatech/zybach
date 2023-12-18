using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Zybach.EFModels.Entities;

[Table("GeoOptixWellStaging")]
public partial class GeoOptixWellStaging
{
    [Key]
    public int GeoOptixWellStagingID { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string WellRegistrationID { get; set; }

    [Required]
    [Column(TypeName = "geometry")]
    public Geometry WellGeometry { get; set; }
}
