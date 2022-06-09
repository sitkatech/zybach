using System.ComponentModel.DataAnnotations;

namespace Zybach.Models.DataTransferObjects
{
    public class SupportTicketCommentUpsertDto
    {
        [Required(ErrorMessage = "Creator user must be specified")]
        public int CreatorUserID { get; set; }
        [Required(ErrorMessage = "Associated support ticket number must be specified")]
        public int SupportTicketID { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        public string CommentNotes{ get; set; }
    }
}
