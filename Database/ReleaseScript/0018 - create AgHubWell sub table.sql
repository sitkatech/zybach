CREATE TABLE dbo.AgHubWell(
	AgHubWellID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_AgHubWell_AgHubWellID PRIMARY KEY,
	WellID int not null constraint FK_AgHubWell_Well_WellID foreign key references dbo.Well(WellID) constraint AK_AgHubWell_WellID unique,
	WellTPID varchar(100) NULL,
	AgHubWellGeometry geometry NOT NULL,
	WellTPNRDPumpRate int NULL,
	TPNRDPumpRateUpdated datetime NULL,
	WellConnectedMeter bit NOT NULL,
	WellAuditPumpRate int NULL,
	AuditPumpRateUpdated datetime NULL,
	HasElectricalData bit NOT NULL,
	RegisteredPumpRate int NULL,
	RegisteredUpdated datetime NULL,
	AgHubRegisteredUser varchar(100) NULL,
	FieldName varchar(100) NULL
)
GO

set identity_insert dbo.AgHubWell on
insert into dbo.AgHubWell(AgHubWellID, WellID, WellTPID, AgHubWellGeometry, WellTPNRDPumpRate, TPNRDPumpRateUpdated, WellConnectedMeter, WellAuditPumpRate, AuditPumpRateUpdated, HasElectricalData, RegisteredPumpRate, RegisteredUpdated, AgHubRegisteredUser, FieldName)
select WellID as AgHubWellID, WellID, WellTPID, WellGeometry as AgHubWellGeometry, WellTPNRDPumpRate, TPNRDPumpRateUpdated, WellConnectedMeter, WellAuditPumpRate, AuditPumpRateUpdated, HasElectricalData, RegisteredPumpRate, RegisteredUpdated, AgHubRegisteredUser, FieldName
from dbo.Well
order by WellID

set identity_insert dbo.AgHubWell off

alter table dbo.Well drop column WellTPID
alter table dbo.Well drop column WellTPNRDPumpRate
alter table dbo.Well drop column TPNRDPumpRateUpdated
alter table dbo.Well drop column WellConnectedMeter
alter table dbo.Well drop column WellAuditPumpRate
alter table dbo.Well drop column AuditPumpRateUpdated
alter table dbo.Well drop column HasElectricalData
alter table dbo.Well drop column FetchDate
alter table dbo.Well drop column RegisteredPumpRate
alter table dbo.Well drop column RegisteredUpdated
alter table dbo.Well drop column AgHubRegisteredUser
alter table dbo.Well drop column FieldName

alter table dbo.AgHubWellIrrigatedAcre drop constraint FK_AgHubWellIrrigatedAcre_AgHubWell_AgHubWellID
GO

alter table dbo.AgHubWellIrrigatedAcre add constraint FK_AgHubWellIrrigatedAcre_AgHubWell_AgHubWellID foreign key (AgHubWellID) references dbo.AgHubWell(AgHubWellID)
