create table dbo.RobustReviewScenarioGETRunHistory (
	RobustReviewScenarioGETRunHistoryID int not null identity(1,1) constraint PK_RobustReviewScenarioGETRunHistory_RobustReviewScenarioGETRunHistoryID primary key,
	CreateByUserID int not null constraint FK_RobustReviewScenarioGETRunHistory_User_CreateByUserID_UserID foreign key references dbo.[User] (UserID),
	CreateDate datetime not null,
	LastUpdateDate datetime null,
	GETRunID int null,
	SuccessfulStartDate datetime null,
	IsTerminal bit not null,
	StatusMessage varchar(100) null,
	StatusHexColor varchar(7) null
)