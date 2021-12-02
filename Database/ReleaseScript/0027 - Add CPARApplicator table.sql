create table dbo.ChemigationPermitAnnualRecordApplicator
(
	ChemigationPermitAnnualRecordApplicatorID int not null identity(1,1) constraint PK_ChemigationPermitAnnualRecordApplicator_ChemigationPermitAnnualRecordApplicatorID primary key,
	ChemigationPermitAnnualRecordID int not null constraint FK_ChemigationPermitAnnualRecordApplicator_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID foreign key references dbo.ChemigationPermitAnnualRecord(ChemigationPermitAnnualRecordID),
	ApplicatorName varchar(100) not null,
	CertificationNumber int not null,
	ExpirationYear int not null,
	HomePhone varchar(30) null,
	MobilePhone varchar(30) null
)
GO
