create view dbo.vGeoServerAgHubIrrigationUnitCropType
as

select AgHubIrrigationUnitID, WellTPID, IrrigationUnitGeometry, IrrigationYear, Acres, CropType, CropTypeLegendDisplayName, CropTypeMapColor, Tillage
from
(
	select ahiu.AgHubIrrigationUnitID, ahiu.WellTPID ,ahiug.IrrigationUnitGeometry, ahw.AgHubWellID,
		ahwia.AgHubWellIrrigatedAcreID, ahwia.IrrigationYear, ahwia.Acres, ahwia.Tillage, ahwia.CropType,
		rank() over (partition by ahw.AgHubIrrigationUnitID, ahwia.IrrigationYear order by ahwia.Acres desc, AgHubWellIrrigatedAcreID) as Ranking,
		-- populate CropType "Not Reported" and "Other" categories
		case when ahwia.CropType is null then 'Not Reported' 
			else (case when ahiact.AgHubIrrigatedAcreCropTypeID is null then 'Other' else ahwia.CropType end)
			end as CropTypeLegendDisplayName,
		case when ahwia.CropType is null then '#e22e1d'
			else (case when ahiact.AgHubIrrigatedAcreCropTypeID is null then '#00b6b6' else ahiact.MapColor end) 
			end as CropTypeMapColor
	from dbo.AgHubIrrigationUnit ahiu
	join dbo.AgHubIrrigationUnitGeometry ahiug on ahiu.AgHubIrrigationUnitID = ahiug.AgHubIrrigationUnitID
	join dbo.AgHubWell ahw on ahiu.AgHubIrrigationUnitID = ahw.AgHubIrrigationUnitID
	join dbo.AgHubWellIrrigatedAcre ahwia on ahw.AgHubWellID = ahwia.AgHubWellID
	left join dbo.AgHubIrrigatedAcreCropType ahiact on ahwia.CropType = ahiact.AgHubIrrigatedAcreCropTypeName
) ahiuct
where Ranking = 1

GO
/*
select * from dbo.vGeoServerAgHubIrrigationUnitCropType
*/