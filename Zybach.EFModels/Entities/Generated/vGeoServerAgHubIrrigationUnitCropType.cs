using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Zybach.EFModels.Entities;

[Keyless]
public partial class vGeoServerAgHubIrrigationUnitCropType
{
    public int AgHubIrrigationUnitID { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string WellTPID { get; set; }

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
    public string Tillage { get; set; }
}