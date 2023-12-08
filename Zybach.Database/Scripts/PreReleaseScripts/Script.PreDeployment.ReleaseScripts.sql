/*
Pre-Deployment Script
--------------------------------------------------------------------------------------
This file is generated on every build, DO NOT modify.
--------------------------------------------------------------------------------------
*/

PRINT N'Zybach.Database - Script.PreDeployment.ReleaseScripts.sql';
GO

:r ".\0001 - store WaterAccount address in temp table.sql"
GO
:r ".\0002 - copy out zone groups.sql"
GO
:r ".\0003 - copy out tables related to allocation slugs.sql"
GO
:r ".\0004 - copy out ParcelUsage and WaterUseType records.sql"
GO
:r ".\0005 - remove baseline scenario.sql"
GO
:r ".\0006 - Remove duplicate ReportingPeriods for each Geography.sql"
GO

