create table dbo.SupportTicketNotification (
	SupportTicketNotificationID int not null identity(1,1) constraint PK_SupportTicketNotification_SupportTicketNotificationID primary key,
	SupportTicketID int not null constraint FK_SupportTicketNotification_SupportTicket_SupportTicketID foreign key (SupportTicketID) references dbo.SupportTicket(SupportTicketID),
	EmailAddresses varchar(200) not null,
	EmailSubject varchar(200) not null,
	EmailBody varchar(max) not null,
	SentDate datetime not null
)