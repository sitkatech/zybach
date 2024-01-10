CREATE TABLE [dbo].[RobustReviewScenarioGETRunHistory](
	[RobustReviewScenarioGETRunHistoryID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_RobustReviewScenarioGETRunHistory_RobustReviewScenarioGETRunHistoryID] PRIMARY KEY,
	[CreateByUserID] [int] NOT NULL CONSTRAINT [FK_RobustReviewScenarioGETRunHistory_User_CreateByUserID_UserID] FOREIGN KEY REFERENCES [dbo].[User] ([UserID]),
	[CreateDate] [datetime] NOT NULL,
	[LastUpdateDate] [datetime] NULL,
	[GETRunID] [int] NULL,
	[SuccessfulStartDate] [datetime] NULL,
	[IsTerminal] [bit] NOT NULL,
	[StatusMessage] [varchar](100) NULL,
	[StatusHexColor] [varchar](7) NULL,
)