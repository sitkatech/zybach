CREATE TABLE [dbo].[WellGroupWell](
	[WellGroupWellID] [int] IDENTITY(1,1) NOT NULL,
	[WellGroupID] [int] NOT NULL,
	[WellID] [int] NOT NULL,
	[IsPrimary] [bit] NOT NULL,
 CONSTRAINT [PK_WellGroupWell_WellGroupWellID] PRIMARY KEY CLUSTERED 
(
	[WellGroupWellID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[WellGroupWell]  WITH CHECK ADD  CONSTRAINT [FK_WellGroupWell_Well_WellID] FOREIGN KEY([WellID])
REFERENCES [dbo].[Well] ([WellID])
GO
ALTER TABLE [dbo].[WellGroupWell] CHECK CONSTRAINT [FK_WellGroupWell_Well_WellID]
GO
ALTER TABLE [dbo].[WellGroupWell]  WITH CHECK ADD  CONSTRAINT [FK_WellGroupWell_WellGroup_WellGroupID] FOREIGN KEY([WellGroupID])
REFERENCES [dbo].[WellGroup] ([WellGroupID])
GO
ALTER TABLE [dbo].[WellGroupWell] CHECK CONSTRAINT [FK_WellGroupWell_WellGroup_WellGroupID]