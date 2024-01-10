CREATE TABLE [dbo].[FileResourceMimeType](
	[FileResourceMimeTypeID] [int] NOT NULL,
	[FileResourceMimeTypeName] [varchar](100) NOT NULL,
	[FileResourceMimeTypeDisplayName] [varchar](100) NOT NULL,
	[FileResourceMimeTypeContentTypeName] [varchar](100) NOT NULL,
	[FileResourceMimeTypeIconSmallFilename] [varchar](100) NULL,
	[FileResourceMimeTypeIconNormalFilename] [varchar](100) NULL,
 CONSTRAINT [PK_FileResourceMimeType_FileResourceMimeTypeID] PRIMARY KEY CLUSTERED 
(
	[FileResourceMimeTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_FileResourceMimeType_FileResourceMimeTypeDisplayName] UNIQUE NONCLUSTERED 
(
	[FileResourceMimeTypeDisplayName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_FileResourceMimeType_FileResourceMimeTypeName] UNIQUE NONCLUSTERED 
(
	[FileResourceMimeTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
