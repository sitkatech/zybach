create table dbo.ChemigationPermitAnnualRecordStatus
(
	ChemigationPermitAnnualRecordStatusID int not null identity (1,1) constraint PK_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusID primary key,
	ChemigationPermitAnnualRecordStatusName varchar(50) not null constraint AK_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusName unique,
	ChemigationPermitAnnualRecordStatusDisplayName varchar(50) not null constraint AK_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusDisplayName unique
)

create table dbo.ChemigationInjectionUnitType
(
	ChemigationInjectionUnitTypeID int not null identity (1,1) constraint PK_ChemigationInjectionUnitType_ChemigationInjectionUnitTypeID primary key,
	ChemigationInjectionUnitTypeName varchar(50) not null constraint AK_ChemigationInjectionUnitType_ChemigationInjectionUnitTypeName unique,
	ChemigationInjectionUnitTypeDisplayName varchar(50) not null constraint AK_ChemigationInjectionUnitType_ChemigationInjectionUnitTypeDisplayName unique
)

create table dbo.ChemigationPermitAnnualRecord
(
	ChemigationPermitAnnualRecordID int not null identity (1,1) constraint PK_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID primary key,
	ChemigationPermitID int not null constraint FK_ChemigationPermitAnnualRecord_ChemigationPermit_ChemigationPermitID foreign key 
		references dbo.ChemigationPermit(ChemigationPermitID),
	RecordYear int not null,
	ChemigationPermitAnnualRecordStatusID int not null constraint FK_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusID foreign key
		references dbo.ChemigationPermitAnnualRecordStatus(ChemigationPermitAnnualRecordStatusID),
	PivotName varchar(100) not null,
	ChemigationInjectionUnitTypeID int not null constraint FK_ChemigationPermitAnnualRecord_ChemigationInjectionUnitType_ChemigationInjectionUnitTypeID foreign key
		references dbo.ChemigationInjectionUnitType(ChemigationInjectionUnitTypeID),
	ApplicantFirstName varchar(100) not null,
	ApplicantLastName varchar(100) not null,
	ApplicantMailingAddress varchar(100) not null,
	ApplicantCity varchar(50) not null,
	ApplicantState varchar(10) not null,
	ApplicantZipCode int not null,
	ApplicantPhone varchar(30) null,
	ApplicantMobilePhone varchar(30) null,
	DateReceived datetime null,
	DatePaid datetime null,
	constraint AK_ChemigationPermitAnnualRecord_ChemigationPermitID_RecordYear unique (ChemigationPermitID, RecordYear)
)

GO

INSERT INTO dbo.ChemigationPermitAnnualRecordStatus(ChemigationPermitAnnualRecordStatusName, ChemigationPermitAnnualRecordStatusDisplayName)
VALUES
('PendingPayment', 'Pending Payment'),
('ReadyForReview', 'Ready For Review'),
('PendingInspection', 'Pending Inspection'),
('Approved', 'Approved')

INSERT INTO dbo.ChemigationInjectionUnitType(ChemigationInjectionUnitTypeName, ChemigationInjectionUnitTypeDisplayName)
VALUES
('Portable', 'Portable'),
('Stationary', 'Stationary')