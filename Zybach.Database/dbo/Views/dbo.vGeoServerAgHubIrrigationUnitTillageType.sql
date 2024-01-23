create view dbo.vGeoServerAgHubIrrigationUnitTillageType
as

select AgHubIrrigationUnitID, WellTPID, IrrigationUnitGeometry, IrrigationYear, Acres, CropType, Tillage, TillageTypeLegendDisplayName, TillageTypeMapColor
from
(
	select ahiu.AgHubIrrigationUnitID, ahiu.WellTPID ,ahiug.IrrigationUnitGeometry, ahw.AgHubWellID,
		ahwia.AgHubWellIrrigatedAcreID, ahwia.IrrigationYear, ahwia.Acres, ahwia.Tillage, ahwia.CropType,
		rank() over (partition by ahw.AgHubIrrigationUnitID, ahwia.IrrigationYear order by ahwia.Acres desc, AgHubWellIrrigatedAcreID) as Ranking,
		-- populate TillageType "Not Reported" and "Other" categories
		case when ahwia.Tillage is null then 'Not Reported' 
			else (case when ahiatt.AgHubIrrigatedAcreTillageTypeID is null then 'Other' else ahwia.CropType end)
			end as TillageTypeLegendDisplayName,
		case when ahwia.CropType is null then '#e22e1d'
			else (case when ahiatt.AgHubIrrigatedAcreTillageTypeID is null then '#00b6b6' else ahiatt.MapColor end) 
			end as TillageTypeMapColor
	from dbo.AgHubIrrigationUnit ahiu
	join dbo.AgHubIrrigationUnitGeometry ahiug on ahiu.AgHubIrrigationUnitID = ahiug.AgHubIrrigationUnitID
	join dbo.AgHubWell ahw on ahiu.AgHubIrrigationUnitID = ahw.AgHubIrrigationUnitID
	join dbo.AgHubWellIrrigatedAcre ahwia on ahw.AgHubWellID = ahwia.AgHubWellID
	left join dbo.AgHubIrrigatedAcreTillageType ahiatt on ahwia.Tillage = ahiatt.AgHubIrrigatedAcreTillageTypeName
) ahiuct
where Ranking = 1

GO
/*
select * from dbo.vGeoServerAgHubIrrigationUnitTillageType
*/