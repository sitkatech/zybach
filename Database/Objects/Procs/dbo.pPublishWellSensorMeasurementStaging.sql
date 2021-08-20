IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pPublishWellSensorMeasurementStaging'))
    drop procedure dbo.pPublishWellSensorMeasurementStaging
go

create procedure dbo.pPublishWellSensorMeasurementStaging
(
	@MeasurementTypeID int
)
as
begin

	delete from dbo.WellSensorMeasurement where MeasurementTypeID = @MeasurementTypeID

	insert into dbo.WellSensorMeasurement(WellRegistrationID, MeasurementTypeID, ReadingDate, MeasurementValue, SensorName)
	select WellRegistrationID, MeasurementTypeID, ReadingDate, MeasurementValue, SensorName
	from dbo.WellSensorMeasurementStaging wms
	where wms.MeasurementTypeID = @MeasurementTypeID

end

GO