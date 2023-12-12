CREATE TABLE [dbo].[Well](
	[WellID] [int] IDENTITY(1,1) NOT NULL,
	[WellRegistrationID] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[WellGeometry] [geometry] NOT NULL,
	[StreamflowZoneID] [int] NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastUpdateDate] [datetime] NULL,
	[WellNickname] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[TownshipRangeSection] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[CountyID] [int] NULL,
	[WellParticipationID] [int] NULL,
	[WellUseID] [int] NULL,
	[RequiresChemigation] [bit] NOT NULL,
	[RequiresWaterLevelInspection] [bit] NOT NULL,
	[WellDepth] [decimal](10, 4) NULL,
	[Clearinghouse] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[PageNumber] [int] NULL,
	[SiteName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SiteNumber] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ScreenInterval] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[ScreenDepth] [decimal](10, 4) NULL,
	[OwnerName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[OwnerAddress] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[OwnerCity] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[OwnerState] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[OwnerZipCode] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AdditionalContactName] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AdditionalContactAddress] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AdditionalContactCity] [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AdditionalContactState] [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AdditionalContactZipCode] [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[IsReplacement] [bit] NOT NULL,
	[Notes] [varchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
 CONSTRAINT [PK_Well_WellID] PRIMARY KEY CLUSTERED 
(
	[WellID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [AK_Well_WellRegistrationID] UNIQUE NONCLUSTERED 
(
	[WellRegistrationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER TABLE [dbo].[Well]  WITH CHECK ADD  CONSTRAINT [FK_Well_County_CountyID] FOREIGN KEY([CountyID])
REFERENCES [dbo].[County] ([CountyID])
GO
ALTER TABLE [dbo].[Well] CHECK CONSTRAINT [FK_Well_County_CountyID]
GO
ALTER TABLE [dbo].[Well]  WITH CHECK ADD  CONSTRAINT [FK_Well_StreamFlowZone_StreamFlowZoneID] FOREIGN KEY([StreamflowZoneID])
REFERENCES [dbo].[StreamFlowZone] ([StreamFlowZoneID])
GO
ALTER TABLE [dbo].[Well] CHECK CONSTRAINT [FK_Well_StreamFlowZone_StreamFlowZoneID]
GO
ALTER TABLE [dbo].[Well]  WITH CHECK ADD  CONSTRAINT [FK_Well_WellParticipation_WellParticipationID] FOREIGN KEY([WellParticipationID])
REFERENCES [dbo].[WellParticipation] ([WellParticipationID])
GO
ALTER TABLE [dbo].[Well] CHECK CONSTRAINT [FK_Well_WellParticipation_WellParticipationID]
GO
ALTER TABLE [dbo].[Well]  WITH CHECK ADD  CONSTRAINT [FK_Well_WellUse_WellUseID] FOREIGN KEY([WellUseID])
REFERENCES [dbo].[WellUse] ([WellUseID])
GO
ALTER TABLE [dbo].[Well] CHECK CONSTRAINT [FK_Well_WellUse_WellUseID]