

CREATE TABLE [dbo].[ReportTemplateModelType](
	[ReportTemplateModelTypeID] [int] NOT NULL,
	[ReportTemplateModelTypeName] [varchar](100) NOT NULL,
	[ReportTemplateModelTypeDisplayName] [varchar](100) NOT NULL,
	[ReportTemplateModelTypeDescription] [varchar](250) NOT NULL,
 CONSTRAINT [PK_ReportTemplateModelType_ReportTemplateModelTypeID] PRIMARY KEY CLUSTERED 
(
	[ReportTemplateModelTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ReportTemplateModelType_ReportTemplateModelTypeDisplayName] UNIQUE NONCLUSTERED 
(
	[ReportTemplateModelTypeDisplayName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ReportTemplateModelType_ReportTemplateModelTypeName] UNIQUE NONCLUSTERED 
(
	[ReportTemplateModelTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO





CREATE TABLE [dbo].[ReportTemplateModel](
	[ReportTemplateModelID] [int] NOT NULL,
	[ReportTemplateModelName] [varchar](100) NOT NULL,
	[ReportTemplateModelDisplayName] [varchar](100) NOT NULL,
	[ReportTemplateModelDescription] [varchar](250) NOT NULL,
 CONSTRAINT [PK_ReportTemplateModel_ReportTemplateModelID] PRIMARY KEY CLUSTERED 
(
	[ReportTemplateModelID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ReportTemplateModel_ReportTemplateModelDisplayName] UNIQUE NONCLUSTERED 
(
	[ReportTemplateModelDisplayName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ReportTemplateModel_ReportTemplateModelName] UNIQUE NONCLUSTERED 
(
	[ReportTemplateModelName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



CREATE TABLE [dbo].[ReportTemplate](
	[ReportTemplateID] [int] IDENTITY(1,1) NOT NULL,
	[FileResourceID] [int] NOT NULL,
	[DisplayName] [varchar](50) NOT NULL,
	[Description] [varchar](250) NULL,
	[ReportTemplateModelTypeID] [int] NOT NULL,
	[ReportTemplateModelID] [int] NOT NULL,
 CONSTRAINT [PK_ReportTemplate_ReportTemplateID] PRIMARY KEY CLUSTERED 
(
	[ReportTemplateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_ReportTemplate_DisplayName] UNIQUE NONCLUSTERED 
(
	[DisplayName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],

) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ReportTemplate]  WITH CHECK ADD  CONSTRAINT [FK_ReportTemplate_FileResource_FileResourceID] FOREIGN KEY([FileResourceID])
REFERENCES [dbo].[FileResource] ([FileResourceID])
GO

ALTER TABLE [dbo].[ReportTemplate] CHECK CONSTRAINT [FK_ReportTemplate_FileResource_FileResourceID]
GO


ALTER TABLE [dbo].[ReportTemplate]  WITH CHECK ADD  CONSTRAINT [FK_ReportTemplate_ReportTemplateModel_ReportTemplateModelID] FOREIGN KEY([ReportTemplateModelID])
REFERENCES [dbo].[ReportTemplateModel] ([ReportTemplateModelID])
GO

ALTER TABLE [dbo].[ReportTemplate] CHECK CONSTRAINT [FK_ReportTemplate_ReportTemplateModel_ReportTemplateModelID]
GO

ALTER TABLE [dbo].[ReportTemplate]  WITH CHECK ADD  CONSTRAINT [FK_ReportTemplate_ReportTemplateModelType_ReportTemplateModelTypeID] FOREIGN KEY([ReportTemplateModelTypeID])
REFERENCES [dbo].[ReportTemplateModelType] ([ReportTemplateModelTypeID])
GO

ALTER TABLE [dbo].[ReportTemplate] CHECK CONSTRAINT [FK_ReportTemplate_ReportTemplateModelType_ReportTemplateModelTypeID]
GO



insert into dbo.CustomRichTextType values (8, 'ReportsList', 'Reports List')
insert into dbo.CustomRichText (CustomRichTextTypeID, CustomRichTextContent)
values (8, 'Reports List')