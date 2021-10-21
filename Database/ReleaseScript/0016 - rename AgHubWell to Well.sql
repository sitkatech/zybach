exec sp_rename 'dbo.PK_AgHubWell_AgHubWellID', 'PK_Well_WellID', 'OBJECT';
exec sp_rename 'dbo.FK_AghubWell_StreamFlowZone_StreamFlowZoneID', 'FK_Well_StreamFlowZone_StreamFlowZoneID', 'OBJECT';
exec sp_rename 'dbo.AgHubWell.AgHubWellID', 'WellID', 'COLUMN';
exec sp_rename 'dbo.AgHubWell', 'Well';

alter table dbo.Well add CreateDate datetime null, LastUpdateDate datetime null
GO

update dbo.Well
set CreateDate = FetchDate, LastUpdateDate = FetchDate

alter table dbo.Well alter column CreateDate datetime not null
alter table dbo.Well alter column LastUpdateDate datetime null

