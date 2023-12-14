CREATE TABLE [dbo].[SupportTicketComment](
	[SupportTicketCommentID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_SupportTicketComment_SupportTicketCommentID] PRIMARY KEY,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[CreatorUserID] [int] NOT NULL CONSTRAINT [FK_SupportTicketComment_User_CreatorUserID_UserID] FOREIGN KEY REFERENCES [dbo].[User] ([UserID]),
	[SupportTicketID] [int] NOT NULL CONSTRAINT [FK_SupportTicketComment_SupportTicket_SupportTicketID] FOREIGN KEY REFERENCES [dbo].[SupportTicket] ([SupportTicketID]),
	[CommentNotes] [varchar](500) NULL
)