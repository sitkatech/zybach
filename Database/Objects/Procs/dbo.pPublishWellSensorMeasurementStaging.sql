IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pPublishWellSensorMeasurementStaging'))
    drop procedure dbo.pPublishWellSensorMeasurementStaging
go

create procedure dbo.pPublishWellSensorMeasurementStaging
as
begin

	-- we need to group by and sum because of case sensitive issues with influx data
	select wsms.WellRegistrationID, wsms.MeasurementTypeID, wsms.ReadingYear, wsms.ReadingMonth, wsms.ReadingDay, wsms.SensorName, sum(wsms.MeasurementValue) as MeasurementValue
	into #wms
	from dbo.WellSensorMeasurementStaging wsms
	group by wsms.WellRegistrationID, wsms.MeasurementTypeID, wsms.ReadingYear, wsms.ReadingMonth, wsms.ReadingDay, wsms.SensorName

	insert into dbo.WellSensorMeasurement(WellRegistrationID, MeasurementTypeID, ReadingYear, ReadingMonth, ReadingDay, MeasurementValue, SensorName)
	select wsms.WellRegistrationID, wsms.MeasurementTypeID, wsms.ReadingYear, wsms.ReadingMonth, wsms.ReadingDay, wsms.MeasurementValue, wsms.SensorName
	from #wms wsms
	left join dbo.WellSensorMeasurement wsm 
		on wsms.WellRegistrationID = wsm.WellRegistrationID 
		and wsms.MeasurementTypeID = wsm.MeasurementTypeID
		and wsms.ReadingYear = wsm.ReadingYear
		and wsms.ReadingMonth = wsm.ReadingMonth
		and wsms.ReadingDay = wsm.ReadingDay
		and isnull(wsms.SensorName, '') = isnull(wsm.SensorName, '')
	where wsm.WellSensorMeasurementID is null

	update wsm
	set wsm.MeasurementValue = wsms.MeasurementValue		
	from dbo.WellSensorMeasurement wsm
	join #wms wsms 
		on wsm.WellRegistrationID = wsms.WellRegistrationID 
		and wsm.MeasurementTypeID = wsms.MeasurementTypeID
		and wsm.ReadingYear = wsms.ReadingYear
		and wsm.ReadingMonth = wsms.ReadingMonth
		and wsm.ReadingDay = wsms.ReadingDay
		and isnull(wsm.SensorName, '') = isnull(wsms.SensorName, '')
end

GO