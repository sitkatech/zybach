SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupportTicket](
	[SupportTicketID] [int] IDENTITY(1,1) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[CreatorUserID] [int] NOT NULL,
	[AssigneeUserID] [int] NULL,
	[WellID] [int] NOT NULL,
	[SensorID] [int] NULL,
	[SupportTicketStatusID] [int] NOT NULL,
	[SupportTicketPriorityID] [int] NOT NULL,
	[SupportTicketTitle] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SupportTicketDescription] [varchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_SupportTicket_SupportTicketID] PRIMARY KEY CLUSTERED 
(
	[SupportTicketID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[SupportTicket]  WITH CHECK ADD  CONSTRAINT [FK_SupportTicket_Sensor_SensorID] FOREIGN KEY([SensorID])
REFERENCES [dbo].[Sensor] ([SensorID])
GO
ALTER TABLE [dbo].[SupportTicket] CHECK CONSTRAINT [FK_SupportTicket_Sensor_SensorID]
GO
ALTER TABLE [dbo].[SupportTicket]  WITH CHECK ADD  CONSTRAINT [FK_SupportTicket_SupportTicketPriority_SupportTicketPriorityID] FOREIGN KEY([SupportTicketPriorityID])
REFERENCES [dbo].[SupportTicketPriority] ([SupportTicketPriorityID])
GO
ALTER TABLE [dbo].[SupportTicket] CHECK CONSTRAINT [FK_SupportTicket_SupportTicketPriority_SupportTicketPriorityID]
GO
ALTER TABLE [dbo].[SupportTicket]  WITH CHECK ADD  CONSTRAINT [FK_SupportTicket_SupportTicketStatus_SupportTicketStatusID] FOREIGN KEY([SupportTicketStatusID])
REFERENCES [dbo].[SupportTicketStatus] ([SupportTicketStatusID])
GO
ALTER TABLE [dbo].[SupportTicket] CHECK CONSTRAINT [FK_SupportTicket_SupportTicketStatus_SupportTicketStatusID]
GO
ALTER TABLE [dbo].[SupportTicket]  WITH CHECK ADD  CONSTRAINT [FK_SupportTicket_User_AssigneeUserID_UserID] FOREIGN KEY([AssigneeUserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[SupportTicket] CHECK CONSTRAINT [FK_SupportTicket_User_AssigneeUserID_UserID]
GO
ALTER TABLE [dbo].[SupportTicket]  WITH CHECK ADD  CONSTRAINT [FK_SupportTicket_User_CreatorUserID_UserID] FOREIGN KEY([CreatorUserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[SupportTicket] CHECK CONSTRAINT [FK_SupportTicket_User_CreatorUserID_UserID]
GO
ALTER TABLE [dbo].[SupportTicket]  WITH CHECK ADD  CONSTRAINT [FK_SupportTicket_Well_WellID] FOREIGN KEY([WellID])
REFERENCES [dbo].[Well] ([WellID])
GO
ALTER TABLE [dbo].[SupportTicket] CHECK CONSTRAINT [FK_SupportTicket_Well_WellID]