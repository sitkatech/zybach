exec sp_rename 'dbo.AgHubWellStaging.LandOwnerName', 'AgHubRegisteredUser', 'COLUMN'
exec sp_rename 'dbo.Well.LandOwnerName', 'AgHubRegisteredUser', 'COLUMN'
