CREATE TABLE [dbo].[SupportTicket](
	[SupportTicketID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_SupportTicket_SupportTicketID] PRIMARY KEY,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[CreatorUserID] [int] NOT NULL CONSTRAINT [FK_SupportTicket_User_CreatorUserID_UserID] FOREIGN KEY REFERENCES [dbo].[User] ([UserID]),
	[AssigneeUserID] [int] NULL CONSTRAINT [FK_SupportTicket_User_AssigneeUserID_UserID] FOREIGN KEY REFERENCES [dbo].[User] ([UserID]),
	[WellID] [int] NOT NULL CONSTRAINT [FK_SupportTicket_Well_WellID] FOREIGN KEY REFERENCES [dbo].[Well] ([WellID]),
	[SensorID] [int] NULL CONSTRAINT [FK_SupportTicket_Sensor_SensorID] FOREIGN KEY REFERENCES [dbo].[Sensor] ([SensorID]),
	[SupportTicketStatusID] [int] NOT NULL CONSTRAINT [FK_SupportTicket_SupportTicketStatus_SupportTicketStatusID] FOREIGN KEY REFERENCES [dbo].[SupportTicketStatus] ([SupportTicketStatusID]),
	[SupportTicketPriorityID] [int] NOT NULL CONSTRAINT [FK_SupportTicket_SupportTicketPriority_SupportTicketPriorityID] FOREIGN KEY REFERENCES [dbo].[SupportTicketPriority] ([SupportTicketPriorityID]),
	[SupportTicketTitle] [varchar](100) NOT NULL,
	[SupportTicketDescription] [varchar](500) NULL
)