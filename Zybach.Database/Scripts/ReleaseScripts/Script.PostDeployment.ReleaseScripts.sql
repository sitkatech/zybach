/*
Post-Deployment Script
--------------------------------------------------------------------------------------
This file is generated on every build, DO NOT modify.
--------------------------------------------------------------------------------------
*/

PRINT N'Zybach.Database - Script.PostDeployment.ReleaseScripts.sql';
GO

:r ".\0001 - Run MakeValid__ on AgHubIrrigationUnit geometries.sql"
GO
:r ".\0002 - Prepopulate prism monthly sync records.sql"
GO

