create table dbo.AgHubWellIrrigationUnit(
	AgHubWellIrrigationUnitID int not null identity(1,1) constraint PK_AgHubWellIrrigationUnit_AgHubWellIrrigationUnitID primary key,
	AgHubWellID int not null constraint FK_AgHubWellIrrigationUnit_AgHubWell_AgHubWellID foreign key references dbo.AgHubWell(AgHubWellID) constraint AK_AgHubWellIrrigationUnit_AgHubWellID unique,
	IrrigationUnitGeometry geometry not null
)

alter table dbo.AgHubWellStaging add IrrigationUnitGeometry geometry null