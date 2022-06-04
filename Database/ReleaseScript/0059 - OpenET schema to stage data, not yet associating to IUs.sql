create table dbo.OpenETGoogleBucketResponseEvapotranspirationDatum (
	OpenETGoogleBucketResponseEvapotranspirationDatumID int not null identity(1,1) constraint PK_OpenETGoogleBucketResponseEvapotranspirationDatum_OpenETGoogleBucketResponseEvapotranspirationDatumID primary key,
	WellTPID varchar(100) not null,
	WaterMonth int not null,
	WaterYear int not null,
	EvapotranspirationRateInches decimal(20,4) null
)

insert into dbo.CustomRichTextType(CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
values (23, 'OpenETIntegration', 'OpenET Integration')

insert into dbo.CustomRichText(CustomRichTextTypeID, CustomRichTextContent)
values (23, 'Default text for OpenET Integration')

create table dbo.OpenETSyncResultType (
	OpenETSyncResultTypeID int not null constraint PK_OpenETSyncResultType_OpenETSyncResultTypeID primary key,
	OpenETSyncResultTypeName varchar(100) not null constraint AK_OpenETSyncResultType_AK_OpenETSyncResultTypeName unique,
	OpenETSyncResultTypeDisplayName varchar(100) not null constraint AK_OpenETSyncResultType_OpenETSyncResultTypeDisplayName unique
)

insert into dbo.OpenETSyncResultType (OpenETSyncResultTypeID, OpenETSyncResultTypeName, OpenETSyncResultTypeDisplayName)
values (1, 'InProgress', 'In Progress'),
(2, 'Succeeded', 'Succeeded'),
(3, 'Failed', 'Failed'),
(4, 'NoNewData', 'No New Data'),
(5, 'DataNotAvailable', 'Data Not Available'),
(6, 'Created', 'Created')

create table dbo.WaterYearMonth (
	WaterYearMonthID int identity(1,1) not null constraint PK_WaterYearMonth_WaterYearMonthID primary key,
	[Year] int not null,
	[Month] int not null,
	FinalizeDate datetime null,
	constraint AK_WaterYearMonth_Year_Month unique ([Year], [Month])
)

CREATE TABLE dbo.OpenETSyncHistory(
	OpenETSyncHistoryID int IDENTITY(1,1) NOT NULL constraint PK_OpenETSyncHistory_OpenETSyncHistoryID primary key,
	WaterYearMonthID int NOT NULL constraint FK_OpenETSyncHistory_WaterYearMonth_WaterYearMonthID foreign key references dbo.WaterYearMonth (WaterYearMonthID),
	OpenETSyncResultTypeID int NOT NULL constraint FK_OpenETSyncHistory_OpenETSyncResultType_OpenETSyncResultTypeID foreign key references dbo.OpenETSyncResultType (OpenETSyncResultTypeID),
	CreateDate datetime NOT NULL,
	UpdateDate datetime NOT NULL,
	GoogleBucketFileRetrievalURL varchar(200) NULL,
	ErrorMessage varchar(max) NULL
)