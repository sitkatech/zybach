using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Zybach.EFModels.Entities;

[Keyless]
public partial class vGeoServerAgHubIrrigationUnit
{
    public int AgHubIrrigationUnitID { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string WellTPID { get; set; }

    [Required]
    [Column(TypeName = "geometry")]
    public Geometry IrrigationUnitGeometry { get; set; }

    public int IrrigationYear { get; set; }

    public double Acres { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string CropType { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string CropTypeLegendDisplayName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string CropTypeMapColor { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Tillage { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string TillageTypeLegendDisplayName { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string TillageTypeMapColor { get; set; }
}
