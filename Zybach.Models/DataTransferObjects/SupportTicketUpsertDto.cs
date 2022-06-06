using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class SupportTicketUpsertDto
    {
        [Required]
        public string WellRegistrationID { get; set; }
        public int? WellID { get; set; }
        public int? SensorID { get; set; }
        [Required]
        public int CreatorUserID { get; set; }
        public int? AssigneeUserID { get; set; }
        [Required] 
        public int SupportTicketPriorityID { get; set; }
        [Required]
        public int SupportTicketStatusID { get; set; }
        [Required]
        public string SupportTicketTitle { get; set; }
        public string SupportTicketDescription { get; set; }

    }
}
