CREATE TABLE [dbo].[RunoffCalculationStatus]
(
	[RunoffCalculationStatusID]				INT NOT NULL,
	[RunoffCalculationStatusName]			VARCHAR(100) NOT NULL,
	[RunoffCalculationStatusDisplayName]	VARCHAR(100) NOT NULL,
	
	CONSTRAINT [PK_RunoffCalculationStatus_RunoffCalculationStatus] PRIMARY KEY CLUSTERED ([RunoffCalculationStatusID] ASC)
)
