if exists (select * from dbo.sysobjects where id = object_id('dbo.vWellSensorMeasurementFirstAndLatestForSensor'))
	drop view dbo.vWellSensorMeasurementFirstAndLatestForSensor
go

Create View dbo.vWellSensorMeasurementFirstAndLatestForSensor
as

select a.SensorName, a.MeasurementTypeID, a.WellRegistrationID, a.FirstReadingDate, a.LastReadingDate, wsm.MeasurementValue as LatestMeasurementValue
from 
(
    select SensorName, MeasurementTypeID, WellRegistrationID, min(cast(concat(ReadingMonth, '/', ReadingDay, '/', ReadingYear) as datetime)) as FirstReadingDate, max(cast(concat(ReadingMonth, '/', ReadingDay, '/', ReadingYear) as datetime)) as LastReadingDate
    from dbo.WellSensorMeasurement
    group by SensorName, MeasurementTypeID, WellRegistrationID
) a
join dbo.WellSensorMeasurement wsm on a.SensorName = wsm.SensorName and a.MeasurementTypeID = wsm.MeasurementTypeID and a.LastReadingDate = cast(concat(wsm.ReadingMonth, '/', wsm.ReadingDay, '/', wsm.ReadingYear) as datetime) and a.WellRegistrationID = wsm.WellRegistrationID

go