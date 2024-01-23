CREATE TABLE [dbo].[AgHubIrrigatedAcreTillageType](
	[AgHubIrrigatedAcreTillageTypeID] [int] NOT NULL CONSTRAINT [PK_AgHubIrrigatedAcreTillageType_AgHubIrrigatedAcreTillageTypeID] PRIMARY KEY,
	[AgHubIrrigatedAcreTillageTypeName] [varchar](100) NOT NULL,
	[AgHubIrrigatedAcreTillageTypeDisplayName] [varchar](100) NOT NULL,
	[MapColor] [varchar](50) NOT NULL
)
