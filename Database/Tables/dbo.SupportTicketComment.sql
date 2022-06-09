SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SupportTicketComment](
	[SupportTicketCommentID] [int] IDENTITY(1,1) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateUpdated] [datetime] NOT NULL,
	[CreatorUserID] [int] NOT NULL,
	[SupportTicketID] [int] NOT NULL,
	[CommentNotes] [varchar](500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_SupportTicketComment_SupportTicketCommentID] PRIMARY KEY CLUSTERED 
(
	[SupportTicketCommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[SupportTicketComment]  WITH CHECK ADD  CONSTRAINT [FK_SupportTicketComment_SupportTicket_SupportTicketID] FOREIGN KEY([SupportTicketID])
REFERENCES [dbo].[SupportTicket] ([SupportTicketID])
GO
ALTER TABLE [dbo].[SupportTicketComment] CHECK CONSTRAINT [FK_SupportTicketComment_SupportTicket_SupportTicketID]
GO
ALTER TABLE [dbo].[SupportTicketComment]  WITH CHECK ADD  CONSTRAINT [FK_SupportTicketComment_User_CreatorUserID_UserID] FOREIGN KEY([CreatorUserID])
REFERENCES [dbo].[User] ([UserID])
GO
ALTER TABLE [dbo].[SupportTicketComment] CHECK CONSTRAINT [FK_SupportTicketComment_User_CreatorUserID_UserID]