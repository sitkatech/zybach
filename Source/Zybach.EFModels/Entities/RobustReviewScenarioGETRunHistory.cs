using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class RobustReviewScenarioGETRunHistory
    {
        private static readonly string errorTerminalStatusMessage = "GET Integration Failure";

        public static void CreateNewRobustReviewScenarioGETRunHistory(ZybachDbContext _dbContext,
            int userID)
        {
            var robustReviewScenarioGETRunHistory = new RobustReviewScenarioGETRunHistory()
            {
                CreateByUserID = userID,
                CreateDate = DateTime.Now,
                StatusMessage = "Waiting for GET API to start run"
            };

            _dbContext.RobustReviewScenarioGETRunHistories.Add(robustReviewScenarioGETRunHistory);
            _dbContext.SaveChanges();
        }

        public static List<RobustReviewScenarioGETRunHistoryDto> List(ZybachDbContext _dbContext)
        {
            return _dbContext.RobustReviewScenarioGETRunHistories.Include(x => x.CreateByUser).ThenInclude(x => x.Role).OrderByDescending(x => x.CreateDate).Select(x => x.AsDto()).ToList();
        }

        public static RobustReviewScenarioGETRunHistory GetNotYetStartedRobustReviewScenarioGETRunHistory(ZybachDbContext _dbContext)
        {
            return _dbContext.RobustReviewScenarioGETRunHistories.SingleOrDefault(x =>
                x.IsTerminal == false && x.GETRunID == null);
        }

        public static RobustReviewScenarioGETRunHistory GetNonTerminalSuccessfullyStartedRobustReviewScenarioGETRunHistory(ZybachDbContext _dbContext)
        {
            return _dbContext.RobustReviewScenarioGETRunHistories.SingleOrDefault(x =>
                x.IsTerminal == false && x.GETRunID != null && x.SuccessfulStartDate != null);
        }

        public static void MarkRobustReviewScenarioGETRunHistoryAsTerminalWithIntegrationFailure(
            ZybachDbContext _dbContext, RobustReviewScenarioGETRunHistory historyEntry)
        {
            historyEntry.LastUpdateDate = DateTime.Now;
            historyEntry.IsTerminal = true;
            historyEntry.StatusMessage = errorTerminalStatusMessage;
            _dbContext.SaveChanges();
        }

        public static Boolean NonTerminalRunsExist(ZybachDbContext _dbContext)
        {
            return GetNotYetStartedRobustReviewScenarioGETRunHistory(_dbContext) != null ||
                   GetNonTerminalSuccessfullyStartedRobustReviewScenarioGETRunHistory(_dbContext) != null;
        }
    }
}