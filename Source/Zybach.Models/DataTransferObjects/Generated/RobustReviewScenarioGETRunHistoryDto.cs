//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[RobustReviewScenarioGETRunHistory]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class RobustReviewScenarioGETRunHistoryDto
    {
        public int RobustReviewScenarioGETRunHistoryID { get; set; }
        public UserDto CreateByUser { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int? GETRunID { get; set; }
        public DateTime? SuccessfulStartDate { get; set; }
        public bool IsTerminal { get; set; }
        public string StatusMessage { get; set; }
        public string StatusHexColor { get; set; }
    }
}