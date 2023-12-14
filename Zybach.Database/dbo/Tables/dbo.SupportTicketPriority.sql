CREATE TABLE [dbo].[SupportTicketPriority](
	[SupportTicketPriorityID] [int] NOT NULL,
	[SupportTicketPriorityName] [varchar](50) NOT NULL,
	[SupportTicketPriorityDisplayName] [varchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
 CONSTRAINT [PK_SupportTicketPriority_SupportTicketPriorityID] PRIMARY KEY CLUSTERED 
(
	[SupportTicketPriorityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
