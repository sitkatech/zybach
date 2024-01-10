create procedure dbo.pTruncateGeoOptixSensorStaging
with execute as owner
as
begin
	TRUNCATE TABLE dbo.GeoOptixSensorStaging
end

GO
