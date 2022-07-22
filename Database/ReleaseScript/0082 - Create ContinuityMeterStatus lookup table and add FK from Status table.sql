create table dbo.ContinuityMeterStatus 
(
	ContinuityMeterStatusID int not null constraint PK_ContinuityMeterStatus_ContinuityMeterStatusID primary key,
	ContinuityMeterStatusName varchar(20) not null,
	ContinuityMeterStatusDisplayName varchar(20) not null
)

go 

insert into dbo.ContinuityMeterStatus (ContinuityMeterStatusID, ContinuityMeterStatusName, ContinuityMeterStatusDisplayName)
values (1, 'ReportingNormally', 'Reporting Normally'),
	(2, 'AlwaysOn', 'Always On'),
	(3, 'AlwaysOff', 'Always Off')

alter table dbo.Sensor
add ContinuityMeterStatusID int null constraint FK_Sensor_ContinuityMeterStatus_ContinuityMeterStatusID foreign key references dbo.ContinuityMeterStatus(ContinuityMeterStatusID),
	ContinuityMeterStatusLastUpdated datetime null