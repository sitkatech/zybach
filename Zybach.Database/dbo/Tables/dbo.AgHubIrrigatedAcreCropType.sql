CREATE TABLE [dbo].[AgHubIrrigatedAcreCropType](
	[AgHubIrrigatedAcreCropTypeID] [int] NOT NULL CONSTRAINT [PK_AgHubIrrigatedAcreCropType_AgHubIrrigatedAcreCropTypeID] PRIMARY KEY,
	[AgHubIrrigatedAcreCropTypeName] [varchar](100) NOT NULL,
	[MapColor] [varchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL
)
