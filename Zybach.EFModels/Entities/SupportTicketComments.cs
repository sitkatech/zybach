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
                DateCreated = DateTime.Now.Date,
                DateUpdated = DateTime.Now.Date,
                CreatorUserID = supportTicketCommentUpsertDto.CreatorUserID,
                CommentNotes = supportTicketCommentUpsertDto.CommentNotes,
                SupportTicketID = supportTicketCommentUpsertDto.SupportTicketID
            };

            var supportTicket = SupportTickets.GetByID(dbContext, supportTicketCommentUpsertDto.SupportTicketID);
            supportTicket.DateUpdated = DateTime.Now.Date;

            dbContext.SupportTicketComments.Add(supportTicketComment);
            dbContext.SaveChanges();
            dbContext.Entry(supportTicketComment).Reload();

            return dbContext.SupportTicketComments
                .AsNoTracking()
                .Include(x => x.CreatorUser)
                .SingleOrDefault(x => x.SupportTicketCommentID == supportTicketComment.SupportTicketCommentID)
                .AsSimpleDto();
        }

        public static SupportTicketComment GetByIDWithTracking(ZybachDbContext dbContext, int supportTicketCommentID)
        {
            return dbContext.SupportTicketComments
                .Include(x => x.CreatorUser)
                .SingleOrDefault(x => x.SupportTicketCommentID == supportTicketCommentID);
        }
    }
}
