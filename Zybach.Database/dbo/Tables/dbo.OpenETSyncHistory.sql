CREATE TABLE [dbo].[OpenETSyncHistory](
	[OpenETSyncHistoryID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_OpenETSyncHistory_OpenETSyncHistoryID] PRIMARY KEY,
	[OpenETSyncResultTypeID] [int] NOT NULL CONSTRAINT [FK_OpenETSyncHistory_OpenETSyncResultType_OpenETSyncResultTypeID] FOREIGN KEY REFERENCES [dbo].[OpenETSyncResultType] ([OpenETSyncResultTypeID]),
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[GoogleBucketFileRetrievalURL] [varchar](200) NULL,
	[ErrorMessage] [varchar](max) NULL,
	[OpenETDataTypeID] [int] NULL CONSTRAINT [FK_OpenETSyncHistory_OpenETDataType_OpenETDataTypeID] FOREIGN KEY REFERENCES [dbo].[OpenETDataType] ([OpenETDataTypeID]),
	[OpenETSyncID] [int] NULL CONSTRAINT [FK_OpenETSyncHistory_OpenETSync_OpenETSyncID] FOREIGN KEY REFERENCES [dbo].[OpenETSync] ([OpenETSyncID]),
)