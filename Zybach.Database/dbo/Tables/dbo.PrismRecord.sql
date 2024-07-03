CREATE TABLE [dbo].[PrismRecord]
(
	[PrismRecordID]	INT NOT NULL IDENTITY(1,1),
	[ElementType]	VARCHAR(50),
	[Date]			DATETIME,
	[BandIndex]		INT,
	[X]				INT,
	[Y]				INT,
	[Value]			FLOAT,

	CONSTRAINT	[PK_PrismRecord_RecordID] PRIMARY KEY CLUSTERED ([PrismRecordID]),
	INDEX		[IX_PrismRecord_Date] ([Date])
);
