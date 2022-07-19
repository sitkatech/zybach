create table dbo.AgHubIrrigationUnitGeometry (
	AgHubIrrigationUnitGeometryID int not null identity(1, 1) constraint PK_AgHubIrrigationUnitGeometry_AgHubIrrigationUnitGeometryID primary key,
	AgHubIrrigationUnitID int not null constraint FK_AgHubIrrigationUnitGeometry_AgHubIrrigationUnit_AgHubIrrigationUnitID foreign key references dbo.AgHubIrrigationUnit(AgHubIrrigationUnitID),
	IrrigationUnitGeometry geometry not null
	constraint AK_AgHubIrrigationUnitGeometry_AgHubIrrigationUnitID unique(AgHubIrrigationUnitID)
)

go

insert into dbo.AgHubIrrigationUnitGeometry (AgHubIrrigationUnitID, IrrigationUnitGeometry)
select AgHubIrrigationUnitID, IrrigationUnitGeometry from dbo.AgHubIrrigationUnit
where IrrigationUnitGeometry is not null

go 

alter table dbo.AgHubIrrigationUnit
drop column IrrigationUnitGeometry