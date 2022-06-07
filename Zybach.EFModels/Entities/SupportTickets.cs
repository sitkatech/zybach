using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public class SupportTickets
    {
        public static IQueryable<SupportTicket> GetSupportTicketsImpl(ZybachDbContext dbContext)
        {
            return dbContext.SupportTickets
                .Include(x => x.Well)
                .Include(x => x.Sensor)
                .Include(x => x.AssigneeUser)
                .Include(x => x.CreatorUser)
                .Include(x => x.SupportTicketComments)
                    .ThenInclude(x => x.CreatorUser)
                .AsNoTracking();
        }

        public static List<SupportTicketSimpleDto> ListAsSimpleDto(ZybachDbContext dbContext)
        {
            return GetSupportTicketsImpl(dbContext)
                .OrderByDescending(x => x.DateUpdated)
                .Select(x => x.AsSimpleDto())
                .ToList();
        }

        public static SupportTicket GetByID(ZybachDbContext dbContext, int supportTicketID)
        {
            return GetSupportTicketsImpl(dbContext).SingleOrDefault(x => x.SupportTicketID == supportTicketID);
        }

        public static SupportTicketDetailDto CreateNewSupportTicket(ZybachDbContext dbContext, SupportTicketUpsertDto supportTicketUpsertDto)
        {
            var supportTicket = new SupportTicket()
            {
                DateCreated = DateTime.Now.Date,
                DateUpdated = DateTime.Now.Date,
                CreatorUserID = supportTicketUpsertDto.CreatorUserID,
                AssigneeUserID = supportTicketUpsertDto.AssigneeUserID,
                // ReSharper disable once PossibleInvalidOperationException - null check performed in controller
                WellID = supportTicketUpsertDto.WellID.Value,
                SensorID = supportTicketUpsertDto.SensorID,
                SupportTicketStatusID = supportTicketUpsertDto.SupportTicketStatusID,
                SupportTicketPriorityID = supportTicketUpsertDto.SupportTicketPriorityID,
                SupportTicketTitle = supportTicketUpsertDto.SupportTicketTitle,
                SupportTicketDescription = supportTicketUpsertDto.SupportTicketDescription
            };

            dbContext.SupportTickets.Add(supportTicket);
            dbContext.SaveChanges();
            dbContext.Entry(supportTicket).Reload();

            return GetByID(dbContext, supportTicket.SupportTicketID).AsDetailDto();
        }

        public static SupportTicketDetailDto UpdateSupportTicket(ZybachDbContext dbContext, SupportTicket supportTicket, SupportTicketUpsertDto supportTicketUpsertDto)
        {
            supportTicket.DateUpdated = DateTime.Now.Date;
            supportTicket.CreatorUserID = supportTicketUpsertDto.CreatorUserID;
            supportTicket.AssigneeUserID = supportTicketUpsertDto.AssigneeUserID;
            // ReSharper disable once PossibleInvalidOperationException - null check performed in controller
            supportTicket.WellID = supportTicketUpsertDto.WellID.Value;
            supportTicket.SensorID = supportTicketUpsertDto.SensorID;
            supportTicket.SupportTicketStatusID = supportTicketUpsertDto.SupportTicketStatusID;
            supportTicket.SupportTicketPriorityID = supportTicketUpsertDto.SupportTicketPriorityID;
            supportTicket.SupportTicketTitle = supportTicketUpsertDto.SupportTicketTitle;
            supportTicket.SupportTicketDescription = supportTicketUpsertDto.SupportTicketDescription;
            dbContext.SaveChanges();
            dbContext.Entry(supportTicket).Reload();
            return GetByID(dbContext, supportTicket.SupportTicketID).AsDetailDto();
        }

        public static SupportTicket GetByIDWithTracking(ZybachDbContext dbContext, int supportTicketID)
        {
            return dbContext.SupportTickets
                .Include(x => x.Well)
                .Include(x => x.Sensor)
                .Include(x => x.AssigneeUser)
                .Include(x => x.CreatorUser)
                .Include(x => x.SupportTicketComments)
                .SingleOrDefault(x => x.SupportTicketID == supportTicketID);
        }
    }
}
