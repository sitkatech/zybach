CREATE TABLE dbo.GeoOptixWellStaging(
	GeoOptixWellStagingID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_GeoOptixWellStaging_GeoOptixWellStagingID PRIMARY KEY,
	WellRegistrationID varchar(100) NOT NULL,
	WellGeometry geometry NOT NULL
)

CREATE TABLE dbo.GeoOptixSensorStaging(
	GeoOptixSensorStagingID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_GeoOptixSensorStaging_GeoOptixSensorStagingID PRIMARY KEY,
	WellRegistrationID varchar(100) NOT NULL,
	SensorName varchar(100) not null,
	SensorType varchar(100) not null
)
GO

create table dbo.SensorType
(
	SensorTypeID int not null constraint PK_SensorType_SensorTypeID primary key,
	SensorTypeName varchar(100) not null constraint AK_SensorType_SensorTypeName unique,
	SensorTypeDisplayName varchar(100) not null constraint AK_SensorType_SensorTypeDisplayName unique
)

CREATE TABLE dbo.GeoOptixWell(
	GeoOptixWellID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_GeoOptixWell_GeoOptixWellID PRIMARY KEY,
	WellID int not null constraint FK_GeoOptixWell_Well_WellID foreign key references dbo.Well(WellID) constraint AK_GeoOptixWell_WellID unique,
	GeoOptixWellGeometry geometry NOT NULL
)

CREATE TABLE dbo.Sensor(
	SensorID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Sensor_SensorID PRIMARY KEY,
	SensorName varchar(100) not null constraint AK_Sensor_SensorName unique,
	SensorTypeID int null constraint FK_Sensor_SensorType_SensorTypeID foreign key references dbo.SensorType(SensorTypeID),
	WellID int null constraint FK_Sensor_Well_WellID foreign key references dbo.Well(WellID),
	InGeoOptix bit not null,
	CreateDate datetime not null,
	LastUpdateDate datetime not null
)
GO
