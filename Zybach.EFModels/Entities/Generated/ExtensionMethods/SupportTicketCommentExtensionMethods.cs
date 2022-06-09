//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[SupportTicketComment]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class SupportTicketCommentExtensionMethods
    {
        public static SupportTicketCommentDto AsDto(this SupportTicketComment supportTicketComment)
        {
            var supportTicketCommentDto = new SupportTicketCommentDto()
            {
                SupportTicketCommentID = supportTicketComment.SupportTicketCommentID,
                DateCreated = supportTicketComment.DateCreated,
                DateUpdated = supportTicketComment.DateUpdated,
                CreatorUser = supportTicketComment.CreatorUser.AsDto(),
                SupportTicket = supportTicketComment.SupportTicket.AsDto(),
                CommentNotes = supportTicketComment.CommentNotes
            };
            DoCustomMappings(supportTicketComment, supportTicketCommentDto);
            return supportTicketCommentDto;
        }

        static partial void DoCustomMappings(SupportTicketComment supportTicketComment, SupportTicketCommentDto supportTicketCommentDto);

        public static SupportTicketCommentSimpleDto AsSimpleDto(this SupportTicketComment supportTicketComment)
        {
            var supportTicketCommentSimpleDto = new SupportTicketCommentSimpleDto()
            {
                SupportTicketCommentID = supportTicketComment.SupportTicketCommentID,
                DateCreated = supportTicketComment.DateCreated,
                DateUpdated = supportTicketComment.DateUpdated,
                CreatorUserID = supportTicketComment.CreatorUserID,
                SupportTicketID = supportTicketComment.SupportTicketID,
                CommentNotes = supportTicketComment.CommentNotes
            };
            DoCustomSimpleDtoMappings(supportTicketComment, supportTicketCommentSimpleDto);
            return supportTicketCommentSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(SupportTicketComment supportTicketComment, SupportTicketCommentSimpleDto supportTicketCommentSimpleDto);
    }
}