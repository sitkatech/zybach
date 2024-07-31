CREATE TABLE [dbo].[AgHubIrrigationUnitCurveNumber](
	[AgHubIrrigationUnitCurveNumberID]	INT			NOT NULL IDENTITY(1,1), 
	[AgHubIrrigationUnitID]				INT			NOT NULL, 

	[HydrologicSoilGroup]				VARCHAR(3)	NULL,

	[MTillCurveNumber]					FLOAT		NOT NULL,
	[STillCurveNumber]					FLOAT		NOT NULL,
	[NTillCurveNumber]					FLOAT		NOT NULL,
	[CTillCurveNumber]					FLOAT		NOT NULL,
	[UndefinedTillCurveNumber]			FLOAT		NOT NULL,

    CONSTRAINT [PK_AgHubIrrigationUnitCurveNumber_AgHubIrrigationUnitRunoffID]	PRIMARY KEY CLUSTERED ([AgHubIrrigationUnitCurveNumberID]),

	CONSTRAINT [FK_AgHubIrrigationUnitCurveNumber_AgHubIrrigationUnitID]		FOREIGN KEY ([AgHubIrrigationUnitID])	REFERENCES [dbo].[AgHubIrrigationUnit]([AgHubIrrigationUnitID]),
)