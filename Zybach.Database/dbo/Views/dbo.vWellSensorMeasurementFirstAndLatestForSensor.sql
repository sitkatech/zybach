Create View dbo.vWellSensorMeasurementFirstAndLatestForSensor
as

select a.SensorName, a.MeasurementTypeID, a.FirstReadingDate, a.LastReadingDate, wsm.MeasurementValue as LatestMeasurementValue
from 
(
    select SensorName, MeasurementTypeID, min(cast(concat(ReadingMonth, '/', ReadingDay, '/', ReadingYear) as datetime)) as FirstReadingDate, max(cast(concat(ReadingMonth, '/', ReadingDay, '/', ReadingYear) as datetime)) as LastReadingDate
    from dbo.WellSensorMeasurement
    where MeasurementTypeID != 5 -- don't include BatteryVoltage; that is it's own call
    group by SensorName, MeasurementTypeID
) a
join dbo.WellSensorMeasurement wsm on a.SensorName = wsm.SensorName and a.MeasurementTypeID = wsm.MeasurementTypeID and a.LastReadingDate = cast(concat(wsm.ReadingMonth, '/', wsm.ReadingDay, '/', wsm.ReadingYear) as datetime)

go