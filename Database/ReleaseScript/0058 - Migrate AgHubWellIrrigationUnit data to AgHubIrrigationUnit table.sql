CREATE TABLE dbo.AgHubIrrigationUnit (
	[AgHubIrrigationUnitID] [int] IDENTITY(1,1) NOT NULL CONSTRAINT [PK_AgHubIrrigationUnit_AgHubIrrigationUnitID] PRIMARY KEY,
	[WellTPID] [varchar](100) NOT NULL CONSTRAINT AK_AgHubIrrigationUnit_WellTPID UNIQUE,
	[IrrigationUnitGeometry] [geometry] NULL,
	[IrrigationUnitAreaInAcres] float NULL
)

INSERT INTO dbo.AgHubIrrigationUnit (WellTPID, IrrigationUnitGeometry)
SELECT Distinct WellTPID, IrrigationUnitGeometry.STAsText()
FROM dbo.AgHubWellIrrigationUnit ahwiu
JOIN dbo.AgHubWell ahw on ahwiu.AgHubWellID = ahw.AgHubWellID

DROP TABLE dbo.AgHubWellIrrigationUnit

alter table dbo.AgHubWell 
add AgHubIrrigationUnitID int null constraint FK_AgHubWell_AgHubIrrigationUnit_AgHubIrrigationUnitID references dbo.AgHubIrrigationUnit(AgHubIrrigationUnitID)

go

update dbo.AgHubWell
set AgHubIrrigationUnitID = ahiu.AgHubIrrigationUnitID
from dbo.AgHubWell ahw
left join dbo.AgHubIrrigationUnit ahiu on ahw.WellTPID = ahiu.WellTPID

alter table dbo.AgHubWell
drop column WellTPID

--Add rich text entry for irrigation unit index
Insert into dbo.CustomRichTextType (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
values (22, 'IrrigationUnitIndex', 'Irrigation Unit Index')

Insert into dbo.CustomRichText(CustomRichTextTypeID, CustomRichTextContent)
values (22, 'Editable text for irrigation unit index page: TPIDs are links to irrigation unit detail pages, Well IDs are links to well detail pages')