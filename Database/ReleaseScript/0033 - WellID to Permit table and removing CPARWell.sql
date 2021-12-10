alter table dbo.ChemigationPermit add WellID int null constraint FK_ChemigationPermit_Well_WellID foreign key references dbo.Well(WellID)
drop table dbo.ChemigationPermitAnnualRecordWell

alter table dbo.ChemigationPermitAnnualRecord alter column PivotName varchar(100) null
alter table dbo.ChemigationPermitAnnualRecord drop column ApplicantLastName
alter table dbo.ChemigationPermitAnnualRecord alter column ApplicantFirstName varchar(200) null
alter table dbo.ChemigationPermitAnnualRecord alter column ApplicantMailingAddress varchar(100) null
alter table dbo.ChemigationPermitAnnualRecord alter column ApplicantCity varchar(50) null
alter table dbo.ChemigationPermitAnnualRecord alter column ApplicantState varchar(20) null
alter table dbo.ChemigationPermitAnnualRecord alter column ApplicantZipCode varchar(10) null
alter table dbo.ChemigationPermitAnnualRecordApplicator alter column CertificationNumber int null
alter table dbo.ChemigationPermitAnnualRecordApplicator alter column ExpirationYear int null
GO

exec sp_rename 'dbo.ChemigationPermitAnnualRecord.ApplicantFirstName', 'ApplicantName', 'COLUMN'