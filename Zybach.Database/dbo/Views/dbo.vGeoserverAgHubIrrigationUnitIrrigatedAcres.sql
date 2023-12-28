create view dbo.vGeoServerAgHubIrrigationUnitCropType
as

select AgHubIrrigationUnitID, WellTPID, IrrigationUnitGeometry, IrrigationYear, Acres, CropType, CropTypeLegendDisplayName, Tillage
from
(
	select ahiu.AgHubIrrigationUnitID, ahiu.WellTPID ,ahiug.IrrigationUnitGeometry, ahw.AgHubWellID,
		ahwia.AgHubWellIrrigatedAcreID, ahwia.IrrigationYear, ahwia.Acres, ahwia.Tillage, ahwia.CropType,
		(case when ahwia.CropType is null then  'Not Reported' else ahwia.CropType end) as CropTypeLegendDisplayName,
		rank() over (partition by ahw.AgHubIrrigationUnitID, ahwia.IrrigationYear order by ahwia.Acres desc, AgHubWellIrrigatedAcreID) as Ranking
	from dbo.AgHubIrrigationUnit ahiu
	join dbo.AgHubIrrigationUnitGeometry ahiug on ahiu.AgHubIrrigationUnitID = ahiug.AgHubIrrigationUnitID
	join dbo.AgHubWell ahw on ahiu.AgHubIrrigationUnitID = ahw.AgHubIrrigationUnitID
	join dbo.AgHubWellIrrigatedAcre ahwia on ahw.AgHubWellID = ahwia.AgHubWellID
) ahiuct
where Ranking = 1

GO
/*
select * from dbo.vGeoServerAgHubIrrigationUnitCropType
*/