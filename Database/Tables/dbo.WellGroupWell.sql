CREATE TABLE [dbo].[WellGroupWell] (
	[WellGroupWellID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_WellGroupWell_WellGroupWellID] PRIMARY KEY,
	[WellGroupID] [int] NOT NULL CONSTRAINT [FK_WellGroupWell_WellGroup_WellGroupID] FOREIGN KEY REFERENCES [dbo].[WellGroup]([WellGroupID]),
	[WellID] [int] NOT NULL CONSTRAINT [FK_WellGroupWell_Well_WellID] FOREIGN KEY REFERENCES [dbo].[Well]([WellID]),
	[IsPrimary] [bit] NOT NULL
)