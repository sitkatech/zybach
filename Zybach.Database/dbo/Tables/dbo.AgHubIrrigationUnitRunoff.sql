CREATE TABLE [dbo].[AgHubIrrigationUnitRunoff](
	[AgHubIrrigationUnitRunoffID]	INT			NOT NULL IDENTITY(1,1), 
	[AgHubIrrigationUnitID]			INT			NOT NULL, 
		
	[Year]							INT			NOT NULL,
	[Month]							INT			NOT NULL,
	[Day]							INT			NOT NULL,
	
	[CropType]						VARCHAR(50) NULL,
	[Tillage]						VARCHAR(50) NULL,
	[CurveNumber]					FLOAT		NOT NULL,

	[Precipitation]					FLOAT		NOT NULL,
	[Area]							FLOAT		NOT NULL,		
	[RunoffDepth]					FLOAT		NOT NULL,
	[RunoffVolume]					FLOAT		NOT NULL,

    CONSTRAINT [PK_AgHubIrrigationUnitRunoff_AgHubIrrigationUnitRunoffID]	PRIMARY KEY CLUSTERED ([AgHubIrrigationUnitRunoffID]),

	CONSTRAINT [FK_AgHubIrrigationUnitRunoff_AgHubIrrigationUnitID]			FOREIGN KEY ([AgHubIrrigationUnitID])	REFERENCES [dbo].[AgHubIrrigationUnit]([AgHubIrrigationUnitID]),
	CONSTRAINT [AK_AgHubIrrigationUnitRunoff_Year_Month_Day]				UNIQUE ([AgHubIrrigationUnitID] ASC, [Year] ASC, [Month] ASC, [Day] ASC)
)