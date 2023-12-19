create procedure dbo.pTruncateWellSensorMeasurementStaging
with execute as owner
as
begin
	TRUNCATE TABLE dbo.WellSensorMeasurementStaging
end

GO
