using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public class SupportTicketDetailDto : SupportTicketSimpleDto
    {
        public List<SupportTicketCommentSimpleDto> Comments { get; set; }
    }
}
