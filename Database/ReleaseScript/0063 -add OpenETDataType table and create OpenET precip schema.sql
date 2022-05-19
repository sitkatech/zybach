create table dbo.OpenETDataType (
	OpenETDataTypeID int not null constraint PK_OpenETDataType_OpenETDataTypeID primary key,
	OpenETDataTypeName varchar(50) not null,
	OpenETDataTypeDisplayName varchar(100) not null,
	OpenETDataTypeVariableName varchar(20) not null
)

insert into dbo.OpenETDataType (OpenETDataTypeID, OpenETDataTypeName, OpenETDataTypeDisplayName, OpenETDataTypeVariableName) values
(1, 'Evapotranspiration', 'Evapotranspiration', 'et'),
(2, 'Precipitation', 'Precipitation', 'pr')

alter table dbo.OpenETSyncHistory
add 
OpenETDataTypeID int null constraint FK_OpenETSyncHistory_OpenETDataType_OpenETDataTypeID references dbo.OpenETDataType(OpenETDataTypeID)

alter table dbo.OpenETGoogleBucketResponseEvapotranspirationDatum
add 
EvapotranspirationRateAcreFeet decimal (20,4) null

create table dbo.OpenETGoogleBucketResponsePrecipitationDatum (
	OpenETGoogleBucketResponsePrecipitationDatumID int not null identity(1,1) constraint PK_OpenETGoogleBucketResponsePrecipitationDatum_OpenETGoogleBucketResponsePrecipitationDatumID primary key,
	WellTPID varchar(100) not null,
	WaterMonth int not null,
	WaterYear int not null,
	PrecipitationAcreFeet decimal (20,4) null,
	PrecipitationInches decimal (20,4) null
)

alter table dbo.AgHubIrrigationUnitWaterYearMonthETDatum
add
PrecipitationAcreFeet decimal (20,4) null,
PrecipitationInches decimal (20,4) null