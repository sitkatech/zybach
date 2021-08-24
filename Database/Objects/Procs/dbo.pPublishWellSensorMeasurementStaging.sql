IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pPublishWellSensorMeasurementStaging'))
    drop procedure dbo.pPublishWellSensorMeasurementStaging
go

create procedure dbo.pPublishWellSensorMeasurementStaging
as
begin

	insert into dbo.WellSensorMeasurement(WellRegistrationID, MeasurementTypeID, ReadingDate, MeasurementValue, SensorName)
	select wsms.WellRegistrationID, wsms.MeasurementTypeID, 
	CONVERT(DATETIME, wsms.ReadingDate AT TIME ZONE 'Central Standard Time' AT TIME ZONE 'UTC') as ReadingDate, 
	wsms.MeasurementValue, wsms.SensorName
	from dbo.WellSensorMeasurementStaging wsms
	left join dbo.WellSensorMeasurement wsm 
		on wsms.WellRegistrationID = wsm.WellRegistrationID 
		and wsms.MeasurementTypeID = wsm.MeasurementTypeID
		and wsms.ReadingDate = wsm.ReadingDate 		
		and isnull(wsms.SensorName, '') = isnull(wsm.SensorName, '')
	where wsm.WellSensorMeasurementID is null

	update wsm
	set wsm.MeasurementValue = wsms.MeasurementValue		
	from dbo.WellSensorMeasurement wsm
	join dbo.WellSensorMeasurementStaging wsms 
		on wsm.WellRegistrationID = wsms.WellRegistrationID 
		and wsm.MeasurementTypeID = wsms.MeasurementTypeID
		and wsm.ReadingDate = CONVERT(DATETIME, wsms.ReadingDate AT TIME ZONE 'Central Standard Time' AT TIME ZONE 'UTC') 
		and isnull(wsm.SensorName, '') = isnull(wsms.SensorName, '')
end

GO