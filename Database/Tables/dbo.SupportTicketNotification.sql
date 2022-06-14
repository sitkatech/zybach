SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupportTicketNotification](
	[SupportTicketNotificationID] [int] IDENTITY(1,1) NOT NULL,
	[SupportTicketID] [int] NOT NULL,
	[EmailAddresses] [varchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[EmailSubject] [varchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[EmailBody] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[SentDate] [datetime] NOT NULL,
 CONSTRAINT [PK_SupportTicketNotification_SupportTicketNotificationID] PRIMARY KEY CLUSTERED 
(
	[SupportTicketNotificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[SupportTicketNotification]  WITH CHECK ADD  CONSTRAINT [FK_SupportTicketNotification_SupportTicket_SupportTicketID] FOREIGN KEY([SupportTicketID])
REFERENCES [dbo].[SupportTicket] ([SupportTicketID])
GO
ALTER TABLE [dbo].[SupportTicketNotification] CHECK CONSTRAINT [FK_SupportTicketNotification_SupportTicket_SupportTicketID]