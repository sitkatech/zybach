create table dbo.AgHubWell
(
	AgHubWellID int not null identity(1, 1) constraint PK_AgHubWell_AgHubWellID primary key,
	WellRegistrationID varchar(100) not null constraint AK_AgHubWell_WellRegistrationID unique,
	WellTPID varchar(100) not null constraint AK_AgHubWell_WellTPID unique,
	WellGeometry geometry not null,
	TPNRDPumpRate int not null,
	TPNRDPumpRateUpdated datetime not null,	
	WellConnectedMeter bit null,
	WellAuditPumpRate int null,
	AuditPumpRateUpdated datetime not null,
	HasElectricalData bit null,
	FetchDate datetime not null
)

create table dbo.AgHubWellIrrigatedAcre
(
	AgHubWellIrrigatedAcreID int not null identity(1,1) constraint PK_AgHubWellIrrigatedAcre_AgHubWellIrrigatedAcreID primary key,
	AgHubWellID int not null constraint FK_AgHubWellIrrigatedAcre_AgHubWell_AgHubWellID foreign key references dbo.AgHubWell(AgHubWellID),
	IrrigationYear int not null,
	Acres decimal(8,2) not null,
	FetchDate datetime not null,
	constraint AK_AgHubWellIrrigatedAcre_AgHubWellID_IrrigationYear unique (AgHubWellID, IrrigationYear)
)


create table dbo.StreamFlowZone
(
	StreamFlowZoneID int not null identity(1,1) constraint PK_StreamFlowZone_StreamFlowZoneID primary key,
	StreamFlowZoneName varchar(100) not null constraint AK_StreamFlowZone_StreamFlowZoneName unique,
	StreamFlowZoneGeometry geometry not null,
	StreamFlowZoneLength float not null,
	StreamFlowZoneArea float not null
)