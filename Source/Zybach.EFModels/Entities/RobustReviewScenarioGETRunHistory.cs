using System;
using System.Collections.Generic;
using System.Linq;
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
            return _dbContext.RobustReviewScenarioGETRunHistories.Select(x => x.AsDto()).ToList();
        }

        public static RobustReviewScenarioGETRunHistory GetNotYetStartedRobustReviewScenarioGetRunHistory(ZybachDbContext _dbContext)
        {
            return _dbContext.RobustReviewScenarioGETRunHistories.SingleOrDefault(x =>
                x.IsTerminal == false && x.GETRunID == null);
        }
    }
}