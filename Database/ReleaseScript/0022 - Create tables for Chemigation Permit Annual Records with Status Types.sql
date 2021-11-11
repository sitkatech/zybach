create table dbo.ChemigationPermitAnnualRecordStatus
(
	ChemigationPermitAnnualRecordStatusID int not null identity (1,1) constraint PK_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusID primary key,
	ChemigationPermitAnnualRecordStatusName varchar(50) not null constraint AK_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusName unique,
	ChemigationPermitAnnualRecordStatusDisplayName varchar(50) not null constraint AK_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusDisplayName unique
)

create table dbo.ChemigationPermitAnnualRecord
(
	ChemigationPermitAnnualRecordID int not null identity (1,1) constraint PK_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID primary key,
	ChemigationPermitID int not null constraint FK_ChemigationPermitAnnualRecord_ChemigationPermit_ChemigationPermitID foreign key 
		references dbo.ChemigationPermit(ChemigationPermitID),
	RecordYear int not null,
	ChemigationPermitAnnualRecordStatusID int not null constraint FK_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordStatus_ChemigationPermitAnnualRecordStatusID foreign key
		references dbo.ChemigationPermitAnnualRecordStatus(ChemigationPermitAnnualRecordStatusID),
	ApplicantFirstName varchar(100) not null,
	ApplicantLastName varchar(100) not null,
	PivotName varchar(100) not null,
	DateReceived datetime null,
	DatePaid datetime null
)

GO

INSERT INTO dbo.ChemigationPermitAnnualRecordStatus(ChemigationPermitAnnualRecordStatusName, ChemigationPermitAnnualRecordStatusDisplayName)
VALUES
('PendingPayment', 'Pending Payment'),
('ReadyForReview', 'Ready For Review'),
('PendingInspection', 'Pending Inspection'),
('Approved', 'Approved')