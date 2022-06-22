insert into dbo.Sensor(SensorName, SensorTypeID, CreateDate, LastUpdateDate, InGeoOptix, IsActive, WellID)
select	concat('E-', upper(w.WellRegistrationID)) as SensorName, 
		4 as SensorTypeID,
		getutcdate() as CreateDate,
		getutcdate() as LastUpdateDate,
		0 as InGeoOptix,
		1 as IsActive,
        w.WellID
from dbo.AgHubWell aw
join dbo.Well w on aw.WellID = w.WellID
left join dbo.Sensor s on concat('E-', upper(w.WellRegistrationID)) = s.SensorName
where aw.WellConnectedMeter = 1 and s.SensorID is null

update wsm
set wsm.SensorName = concat('E-', upper(wsm.WellRegistrationID))
from dbo.WellSensorMeasurement wsm
where wsm.SensorName is null

update wsm
set wsm.SensorName = concat('E-', upper(wsm.WellRegistrationID))
from dbo.WellSensorMeasurementStaging wsm
where wsm.SensorName is null

ALTER TABLE dbo.WellSensorMeasurement drop constraint AK_WellSensorMeasurement_WellRegistrationID_MeasurementTypeID_SensorName_ReadingDate
ALTER TABLE dbo.Sensor DROP CONSTRAINT FK_Sensor_SensorType_SensorTypeID
GO

alter table dbo.WellSensorMeasurementStaging alter column SensorName varchar(100) not null
alter table dbo.WellSensorMeasurement alter column SensorName varchar(100) not null


ALTER TABLE dbo.WellSensorMeasurement add constraint AK_WellSensorMeasurement_WellRegistrationID_MeasurementTypeID_SensorName_ReadingDate UNIQUE 
(
	WellRegistrationID,
	MeasurementTypeID,
	SensorName,
	ReadingYear,
	ReadingMonth,
	ReadingDay
)

alter table dbo.Sensor alter column SensorTypeID int not null 
GO

alter table dbo.Sensor add CONSTRAINT FK_Sensor_SensorType_SensorTypeID FOREIGN KEY (SensorTypeID) REFERENCES dbo.SensorType (SensorTypeID)
