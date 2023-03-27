CREATE TABLE [dbo].[WellGroup] (
	[WellGroupID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_WellGroup_WellID] PRIMARY KEY,
	[WellGroupName] [varchar](50) NOT NULL CONSTRAINT [AK_WellGroup_WellGroupName] UNIQUE
)
