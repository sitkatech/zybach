using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("SupportTicketComment")]
public partial class SupportTicketComment
{
    [Key]
    public int SupportTicketCommentID { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateCreated { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateUpdated { get; set; }

    public int CreatorUserID { get; set; }

    public int SupportTicketID { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string CommentNotes { get; set; }

    [ForeignKey("CreatorUserID")]
    [InverseProperty("SupportTicketComments")]
    public virtual User CreatorUser { get; set; }

    [ForeignKey("SupportTicketID")]
    [InverseProperty("SupportTicketComments")]
    public virtual SupportTicket SupportTicket { get; set; }
}
