SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RobustReviewScenarioGETRunHistory](
	[RobustReviewScenarioGETRunHistoryID] [int] IDENTITY(1,1) NOT NULL,
	[CreateByUserID] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastUpdateDate] [datetime] NULL,
	[GETRunID] [int] NULL,
	[SuccessfulStartDate] [datetime] NULL,
	[IsTerminal] [bit] NOT NULL,
	[StatusMessage] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[StatusHexColor] [varchar](7) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_RobustReviewScenarioGETRunHistory_RobustReviewScenarioGETRunHistoryID] PRIMARY KEY CLUSTERED 
(
	[RobustReviewScenarioGETRunHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[RobustReviewScenarioGETRunHistory]  WITH CHECK ADD  CONSTRAINT [FK_RobustReviewScenarioGETRunHistory_User_CreateByUserID_UserID] FOREIGN KEY([CreateByUserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[RobustReviewScenarioGETRunHistory] CHECK CONSTRAINT [FK_RobustReviewScenarioGETRunHistory_User_CreateByUserID_UserID]