using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public class SupportTicketComments
    {
        public static SupportTicketCommentSimpleDto CreateNewSupportTicketComment(ZybachDbContext dbContext, SupportTicketCommentUpsertDto supportTicketCommentUpsertDto)
        {
            var supportTicketComment = new SupportTicketComment()
            {
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                CreatorUserID = supportTicketCommentUpsertDto.CreatorUserID,
                CommentNotes = supportTicketCommentUpsertDto.CommentNotes,
                SupportTicketID = supportTicketCommentUpsertDto.SupportTicketID
            };

            var supportTicket = dbContext.SupportTickets.Single(x => x.SupportTicketID == supportTicketCommentUpsertDto.SupportTicketID);
            supportTicket.DateUpdated = DateTime.UtcNow;

            dbContext.SupportTicketComments.Add(supportTicketComment);
            dbContext.SaveChanges();

            
            return GetByID(dbContext, supportTicketComment.SupportTicketCommentID)?.AsSimpleDto();
        }

        public static SupportTicketComment GetByID(ZybachDbContext dbContext, int supportTicketCommentID)
        {
            return dbContext.SupportTicketComments
                .AsNoTracking()
                .Include(x => x.CreatorUser)
                .SingleOrDefault(x => x.SupportTicketCommentID == supportTicketCommentID);
        }

        public static SupportTicketComment GetByIDWithTracking(ZybachDbContext dbContext, int supportTicketCommentID)
        {
            return dbContext.SupportTicketComments
                .Include(x => x.CreatorUser)
                .SingleOrDefault(x => x.SupportTicketCommentID == supportTicketCommentID);
        }
    }
}
