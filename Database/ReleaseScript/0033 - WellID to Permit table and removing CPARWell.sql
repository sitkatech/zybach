alter table dbo.ChemigationPermit add WellID int null constraint FK_ChemigationPermit_Well_WellID foreign key references dbo.Well(WellID)
alter table dbo.ChemigationPermitAnnualRecord add TownshipRangeSection varchar(100) not null
alter table dbo.ChemigationPermit drop column TownshipRangeSection
alter table dbo.ChemigationPermitAnnualRecord add ApplicantCompany varchar(200) null
alter table dbo.ChemigationPermitAnnualRecord add AnnualNotes varchar(500) null
alter table dbo.ChemigationPermitAnnualRecord add DateApproved datetime null

drop table dbo.ChemigationPermitAnnualRecordWell

alter table dbo.ChemigationPermitAnnualRecord alter column PivotName varchar(100) null
alter table dbo.ChemigationPermitAnnualRecord alter column ApplicantLastName varchar(200) null
alter table dbo.ChemigationPermitAnnualRecord alter column ApplicantFirstName varchar(200) null
alter table dbo.ChemigationPermitAnnualRecord alter column ApplicantMailingAddress varchar(100) null
alter table dbo.ChemigationPermitAnnualRecord alter column ApplicantCity varchar(50) null
alter table dbo.ChemigationPermitAnnualRecord alter column ApplicantState varchar(20) null
alter table dbo.ChemigationPermitAnnualRecord alter column ApplicantZipCode varchar(10) null
alter table dbo.ChemigationPermitAnnualRecordApplicator alter column CertificationNumber int null
alter table dbo.ChemigationPermitAnnualRecordApplicator alter column ExpirationYear int null
alter table dbo.ChemigationPermitAnnualRecord drop constraint FK_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusID
alter table dbo.ChemigationPermitAnnualRecord drop constraint FK_ChemigationPermitAnnualRecord_ChemigationInjectionUnitType_ChemigationInjectionUnitTypeID
drop table dbo.ChemigationPermitAnnualRecordStatus
drop table dbo.ChemigationInjectionUnitType
GO



create table dbo.ChemigationPermitAnnualRecordStatus
(
	ChemigationPermitAnnualRecordStatusID int not null constraint PK_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusID primary key,
	ChemigationPermitAnnualRecordStatusName varchar(50) not null constraint AK_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusName unique,
	ChemigationPermitAnnualRecordStatusDisplayName varchar(50) not null constraint AK_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusDisplayName unique
)

create table dbo.ChemigationInjectionUnitType
(
	ChemigationInjectionUnitTypeID int not null constraint PK_ChemigationInjectionUnitType_ChemigationInjectionUnitTypeID primary key,
	ChemigationInjectionUnitTypeName varchar(50) not null constraint AK_ChemigationInjectionUnitType_ChemigationInjectionUnitTypeName unique,
	ChemigationInjectionUnitTypeDisplayName varchar(50) not null constraint AK_ChemigationInjectionUnitType_ChemigationInjectionUnitTypeDisplayName unique
)

create table dbo.ChemigationPermitAnnualRecordFeeType
(
	ChemigationPermitAnnualRecordFeeTypeID int not null constraint PK_ChemigationPermitAnnualRecordFeeType_ChemigationPermitAnnualRecordFeeTypeID primary key,
	ChemigationPermitAnnualRecordFeeTypeName varchar(50) not null constraint AK_ChemigationPermitAnnualRecordFeeType_ChemigationPermitAnnualRecordFeeTypeName unique,
	ChemigationPermitAnnualRecordFeeTypeDisplayName varchar(50) not null constraint AK_ChemigationPermitAnnualRecordFeeType_ChemigationPermitAnnualRecordFeeTypeDisplayName unique,
	FeeAmount money not null
)


alter table dbo.ChemigationPermitAnnualRecord add constraint FK_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusID foreign key (ChemigationPermitAnnualRecordStatusID) references dbo.ChemigationPermitAnnualRecordStatus(ChemigationPermitAnnualRecordStatusID)
alter table dbo.ChemigationPermitAnnualRecord add constraint FK_ChemigationPermitAnnualRecord_ChemigationInjectionUnitType_ChemigationInjectionUnitTypeID foreign key (ChemigationInjectionUnitTypeID) references dbo.ChemigationInjectionUnitType(ChemigationInjectionUnitTypeID)
alter table dbo.ChemigationPermitAnnualRecord add ChemigationPermitAnnualRecordFeeTypeID int null constraint FK_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordFeeType_ChemigationPermitAnnualRecordFeeTypeID foreign key (ChemigationPermitAnnualRecordFeeTypeID) references dbo.ChemigationPermitAnnualRecordFeeType(ChemigationPermitAnnualRecordFeeTypeID)
GO

INSERT INTO dbo.ChemigationPermitAnnualRecordStatus(ChemigationPermitAnnualRecordStatusID, ChemigationPermitAnnualRecordStatusName, ChemigationPermitAnnualRecordStatusDisplayName)
VALUES
(1, 'PendingRenewal', 'Pending Renewal'),
(2, 'ReadyForReview', 'Ready For Review'),
(3, 'PendingInspection', 'Pending Inspection'),
(4, 'Approved', 'Approved')

INSERT INTO dbo.ChemigationInjectionUnitType(ChemigationInjectionUnitTypeID, ChemigationInjectionUnitTypeName, ChemigationInjectionUnitTypeDisplayName)
VALUES
(1, 'Portable', 'Portable'),
(2, 'Stationary', 'Stationary')


INSERT INTO dbo.ChemigationPermitAnnualRecordStatus(ChemigationPermitAnnualRecordStatusID, ChemigationPermitAnnualRecordStatusName, ChemigationPermitAnnualRecordStatusDisplayName)
VALUES
(5, 'Canceled', 'Canceled')

INSERT INTO dbo.ChemigationPermitAnnualRecordFeeType(ChemigationPermitAnnualRecordFeeTypeID, ChemigationPermitAnnualRecordFeeTypeName, ChemigationPermitAnnualRecordFeeTypeDisplayName, FeeAmount)
VALUES
(1, 'New', 'New ($40)', 40),
(2, 'Renewal', 'Renewal ($20)', 20),
(3, 'Emergency', 'Emergency ($100)', 100)
