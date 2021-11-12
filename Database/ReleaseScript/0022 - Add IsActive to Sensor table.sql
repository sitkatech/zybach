alter table dbo.Sensor add IsActive bit null
go
update dbo.Sensor set IsActive = 1
alter table dbo.Sensor alter column IsActive bit not null