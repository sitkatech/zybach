CREATE TABLE [dbo].[SupportTicketNotification](
	[SupportTicketNotificationID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_SupportTicketNotification_SupportTicketNotificationID] PRIMARY KEY,
	[SupportTicketID] [int] NOT NULL CONSTRAINT [FK_SupportTicketNotification_SupportTicket_SupportTicketID] FOREIGN KEY REFERENCES [dbo].[SupportTicket] ([SupportTicketID]),
	[EmailAddresses] [varchar](200) NOT NULL,
	[EmailSubject] [varchar](200) NOT NULL,
	[EmailBody] [varchar](max) NOT NULL,
	[SentDate] [datetime] NOT NULL
)