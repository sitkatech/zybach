using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class SupportTicketUpsertDto
    {
        [Required]
        public string WellRegistrationID { get; set; }
        public int? WellID { get; set; }
        public string SensorName { get; set; }
        public int? SensorID { get; set; }
        [Required(ErrorMessage = "Creator user must be specified")]
        public int CreatorUserID { get; set; }
        public int? AssigneeUserID { get; set; }
        [Required(ErrorMessage = "Priority level is required")] 
        public int? SupportTicketPriorityID { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public int? SupportTicketStatusID { get; set; }
        [Display(Name = "Ticket Name")]
        [Required]
        [StringLength(100, ErrorMessage = "Ticket name cannot exceed 100 characters.")]
        public string SupportTicketTitle { get; set; }
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string SupportTicketDescription { get; set; }

    }
}
