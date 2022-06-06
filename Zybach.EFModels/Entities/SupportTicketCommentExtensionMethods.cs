using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class SupportTicketCommentExtensionMethods
    {
        static partial void DoCustomSimpleDtoMappings(SupportTicketComment supportTicketComment, SupportTicketCommentSimpleDto supportTicketCommentSimpleDto)
        {
            supportTicketCommentSimpleDto.CreatorUser = supportTicketComment.CreatorUser.AsSimpleDto();
        }
    }
}
