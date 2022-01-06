using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

#nullable disable

namespace Zybach.EFModels.Entities
{
    [Table("Well")]
    public partial class Well
    {
        public Well()
        {
            ChemigationPermits = new HashSet<ChemigationPermit>();
            Sensors = new HashSet<Sensor>();
            WaterLevelInspections = new HashSet<WaterLevelInspection>();
            WaterQualityInspections = new HashSet<WaterQualityInspection>();
            WellWaterQualityInspectionTypes = new HashSet<WellWaterQualityInspectionType>();
        }

        [Key]
        public int WellID { get; set; }
        [Required]
        [StringLength(100)]
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
        public string WellNickname { get; set; }
        [StringLength(100)]
        public string TownshipRangeSection { get; set; }
        public int? CountyID { get; set; }
        public int? WellParticipationID { get; set; }
        public int? WellUseID { get; set; }
        public bool RequiresChemigation { get; set; }
        public bool RequiresWaterLevelInspection { get; set; }
        [Column(TypeName = "decimal(10, 4)")]
        public decimal? WellDepth { get; set; }
        [StringLength(100)]
        public string Clearinghouse { get; set; }
        public int? PageNumber { get; set; }
        [StringLength(100)]
        public string SiteName { get; set; }
        [StringLength(100)]
        public string SiteNumber { get; set; }
        [StringLength(100)]
        public string ScreenInterval { get; set; }
        [Column(TypeName = "decimal(10, 4)")]
        public decimal? ScreenDepth { get; set; }
        [StringLength(100)]
        public string OwnerName { get; set; }
        [StringLength(100)]
        public string OwnerAddress { get; set; }
        [StringLength(100)]
        public string OwnerCity { get; set; }
        [StringLength(20)]
        public string OwnerState { get; set; }
        [StringLength(10)]
        public string OwnerZipCode { get; set; }
        [StringLength(100)]
        public string AdditionalContactName { get; set; }
        [StringLength(100)]
        public string AdditionalContactAddress { get; set; }
        [StringLength(100)]
        public string AdditionalContactCity { get; set; }
        [StringLength(20)]
        public string AdditionalContactState { get; set; }
        [StringLength(10)]
        public string AdditionalContactZipCode { get; set; }
        public bool IsReplacement { get; set; }
        [StringLength(1000)]
        public string Notes { get; set; }

        [ForeignKey(nameof(CountyID))]
        [InverseProperty("Wells")]
        public virtual County County { get; set; }
        [ForeignKey(nameof(StreamflowZoneID))]
        [InverseProperty(nameof(StreamFlowZone.Wells))]
        public virtual StreamFlowZone StreamflowZone { get; set; }
        [ForeignKey(nameof(WellParticipationID))]
        [InverseProperty("Wells")]
        public virtual WellParticipation WellParticipation { get; set; }
        [ForeignKey(nameof(WellUseID))]
        [InverseProperty("Wells")]
        public virtual WellUse WellUse { get; set; }
        [InverseProperty("Well")]
        public virtual AgHubWell AgHubWell { get; set; }
        [InverseProperty("Well")]
        public virtual GeoOptixWell GeoOptixWell { get; set; }
        [InverseProperty(nameof(ChemigationPermit.Well))]
        public virtual ICollection<ChemigationPermit> ChemigationPermits { get; set; }
        [InverseProperty(nameof(Sensor.Well))]
        public virtual ICollection<Sensor> Sensors { get; set; }
        [InverseProperty(nameof(WaterLevelInspection.Well))]
        public virtual ICollection<WaterLevelInspection> WaterLevelInspections { get; set; }
        [InverseProperty(nameof(WaterQualityInspection.Well))]
        public virtual ICollection<WaterQualityInspection> WaterQualityInspections { get; set; }
        [InverseProperty(nameof(WellWaterQualityInspectionType.Well))]
        public virtual ICollection<WellWaterQualityInspectionType> WellWaterQualityInspectionTypes { get; set; }
    }
}
