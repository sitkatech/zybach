CREATE TABLE [dbo].[OpenETSyncHistory](
	[OpenETSyncHistoryID] [int] IDENTITY(1,1) NOT NULL,
	[OpenETSyncResultTypeID] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[GoogleBucketFileRetrievalURL] [varchar](200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ErrorMessage] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[OpenETDataTypeID] [int] NULL,
	[OpenETSyncID] [int] NULL,
 CONSTRAINT [PK_OpenETSyncHistory_OpenETSyncHistoryID] PRIMARY KEY CLUSTERED 
(
	[OpenETSyncHistoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[OpenETSyncHistory]  WITH CHECK ADD  CONSTRAINT [FK_OpenETSyncHistory_OpenETDataType_OpenETDataTypeID] FOREIGN KEY([OpenETDataTypeID])
REFERENCES [dbo].[OpenETDataType] ([OpenETDataTypeID])
GO
ALTER TABLE [dbo].[OpenETSyncHistory] CHECK CONSTRAINT [FK_OpenETSyncHistory_OpenETDataType_OpenETDataTypeID]
GO
ALTER TABLE [dbo].[OpenETSyncHistory]  WITH CHECK ADD  CONSTRAINT [FK_OpenETSyncHistory_OpenETSync_OpenETSyncID] FOREIGN KEY([OpenETSyncID])
REFERENCES [dbo].[OpenETSync] ([OpenETSyncID])
GO
ALTER TABLE [dbo].[OpenETSyncHistory] CHECK CONSTRAINT [FK_OpenETSyncHistory_OpenETSync_OpenETSyncID]
GO
ALTER TABLE [dbo].[OpenETSyncHistory]  WITH CHECK ADD  CONSTRAINT [FK_OpenETSyncHistory_OpenETSyncResultType_OpenETSyncResultTypeID] FOREIGN KEY([OpenETSyncResultTypeID])
REFERENCES [dbo].[OpenETSyncResultType] ([OpenETSyncResultTypeID])
GO
ALTER TABLE [dbo].[OpenETSyncHistory] CHECK CONSTRAINT [FK_OpenETSyncHistory_OpenETSyncResultType_OpenETSyncResultTypeID]