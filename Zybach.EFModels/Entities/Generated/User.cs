using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("User")]
[Index("Email", Name = "AK_User_Email", IsUnique = true)]
public partial class User
{
    [Key]
    public int UserID { get; set; }

    public Guid? UserGuid { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    [Unicode(false)]
    public string LastName { get; set; }

    [Required]
    [StringLength(255)]
    [Unicode(false)]
    public string Email { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Phone { get; set; }

    public int RoleID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdateDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastActivityDate { get; set; }

    public bool IsActive { get; set; }

    public bool ReceiveSupportEmails { get; set; }

    [StringLength(128)]
    [Unicode(false)]
    public string LoginName { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Company { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? DisclaimerAcknowledgedDate { get; set; }

    public bool PerformsChemigationInspections { get; set; }

    [InverseProperty("CreateUser")]
    public virtual ICollection<BlobResource> BlobResources { get; set; } = new List<BlobResource>();

    [InverseProperty("InspectorUser")]
    public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; } = new List<ChemigationInspection>();

    [InverseProperty("CreateUser")]
    public virtual ICollection<FileResource> FileResources { get; set; } = new List<FileResource>();

    [InverseProperty("FinalizeByUser")]
    public virtual ICollection<PrismMonthlySync> PrismMonthlySyncFinalizeByUsers { get; set; } = new List<PrismMonthlySync>();

    [InverseProperty("LastSynchronizedByUser")]
    public virtual ICollection<PrismMonthlySync> PrismMonthlySyncLastSynchronizedByUsers { get; set; } = new List<PrismMonthlySync>();

    [InverseProperty("CreateByUser")]
    public virtual ICollection<RobustReviewScenarioGETRunHistory> RobustReviewScenarioGETRunHistories { get; set; } = new List<RobustReviewScenarioGETRunHistory>();

    [InverseProperty("AssigneeUser")]
    public virtual ICollection<SupportTicket> SupportTicketAssigneeUsers { get; set; } = new List<SupportTicket>();

    [InverseProperty("CreatorUser")]
    public virtual ICollection<SupportTicketComment> SupportTicketComments { get; set; } = new List<SupportTicketComment>();

    [InverseProperty("CreatorUser")]
    public virtual ICollection<SupportTicket> SupportTicketCreatorUsers { get; set; } = new List<SupportTicket>();

    [InverseProperty("InspectorUser")]
    public virtual ICollection<WaterLevelInspection> WaterLevelInspections { get; set; } = new List<WaterLevelInspection>();

    [InverseProperty("InspectorUser")]
    public virtual ICollection<WaterQualityInspection> WaterQualityInspections { get; set; } = new List<WaterQualityInspection>();
}
