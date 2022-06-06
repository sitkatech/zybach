using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities
{
    [Table("SupportTicket")]
    public partial class SupportTicket
    {
        public SupportTicket()
        {
            SupportTicketComments = new HashSet<SupportTicketComment>();
        }

        [Key]
        public int SupportTicketID { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateUpdated { get; set; }
        public int CreatorUserID { get; set; }
        public int? AssigneeUserID { get; set; }
        public int WellID { get; set; }
        public int? SensorID { get; set; }
        public int SupportTicketStatusID { get; set; }
        public int SupportTicketPriorityID { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string SupportTicketTitle { get; set; }
        [StringLength(200)]
        [Unicode(false)]
        public string SupportTicketDescription { get; set; }

        [ForeignKey("AssigneeUserID")]
        [InverseProperty("SupportTicketAssigneeUsers")]
        public virtual User AssigneeUser { get; set; }
        [ForeignKey("CreatorUserID")]
        [InverseProperty("SupportTicketCreatorUsers")]
        public virtual User CreatorUser { get; set; }
        [ForeignKey("SensorID")]
        [InverseProperty("SupportTickets")]
        public virtual Sensor Sensor { get; set; }
        [ForeignKey("WellID")]
        [InverseProperty("SupportTickets")]
        public virtual Well Well { get; set; }
        [InverseProperty("SupportTicket")]
        public virtual ICollection<SupportTicketComment> SupportTicketComments { get; set; }
    }
}
