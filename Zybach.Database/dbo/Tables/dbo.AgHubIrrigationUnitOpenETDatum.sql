CREATE TABLE [dbo].[AgHubIrrigationUnitOpenETDatum](
	[AgHubIrrigationUnitOpenETDatumID] [int] IDENTITY(1,1) NOT NULL,
	[AgHubIrrigationUnitID] [int] NOT NULL,
	[OpenETDataTypeID] [int] NOT NULL,
	[ReportedDate] [datetime] NOT NULL,
	[TransactionDate] [datetime] NOT NULL,
	[ReportedValueInches] [decimal](20, 4) NULL,
	[AgHubIrrigationUnitAreaInAcres] [decimal](20, 4) NULL,
 CONSTRAINT [PK_AgHubIrrigationUnitOpenETDatum_AgHubIrrigationUnitOpenETDatumID] PRIMARY KEY CLUSTERED 
(
	[AgHubIrrigationUnitOpenETDatumID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[AgHubIrrigationUnitOpenETDatum]  WITH CHECK ADD  CONSTRAINT [FK_AgHubIrrigationUnitOpenETDatum_AgHubIrrigationUnit_AgHubIrrigationUnitID] FOREIGN KEY([AgHubIrrigationUnitID])
REFERENCES [dbo].[AgHubIrrigationUnit] ([AgHubIrrigationUnitID])
GO
ALTER TABLE [dbo].[AgHubIrrigationUnitOpenETDatum] CHECK CONSTRAINT [FK_AgHubIrrigationUnitOpenETDatum_AgHubIrrigationUnit_AgHubIrrigationUnitID]
GO
ALTER TABLE [dbo].[AgHubIrrigationUnitOpenETDatum]  WITH CHECK ADD  CONSTRAINT [FK_AgHubIrrigationUnitOpenETDatum_OpenETDataType_OpenETDataTypeID] FOREIGN KEY([OpenETDataTypeID])
REFERENCES [dbo].[OpenETDataType] ([OpenETDataTypeID])
GO
ALTER TABLE [dbo].[AgHubIrrigationUnitOpenETDatum] CHECK CONSTRAINT [FK_AgHubIrrigationUnitOpenETDatum_OpenETDataType_OpenETDataTypeID]