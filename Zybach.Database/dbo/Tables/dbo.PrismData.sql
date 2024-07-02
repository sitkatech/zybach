CREATE TABLE [dbo].[PrismData]
(
	[PrismDataID]	INT NOT NULL IDENTITY(1,1),
	[ElementType]	VARCHAR(50),
	[Date]			DATETIME,
	[X]				INT,
	[Y]				INT,
	[Value]			FLOAT,

	CONSTRAINT	[PK_PrismID_PrismDataID] PRIMARY KEY CLUSTERED ([PrismDataID]),
	INDEX		[IX_PrismData_Date] ([Date])
);
