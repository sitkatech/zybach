using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Zybach.EFModels.Entities;

[Table("SupportTicketNotification")]
public partial class SupportTicketNotification
{
    [Key]
    public int SupportTicketNotificationID { get; set; }

    public int SupportTicketID { get; set; }

    [Required]
    [StringLength(200)]
    [Unicode(false)]
    public string EmailAddresses { get; set; }

    [Required]
    [StringLength(200)]
    [Unicode(false)]
    public string EmailSubject { get; set; }

    [Required]
    [Unicode(false)]
    public string EmailBody { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime SentDate { get; set; }

    [ForeignKey("SupportTicketID")]
    [InverseProperty("SupportTicketNotifications")]
    public virtual SupportTicket SupportTicket { get; set; }
}
