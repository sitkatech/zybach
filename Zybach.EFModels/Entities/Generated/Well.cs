using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Zybach.EFModels.Entities;

[Table("Well")]
[Index("WellRegistrationID", Name = "AK_Well_WellRegistrationID", IsUnique = true)]
public partial class Well
{
    [Key]
    public int WellID { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string WellRegistrationID { get; set; }

    [Required]
    [Column(TypeName = "geometry")]
    public Geometry WellGeometry { get; set; }

    public int? StreamflowZoneID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastUpdateDate { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string WellNickname { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string TownshipRangeSection { get; set; }

    public int? CountyID { get; set; }

    public int? WellParticipationID { get; set; }

    public int? WellUseID { get; set; }

    public bool RequiresChemigation { get; set; }

    public bool RequiresWaterLevelInspection { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal? WellDepth { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Clearinghouse { get; set; }

    public int? PageNumber { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string SiteName { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string SiteNumber { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string ScreenInterval { get; set; }

    [Column(TypeName = "decimal(10, 4)")]
    public decimal? ScreenDepth { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string OwnerName { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string OwnerAddress { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string OwnerCity { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string OwnerState { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string OwnerZipCode { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string AdditionalContactName { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string AdditionalContactAddress { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string AdditionalContactCity { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string AdditionalContactState { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string AdditionalContactZipCode { get; set; }

    public bool IsReplacement { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string Notes { get; set; }

    [InverseProperty("Well")]
    public virtual AgHubWell AgHubWell { get; set; }

    [InverseProperty("Well")]
    public virtual ICollection<ChemigationPermit> ChemigationPermits { get; set; } = new List<ChemigationPermit>();

    [InverseProperty("Well")]
    public virtual GeoOptixWell GeoOptixWell { get; set; }

    [InverseProperty("Well")]
    public virtual ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();

    [ForeignKey("StreamflowZoneID")]
    [InverseProperty("Wells")]
    public virtual StreamFlowZone StreamflowZone { get; set; }

    [InverseProperty("Well")]
    public virtual ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();

    [InverseProperty("Well")]
    public virtual ICollection<WaterLevelInspection> WaterLevelInspections { get; set; } = new List<WaterLevelInspection>();

    [InverseProperty("Well")]
    public virtual ICollection<WaterQualityInspection> WaterQualityInspections { get; set; } = new List<WaterQualityInspection>();

    [InverseProperty("Well")]
    public virtual ICollection<WellGroupWell> WellGroupWells { get; set; } = new List<WellGroupWell>();

    [ForeignKey("WellParticipationID")]
    [InverseProperty("Wells")]
    public virtual WellParticipation WellParticipation { get; set; }

    [InverseProperty("Well")]
    public virtual ICollection<WellWaterQualityInspectionType> WellWaterQualityInspectionTypes { get; set; } = new List<WellWaterQualityInspectionType>();
}
