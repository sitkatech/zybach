if exists (select * from dbo.sysobjects where id = object_id('dbo.vSensorLatestBatteryVoltage'))
	drop view dbo.vSensorLatestBatteryVoltage
go

Create View dbo.vSensorLatestBatteryVoltage
as

select a.SensorName, cast(concat(a.ReadingMonth, '/', a.ReadingDay, '/', a.ReadingYear) as datetime) as LastVoltageReadingDate, a.MeasurementValue as LastVoltageReading
from 
(
    select WellSensorMeasurementID, WellRegistrationID, MeasurementTypeID, ReadingYear, ReadingMonth, ReadingDay, SensorName, MeasurementValue, IsAnomalous, IsElectricSource, row_number() over (partition by SensorName order by cast(concat(ReadingMonth, '/', ReadingDay, '/', ReadingYear) as datetime) desc) as Ranking
    from dbo.WellSensorMeasurement
    where MeasurementTypeID = 5
) a
where Ranking = 1

go