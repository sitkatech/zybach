//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[RobustReviewScenarioGETRunHistory]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class RobustReviewScenarioGETRunHistoryExtensionMethods
    {
        public static RobustReviewScenarioGETRunHistoryDto AsDto(this RobustReviewScenarioGETRunHistory robustReviewScenarioGETRunHistory)
        {
            var robustReviewScenarioGETRunHistoryDto = new RobustReviewScenarioGETRunHistoryDto()
            {
                RobustReviewScenarioGETRunHistoryID = robustReviewScenarioGETRunHistory.RobustReviewScenarioGETRunHistoryID,
                CreateByUser = robustReviewScenarioGETRunHistory.CreateByUser.AsDto(),
                CreateDate = robustReviewScenarioGETRunHistory.CreateDate,
                LastUpdateDate = robustReviewScenarioGETRunHistory.LastUpdateDate,
                GETRunID = robustReviewScenarioGETRunHistory.GETRunID,
                SuccessfulStartDate = robustReviewScenarioGETRunHistory.SuccessfulStartDate,
                IsTerminal = robustReviewScenarioGETRunHistory.IsTerminal,
                StatusMessage = robustReviewScenarioGETRunHistory.StatusMessage,
                StatusHexColor = robustReviewScenarioGETRunHistory.StatusHexColor
            };
            DoCustomMappings(robustReviewScenarioGETRunHistory, robustReviewScenarioGETRunHistoryDto);
            return robustReviewScenarioGETRunHistoryDto;
        }

        static partial void DoCustomMappings(RobustReviewScenarioGETRunHistory robustReviewScenarioGETRunHistory, RobustReviewScenarioGETRunHistoryDto robustReviewScenarioGETRunHistoryDto);

    }
}