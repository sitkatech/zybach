/*
Post-Deployment Script
--------------------------------------------------------------------------------------
This file is generated on every build, DO NOT modify.
--------------------------------------------------------------------------------------
*/

PRINT N'Zybach.Database - Script.PostDeployment.ReleaseScripts.sql';
GO

:r ".\0001 - Initial data script.sql"
GO
:r ".\0002 - Create geographies.sql"
GO
:r ".\0004 - Adding parcel tagging RTEs.sql"
GO
:r ".\0005 - Add water accounts RTE.sql"
GO
:r ".\0006 - Import WaterType data.sql"
GO
:r ".\0007 - Add water transaction RTEs.sql"
GO
:r ".\0008 - Add platform field definitions.sql"
GO
:r ".\0009 - Add update parcels field definitions.sql"
GO
:r ".\0011 - Add RTE for footers.sql"
GO
:r ".\0012 - Add RTE for edit accounts and edit users.sql"
GO
:r ".\0013 - Update Accounts to include verification key codes and owner address.sql"
GO
:r ".\0014 - Drop VerificationKeys, UserVerificationKeys and UserParcels.sql"
GO
:r ".\0015 - Add RTE for Water dashboard.sql"
GO
:r ".\0016 - Update row to ParcelLayerGDBCommonMappingToParcelStagingColumn.sql"
GO
:r ".\0017 - update geography regex patterns.sql"
GO
:r ".\0018 - update geography regex pattern for pajaro.sql"
GO
:r ".\0019 - Add RTE for change parcel owner.sql"
GO
:r ".\0020 - example users.sql"
GO
:r ".\0021 - adding MIUGSA OpenET shape file.sql"
GO
:r ".\0022 - Add row to WellUploadGDBColumn.sql"
GO
:r ".\0023 - Add RTE for well bulk upload.sql"
GO
:r ".\0024 - update coordinate system for geography.sql"
GO
:r ".\0025 - Add RTE for wells grid.sql"
GO
:r ".\0026 - Add scenarios to models.sql"
GO
:r ".\0027 - Add RTE and Field Defs for water usage CSV upload.sql"
GO
:r ".\0028 - Populate ReportedValueInAcreFeet on ParcelUsage table.sql"
GO
:r ".\0029 - Add RTE for wells index.sql"
GO
:r ".\0030 - Populate UnitType on ParcelUsage table.sql"
GO
:r ".\0031 - Add RTE for external map layer.sql"
GO
:r ".\0032 - Add MSGSA as new Geography.sql"
GO
:r ".\0033 - Add RTE for our geographies.sql"
GO
:r ".\0034 - Add RTE for pop-up field.sql"
GO
:r ".\0035 - Add RTE for OpenET.sql"
GO
:r ".\0036 - Add RTE for Usage Estimates page.sql"
GO
:r ".\0037 - Add RTE for OpenET grid.sql"
GO
:r ".\0038 - Remove OpenET shapefile from MSGSA.sql"
GO
:r ".\0039 - Add RTE for Etimate Date.sql"
GO
:r ".\0040 - Change EDF to Demo.sql"
GO
:r ".\0041 - Concat Owner address together.sql"
GO
:r ".\0042 - adding an OpenET water use type.sql"
GO
:r ".\0043 - Add RTE for Zone Group Edit.sql"
GO
:r ".\0044 - Add RTE for Zone Group Configuration.sql"
GO
:r ".\0045 - Add RTE for Zone Group CSV Uploader.sql"
GO
:r ".\0046 - Add RTE for Zone Group Index Page.sql"
GO
:r ".\0047 - Adjust Zone Group Slug.sql"
GO
:r ".\0048 - Add RTEs for home page.sql"
GO
:r ".\0049 - Add RTEs for Water Account Budget Report.sql"
GO
:r ".\0050 - Remove quarterly reporting period option.sql"
GO
:r ".\0051 - Add RTE for Custom Page Edit Properties.sql"
GO
:r ".\0052 - Remove View MenuItem.sql"
GO
:r ".\0053 - Add RTE for Zone Group Usage Chart.sql"
GO
:r ".\0054 - Remove duplicate Water Account.sql"
GO
:r ".\0055 - Set demo geography.sql"
GO
:r ".\0056 - Adjust EndDate for WaterAccountParcel.sql"
GO
:r ".\0057 - migrate contact information for water accounts.sql"
GO
:r ".\0058 - RTEs for Modals.sql"
GO
:r ".\0059 - demo water manager keycode.sql"
GO
:r ".\0060 - RTEs field definitions.sql"
GO
:r ".\0061 - Populate OwnerAddress in Parcels.sql"
GO
:r ".\0062 - RTEs for Geography Dashboard.sql"
GO
:r ".\0063 - Populate GSACanonicalID column on Geography table.sql"
GO
:r ".\0064 - RTE for Monitoring Wells Grid.sql"
GO
:r ".\0065 - RTEs for Water Account Dashboard.sql"
GO
:r ".\0066 - add colors to Geographies.sql"
GO
:r ".\0067 - update RTEs that are geography specific.sql"
GO
:r ".\0068 - water type slugs.sql"
GO
:r ".\0069 - Populate Merced ModelBoundary.sql"
GO
:r ".\0070 - Add new Merced ModelScenario.sql"
GO
:r ".\0071 - Add a Well Scenario RTE.sql"
GO
:r ".\0072 - Add RTE for Allocation Plan Edit.sql"
GO
:r ".\0073 - Add RTEs for Configure Allocaiton and Copy Allocation Plan.sql"
GO
:r ".\0074 - Add RTE for Account Allocation Plans.sql"
GO
:r ".\0075 - Populate kern ModelBoundary.sql"
GO
:r ".\0076 - Add new kern ModelScenario.sql"
GO
:r ".\0076 - remove old model runs.sql"
GO
:r ".\0077 - populate WaterMeasurement and WaterMeasurementType records.sql"
GO
:r ".\0078 - update OpenET WaterMeasurementType records.sql"
GO
:r ".\0079 - manually create OpenETSync precip records.sql"
GO
:r ".\0080 - add model scenarios for recharge.sql"
GO
:r ".\0081 - populate Geography SourceOfRecordWaterMeasurementTypeID.sql"
GO
:r ".\0082 - Add PostToLedger RTE and OpenETSyncVariable field def.sql"
GO
:r ".\0083 - Add RTE for Landowner Parcel Index.sql"
GO
:r ".\0084 - fix modelscenarios to match GET scenarios.sql"
GO
:r ".\0085 - Add RTE for scenario planner.sql"
GO
:r ".\0086 - Add RTE for scenario planner runs.sql"
GO
:r ".\0087 - update to model scenario to match get environments.sql"
GO
:r ".\0088 - deleting existing Parcel data.sql"
GO
:r ".\0089 - Add new RTEs  for Activity Center and Water Account Suggestions.sql"
GO
:r ".\0090 - Add new Geography Water Level RTEs.sql"
GO
:r ".\0091 - Add new Geography Water Level RTEs.sql"
GO

