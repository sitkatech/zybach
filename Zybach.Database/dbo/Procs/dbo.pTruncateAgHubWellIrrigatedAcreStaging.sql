create procedure dbo.pTruncateAgHubWellIrrigatedAcreStaging
with execute as owner
as
begin
	TRUNCATE TABLE dbo.AgHubWellIrrigatedAcreStaging
end

GO
