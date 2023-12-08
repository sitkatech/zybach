IF EXISTS(SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(N'dbo.pChemigationPermitAnnualRecordBulkCreateForRecordYear'))
    drop procedure dbo.pChemigationPermitAnnualRecordBulkCreateForRecordYear
go

create procedure dbo.pChemigationPermitAnnualRecordBulkCreateForRecordYear
(
	@recordYear int,
	@chemigationPermitsRenewed int output,
	@chemigationInspectionsCreated int output
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

	insert into dbo.ChemigationPermitAnnualRecord(ChemigationPermitID, RecordYear, TownshipRangeSection, ChemigationPermitAnnualRecordStatusID, PivotName, ChemigationInjectionUnitTypeID, ApplicantCompany, ApplicantFirstName, ApplicantLastName, ApplicantMailingAddress, ApplicantCity, ApplicantState, ApplicantZipCode, ApplicantPhone, ApplicantMobilePhone, ApplicantEmail, ChemigationPermitAnnualRecordFeeTypeID, NDEEAmount)
	select cpar.ChemigationPermitID, @recordYear, cpar.TownshipRangeSection, 1 as ChemigationPermitAnnualRecordStatusID, cpar.PivotName, cpar.ChemigationInjectionUnitTypeID, cpar.ApplicantCompany, cpar.ApplicantFirstName, cpar.ApplicantLastName, cpar.ApplicantMailingAddress, cpar.ApplicantCity, cpar.ApplicantState, cpar.ApplicantZipCode, cpar.ApplicantPhone, cpar.ApplicantMobilePhone, cpar.ApplicantEmail, 2 as ChemigationPermitAnnualRecordFeeTypeID, 2.0 -- renewal amount
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

	drop table if exists #chemigationInspectionsToCreate
	select ChemigationPermitID, RecordYear, ChemigationInspectionID, ChemigationPermitAnnualRecordID, ChemigationInspectionStatusID
		, ChemigationInspectionTypeID, InspectionDate, InspectorUserID, ChemigationMainlineCheckValveID, HasVacuumReliefValve, HasInspectionPort
		, ChemigationLowPressureValveID, ChemigationInjectionValveID, ChemigationInterlockTypeID
		, TillageID, CropTypeID, InspectionNotes, ChemigationInspectionFailureReasonID, Ranking
	into #chemigationInspectionsToCreate
	from
	(
		select c.ChemigationPermitID, cpar.RecordYear, ci.ChemigationInspectionID, ci.ChemigationPermitAnnualRecordID, ci.ChemigationInspectionStatusID
		, ci.ChemigationInspectionTypeID, ci.InspectionDate, ci.InspectorUserID, ci.ChemigationMainlineCheckValveID, ci.HasVacuumReliefValve, ci.HasInspectionPort
		, ci.ChemigationLowPressureValveID, ci.ChemigationInjectionValveID, ci.ChemigationInterlockTypeID
		, ci.TillageID, ci.CropTypeID, ci.InspectionNotes, ci.ChemigationInspectionFailureReasonID
		, row_number() over (partition by c.ChemigationPermitID order by cpar.RecordYear desc, ci.InspectionDate desc) as Ranking
		from #chemigationPermitsToRenew c
		join dbo.ChemigationPermitAnnualRecord cpar on c.ChemigationPermitID = cpar.ChemigationPermitID
		join dbo.ChemigationInspection ci on cpar.ChemigationPermitAnnualRecordID = ci.ChemigationPermitAnnualRecordID
	) a
	where a.Ranking = 1

	insert into dbo.ChemigationInspection(ChemigationPermitAnnualRecordID, ChemigationInspectionStatusID, ChemigationInspectionTypeID
	, InspectorUserID, ChemigationMainlineCheckValveID, HasVacuumReliefValve, HasInspectionPort
	, ChemigationLowPressureValveID, ChemigationInjectionValveID, ChemigationInterlockTypeID, TillageID, CropTypeID)
	select cparnew.ChemigationPermitAnnualRecordID, 1 as ChemigationInspectionStatusID, 3 as ChemigationInspectionTypeID
	, c.InspectorUserID, c.ChemigationMainlineCheckValveID, c.HasVacuumReliefValve, c.HasInspectionPort
	, c.ChemigationLowPressureValveID, c.ChemigationInjectionValveID, c.ChemigationInterlockTypeID, c.TillageID, c.CropTypeID
	from dbo.ChemigationPermit cp
	join dbo.ChemigationPermitAnnualRecord cparnew on cp.ChemigationPermitID = cparnew.ChemigationPermitID and cparnew.RecordYear = @recordYear
	join #chemigationInspectionsToCreate c on cp.ChemigationPermitID = c.ChemigationPermitID and c.RecordYear <= @recordYear - 3

	select @chemigationPermitsRenewed = count(ChemigationPermitID) from #chemigationPermitsToRenew
	select @chemigationInspectionsCreated = count(ChemigationPermitID) from #chemigationInspectionsToCreate where RecordYear <= @recordYear - 3
end

GO