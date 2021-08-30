alter table dbo.AghubWell add StreamflowZoneID int null constraint FK_AghubWell_StreamFlowZone_StreamFlowZoneID foreign key references dbo.StreamFlowZone(StreamFlowZoneID)
GO

update aw
set StreamflowZoneID = sfz.StreamFlowZoneID--, sfz.StreamFlowZoneName, aw.AgHubWellID, aw.WellRegistrationID
from dbo.StreamFlowZone sfz
join dbo.AgHubWell aw on aw.WellGeometry.STWithin(sfz.StreamFlowZoneGeometry) = 1