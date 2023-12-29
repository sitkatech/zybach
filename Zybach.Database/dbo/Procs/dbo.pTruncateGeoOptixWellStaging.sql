create procedure dbo.pTruncateGeoOptixWellStaging
with execute as owner
as
begin
	TRUNCATE TABLE dbo.GeoOptixWellStaging
end

GO
