EXEC sp_rename 'dbo.AgHubIrrigationUnitWaterYearMonthETDatum.EvapotranspirationRateInches', 'EvapotranspirationInches', 'COLUMN';
EXEC sp_rename 'dbo.OpenETGoogleBucketResponseEvapotranspirationDatum.EvapotranspirationRateInches', 'EvapotranspirationInches', 'COLUMN';

ALTER TABLE dbo.OpenETGoogleBucketResponseEvapotranspirationDatum
drop column EvapotranspirationRateAcreFeet

ALTER TABLE dbo.OpenETGoogleBucketResponsePrecipitationDatum
drop column PrecipitationAcreFeet

ALTER TABLE dbo.AgHubIrrigationUnitWaterYearMonthETDatum
drop column EvapotranspirationRateAcreFeet

ALTER TABLE dbo.AgHubIrrigationUnitWaterYearMonthETDatum
add EvapotranspirationAcreInches decimal (20, 4) null

ALTER TABLE dbo.AgHubIrrigationUnitWaterYearMonthPrecipitationDatum
drop column PrecipitationAcreFeet

ALTER TABLE dbo.AgHubIrrigationUnitWaterYearMonthPrecipitationDatum
add PrecipitationAcreInches decimal (20,4) null

go

--populate new columns with correct acre-inch amounts based off of AgHub irrigation unit areas

UPDATE ahiuwymed
SET ahiuwymed.EvapotranspirationAcreInches = e.EvapotranspirationInches * ahiu.IrrigationUnitAreaInAcres
FROM dbo.AgHubIrrigationUnitWaterYearMonthETDatum ahiuwymed
INNER JOIN dbo.AgHubIrrigationUnitWaterYearMonthETDatum e
on ahiuwymed.AgHubIrrigationUnitWaterYearMonthETDatumID = e.AgHubIrrigationUnitWaterYearMonthETDatumID
JOIN AgHubIrrigationUnit ahiu on ahiu.AgHubIrrigationUnitID = ahiuwymed.AgHubIrrigationUnitID

UPDATE ahiuwympd
SET ahiuwympd.PrecipitationAcreInches = p.PrecipitationInches * ahiu.IrrigationUnitAreaInAcres
FROM dbo.AgHubIrrigationUnitWaterYearMonthPrecipitationDatum ahiuwympd
INNER JOIN dbo.AgHubIrrigationUnitWaterYearMonthPrecipitationDatum p
on ahiuwympd.AgHubIrrigationUnitWaterYearMonthPrecipitationDatumID = p.AgHubIrrigationUnitWaterYearMonthPrecipitationDatumID
JOIN AgHubIrrigationUnit ahiu on ahiu.AgHubIrrigationUnitID = ahiuwympd.AgHubIrrigationUnitID

