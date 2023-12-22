create procedure dbo.pTruncateAgHubWellStaging
with execute as owner
as
begin
	TRUNCATE TABLE dbo.AgHubWellStaging
end

GO
