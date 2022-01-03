IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pChemigationPermitAnnualRecordBulkCreateForRecordYear'))
    drop procedure dbo.pChemigationPermitAnnualRecordBulkCreateForRecordYear
go

create procedure dbo.pChemigationPermitAnnualRecordBulkCreateForRecordYear
(
	@recordYear int,
	@recordsCreated int output
)
as
begin

	drop table if exists #chemigationPermitsToRenew
	select cp.ChemigationPermitID, max(RecordYear) as LatestRecordYear
	into #chemigationPermitsToRenew
	from dbo.ChemigationPermit cp
	join dbo.ChemigationPermitAnnualRecord cpar on cp.ChemigationPermitID = cpar.ChemigationPermitID
	where cp.ChemigationPermitStatusID = 1
	group by cp.ChemigationPermitID having max(RecordYear) = @recordYear - 1

	insert into dbo.ChemigationPermitAnnualRecord(ChemigationPermitID, RecordYear, ChemigationPermitAnnualRecordStatusID, PivotName, ChemigationInjectionUnitTypeID, ApplicantName, ApplicantMailingAddress, ApplicantCity, ApplicantState, ApplicantZipCode, ApplicantPhone, ApplicantMobilePhone, ApplicantEmail, NDEEAmount)
	select cpar.ChemigationPermitID, @recordYear, cpar.ChemigationPermitAnnualRecordStatusID, cpar.PivotName, cpar.ChemigationInjectionUnitTypeID, cpar.ApplicantName, cpar.ApplicantMailingAddress, cpar.ApplicantCity, cpar.ApplicantState, cpar.ApplicantZipCode, cpar.ApplicantPhone, cpar.ApplicantMobilePhone, cpar.ApplicantEmail, 2.0 -- renewal amount
	from dbo.ChemigationPermit cp
	join #chemigationPermitsToRenew a on cp.ChemigationPermitID = a.ChemigationPermitID
	join dbo.ChemigationPermitAnnualRecord cpar on cp.ChemigationPermitID = cpar.ChemigationPermitID and cpar.RecordYear = a.LatestRecordYear

	insert into dbo.ChemigationPermitAnnualRecordApplicator(ChemigationPermitAnnualRecordID, ApplicatorName, CertificationNumber, ExpirationYear, HomePhone, MobilePhone)
	select cparnew.ChemigationPermitAnnualRecordID, cpara.ApplicatorName, cpara.CertificationNumber, cpara.ExpirationYear, cpara.HomePhone, cpara.MobilePhone
	from dbo.ChemigationPermit cp
	join #chemigationPermitsToRenew a on cp.ChemigationPermitID = a.ChemigationPermitID
	join dbo.ChemigationPermitAnnualRecord cpar on cp.ChemigationPermitID = cpar.ChemigationPermitID and cpar.RecordYear = a.LatestRecordYear
	join dbo.ChemigationPermitAnnualRecordApplicator cpara on cpar.ChemigationPermitAnnualRecordID = cpara.ChemigationPermitAnnualRecordID
	join dbo.ChemigationPermitAnnualRecord cparnew on cp.ChemigationPermitID = cparnew.ChemigationPermitID and cparnew.RecordYear = @recordYear

	insert into dbo.ChemigationPermitAnnualRecordChemicalFormulation(ChemigationPermitAnnualRecordID, ChemicalFormulationID, ChemicalUnitID, AcresTreated)
	select cparnew.ChemigationPermitAnnualRecordID, cparcf.ChemicalFormulationID, cparcf.ChemicalUnitID, cparcf.AcresTreated
	from dbo.ChemigationPermit cp
	join #chemigationPermitsToRenew a on cp.ChemigationPermitID = a.ChemigationPermitID
	join dbo.ChemigationPermitAnnualRecord cpar on cp.ChemigationPermitID = cpar.ChemigationPermitID and cpar.RecordYear = a.LatestRecordYear
	join dbo.ChemigationPermitAnnualRecordChemicalFormulation cparcf on cpar.ChemigationPermitAnnualRecordID = cparcf.ChemigationPermitAnnualRecordID
	join dbo.ChemigationPermitAnnualRecord cparnew on cp.ChemigationPermitID = cparnew.ChemigationPermitID and cparnew.RecordYear = @recordYear

	select @recordsCreated = count(ChemigationPermitID) from #chemigationPermitsToRenew
end

GO