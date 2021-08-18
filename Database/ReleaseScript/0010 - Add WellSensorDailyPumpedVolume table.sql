create table dbo.MeasurementType
(
	MeasurementTypeID int not null constraint PK_MeasurementType_MeasurementTypeID primary key,
	MeasurementTypeName varchar(100) not null constraint AK_MeasurementType_MeasurementTypeName unique,
	MeasurementTypeDisplayName varchar(100) not null constraint AK_MeasurementType_MeasurementTypeDisplayName unique
)

create table WellSensorMeasurement
(
	WellSensorMeasurementID int not null identity(1,1) constraint PK_WellSensorMeasurement_WellSensorMeasurementID primary key,
	WellRegistrationID varchar(100) not null,
	MeasurementTypeID int not null constraint FK_WellSensorMeasurement_MeasurementType_MeasurementTypeID foreign key references dbo.MeasurementType(MeasurementTypeID),
	ReadingDate datetime not null,
	SensorName varchar(100) not null,
	MeasurementValue float not null
)