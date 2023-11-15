using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("User")]
    [Index("Email", Name = "AK_User_Email", IsUnique = true)]
    public partial class User
    {
        public User()
        {
            ChemigationInspections = new HashSet<ChemigationInspection>();
            FileResources = new HashSet<FileResource>();
            RobustReviewScenarioGETRunHistories = new HashSet<RobustReviewScenarioGETRunHistory>();
            SupportTicketAssigneeUsers = new HashSet<SupportTicket>();
            SupportTicketComments = new HashSet<SupportTicketComment>();
            SupportTicketCreatorUsers = new HashSet<SupportTicket>();
            WaterLevelInspections = new HashSet<WaterLevelInspection>();
            WaterQualityInspections = new HashSet<WaterQualityInspection>();
        }

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

        [InverseProperty("InspectorUser")]
        public virtual ICollection<ChemigationInspection> ChemigationInspections { get; set; }
        [InverseProperty("CreateUser")]
        public virtual ICollection<FileResource> FileResources { get; set; }
        [InverseProperty("CreateByUser")]
        public virtual ICollection<RobustReviewScenarioGETRunHistory> RobustReviewScenarioGETRunHistories { get; set; }
        [InverseProperty("AssigneeUser")]
        public virtual ICollection<SupportTicket> SupportTicketAssigneeUsers { get; set; }
        [InverseProperty("CreatorUser")]
        public virtual ICollection<SupportTicketComment> SupportTicketComments { get; set; }
        [InverseProperty("CreatorUser")]
        public virtual ICollection<SupportTicket> SupportTicketCreatorUsers { get; set; }
        [InverseProperty("InspectorUser")]
        public virtual ICollection<WaterLevelInspection> WaterLevelInspections { get; set; }
        [InverseProperty("InspectorUser")]
        public virtual ICollection<WaterQualityInspection> WaterQualityInspections { get; set; }
    }
}
