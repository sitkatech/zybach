using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class RobustReviewScenarioGETRunHistory
    {
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
    }
}