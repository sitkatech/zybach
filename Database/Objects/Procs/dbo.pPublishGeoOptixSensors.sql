IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pPublishGeoOptixSensors'))
    drop procedure dbo.pPublishGeoOptixSensors
go

create procedure dbo.pPublishGeoOptixSensors
as
begin
	declare @fetchDate datetime
	set @fetchDate = GETUTCDATE()

	update s
	set InGeoOptix = 0
	from dbo.Sensor s
	left join dbo.GeoOptixSensorStaging aws on s.SensorName = aws.SensorName
	where aws.GeoOptixSensorStagingID is null

	insert into dbo.Sensor(SensorName, SensorTypeID, CreateDate, LastUpdateDate, InGeoOptix, IsActive)
	select	upper(ltrim(rtrim(aws.SensorName))) as SensorName, 
			st.SensorTypeID,
			@fetchDate as CreateDate,
			@fetchDate as LastUpdateDate,
			1 as InGeoOptix,
			1 as IsActive
	from dbo.GeoOptixSensorStaging aws
	left join dbo.Sensor aw on upper(ltrim(rtrim(aws.SensorName))) = aw.SensorName
	join dbo.SensorType st on case when aws.SensorType  = 'PumpMonitor' then 'ContinuityMeter' else aws.SensorType end = st.SensorTypeName
	where aw.SensorID is null

	-- set WellID to null for all Sensors
	update dbo.Sensor
	set LastUpdateDate = @fetchDate, WellID = null

	-- reset the WellID this sensor is mapped to
	update s
	set s.WellID = w.WellID, InGeoOptix = 1
	from dbo.Sensor s
	join dbo.GeoOptixSensorStaging gs on s.SensorName = gs.SensorName
	left join dbo.Well w on gs.WellRegistrationID = w.WellRegistrationID

end

GO