create procedure dbo.pPublishWellSensorMeasurementStaging
as
begin

	-- we need to group by and sum because of case sensitive issues with influx data
	select wsms.WellRegistrationID, wsms.MeasurementTypeID, wsms.ReadingYear, wsms.ReadingMonth, wsms.ReadingDay, wsms.SensorName, wsms.IsElectricSource, sum(wsms.MeasurementValue) as MeasurementValue
	into #wms
	from dbo.WellSensorMeasurementStaging wsms
	group by wsms.WellRegistrationID, wsms.MeasurementTypeID, wsms.ReadingYear, wsms.ReadingMonth, wsms.ReadingDay, wsms.SensorName, wsms.IsElectricSource

	insert into dbo.WellSensorMeasurement(WellRegistrationID, MeasurementTypeID, ReadingYear, ReadingMonth, ReadingDay, MeasurementValue, SensorName, IsElectricSource)
	select wsms.WellRegistrationID, wsms.MeasurementTypeID, wsms.ReadingYear, wsms.ReadingMonth, wsms.ReadingDay, wsms.MeasurementValue, wsms.SensorName, wsms.IsElectricSource
	from #wms wsms
	left join dbo.WellSensorMeasurement wsm 
		on wsms.WellRegistrationID = wsm.WellRegistrationID 
		and wsms.MeasurementTypeID = wsm.MeasurementTypeID
		and wsms.ReadingYear = wsm.ReadingYear
		and wsms.ReadingMonth = wsm.ReadingMonth
		and wsms.ReadingDay = wsm.ReadingDay
		and wsms.SensorName = wsm.SensorName
	where wsm.WellSensorMeasurementID is null

	update wsm
	set wsm.MeasurementValue = wsms.MeasurementValue, wsm.IsElectricSource = wsms.IsElectricSource	
	from dbo.WellSensorMeasurement wsm
	join #wms wsms 
		on wsm.WellRegistrationID = wsms.WellRegistrationID 
		and wsm.MeasurementTypeID = wsms.MeasurementTypeID
		and wsm.ReadingYear = wsms.ReadingYear
		and wsm.ReadingMonth = wsms.ReadingMonth
		and wsm.ReadingDay = wsms.ReadingDay
		and wsm.SensorName = wsms.SensorName
end

GO