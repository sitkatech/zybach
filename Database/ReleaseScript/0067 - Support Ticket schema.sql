create table dbo.SupportTicketStatus (
	SupportTicketStatusID int not null constraint PK_SupportTicketStatus_SupportTicketStatusID primary key,
	SupportTicketStatusName varchar(50) not null,
	SupportTicketStatusDisplayName varchar(50) not null,
	SortOrder int not null
)

insert into dbo.SupportTicketStatus (SupportTicketStatusID, SupportTicketStatusName, SupportTicketStatusDisplayName, SortOrder) 
values
(1, 'Open', 'Open', 10),
(2, 'InProgress', 'In Progress', 20),
(3, 'Resolved', 'Resolved', 30)

create table dbo.SupportTicketPriority (
	SupportTicketPriorityID int not null constraint PK_SupportTicketPriority_SupportTicketPriorityID primary key,
	SupportTicketPriorityName varchar(50) not null,
	SupportTicketPriorityDisplayName varchar(50) not null,
	SortOrder int not null
)

insert into dbo.SupportTicketPriority (SupportTicketPriorityID, SupportTicketPriorityName, SupportTicketPriorityDisplayName, SortOrder) 
values
(1, 'High', 'High', 10),
(2, 'Medium', 'Medium', 20),
(3, 'Low', 'Low', 30)

go

create table dbo.SupportTicket (
	SupportTicketID int not null identity(1,1) constraint PK_SupportTicket_SupportTicketID primary key,
	DateCreated datetime not null,
	DateUpdated datetime not null,
	CreatorUserID int not null constraint FK_SupportTicket_User_CreatorUserID_UserID foreign key (CreatorUserID) references dbo.[User](UserID),
	AssigneeUserID int null constraint FK_SupportTicket_User_AssigneeUserID_UserID foreign key (AssigneeUserID) references dbo.[User](UserID),
	WellID int not null constraint FK_SupportTicket_Well_WellID foreign key (WellID) references dbo.Well(WellID),
	SensorID int null constraint FK_SupportTicket_Sensor_SensorID foreign key (SensorID) references dbo.Sensor(SensorID),
	SupportTicketStatusID int not null constraint FK_SupportTicket_SupportTicketStatus_SupportTicketStatusID foreign key (SupportTicketStatusID) references dbo.SupportTicketStatus(SupportTicketStatusID),
	SupportTicketPriorityID int not null constraint FK_SupportTicket_SupportTicketPriority_SupportTicketPriorityID foreign key (SupportTicketPriorityID) references dbo.SupportTicketPriority(SupportTicketPriorityID),
	SupportTicketTitle varchar(100) not null,
	SupportTicketDescription varchar(200) null
)

create table dbo.SupportTicketComment (
	SupportTicketCommentID int not null identity(1,1) constraint PK_SupportTicketComment_SupportTicketCommentID primary key,
	DateCreated datetime not null,
	DateUpdated datetime not null,
	CreatorUserID int not null constraint FK_SupportTicketComment_User_CreatorUserID_UserID foreign key (CreatorUserID) references dbo.[User](UserID),
	SupportTicketID int not null constraint FK_SupportTicketComment_SupportTicket_SupportTicketID foreign key (SupportTicketID) references dbo.SupportTicket(SupportTicketID),
	CommentNotes varchar(500) null
)