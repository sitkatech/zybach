declare @dateCreated datetime
set @dateCreated = '12/9/2021'
insert into dbo.Well(WellRegistrationID, WellGeometry, CreateDate, LastUpdateDate)
select bw.WellRegistrationID, geometry::STGeomFromText(WKT, 4326) as WellGeometry, @dateCreated, @dateCreated
from dbo.BeehiveWell bw
join
(
	select distinct bw.WellFeatureID, bw.WellRegistrationID, 'POINT (' + replace([Long/Lat], ',', '') + ')' as WKT
	from dbo.BeehiveWell bw
	join dbo.BeehivePermit bp on bw.WellFeatureID = bp.WellFeatureID
	where len(bw.WellRegistrationID) > 0
) a on bw.WellFeatureID = a.WellFeatureID
left join dbo.Well w on bw.WellRegistrationID = w.WellRegistrationID
where w.WellID is null AND bw.[Long/Lat] is not null

update dbo.BeehivePermit
set Township = '13N', [Range] = '33W'
where PermitNumber = '0345'

update dbo.BeehivePermit
set Township = '13N', [Range] = '33W'
where PermitNumber = '0861'

update dbo.BeehivePermit
set [Quarter] = 'E1/2'
where PermitNumber = '1382'

update dbo.BeehivePermit
set Township = '13N', [Range] = '38W'
where PermitNumber = '1446'

update dbo.BeehivePermit
set [Quarter] = 'W1/2'
where PermitNumber = '1474'

update dbo.BeehivePermit
set [Quarter] = 'NW'
where PermitNumber = '1617'

update dbo.BeehivePermit
set [Quarter] = 'NW'
where PermitNumber = '1659'

update dbo.BeehivePermit
set Section = '2'
where PermitNumber = '1827'

update dbo.BeehivePermit
set [Quarter] = 'NE'
where PermitNumber = '1910'

insert into dbo.ChemigationPermit([ChemigationPermitNumber], [ChemigationPermitStatusID], [TownshipRangeSection], [ChemigationCountyID], WellID, [DateCreated])
select PermitNumber, 1 as [ChemigationPermitStatusID], ltrim(rtrim([Quarter] + ' ' + Section + ' ' + Township + ' ' + [Range])) as TownshipRangeSection, c.ChemigationCountyID, w.WellID, dateadd(hour, 8, min(bp.[Date])) as DateCreated
from dbo.BeehivePermit bp
join dbo.ChemigationCounty c on bp.County = c.ChemigationCountyDisplayName
join dbo.Well w on bp.WellRegistrationID = w.WellRegistrationID
where PermitNumber not in ('1886', '1913')
and
PermitNumber not in 
(
'0493'
,'0495'
,'0500'
,'0509'
,'0510'
,'0571'
,'0648'
,'0692'
,'0993'
,'0994'
,'0995'
,'0997'
,'1080'
,'1197'
,'1328'
,'1346'
,'1376'
,'1394'
,'1786'
,'1787'
) -- these permit numbers have more than one permitholder
group by PermitNumber, [Quarter], Section, Township, [Range], c.ChemigationCountyID, w.WellID
order by PermitNumber, [Quarter], Section, Township, [Range], c.ChemigationCountyID, w.WellID


insert into dbo.ChemigationPermitAnnualRecord([ChemigationPermitID], [RecordYear], [ChemigationPermitAnnualRecordStatusID], [PivotName], [ChemigationInjectionUnitTypeID], [ApplicantName], [ApplicantMailingAddress], [ApplicantCity], [ApplicantState], [ApplicantZipCode], ApplicantPhone, ApplicantMobilePhone, [DateReceived], [DatePaid], NDEEAmount)
select cp.ChemigationPermitID, bp.ChemigationYear as RecordYear, 4  /* using Approved for now bp.[Status]*/, case when len(bw.[Pivot Name]) = 0 then null else bw.[Pivot Name] end as PivotName, 1 as [ChemigationInjectionUnitTypeID], PermitHolder, PermitHolderAdd, PermitHolderCity, PermitHolderState, PermitHolderZip, PermitHolderHomePhone, case when len(PermitHolderMobilePhone) = 8 then '(308) ' + PermitHolderMobilePhone when len(PermitHolderMobilePhone) = 0 then null else PermitHolderMobilePhone end
, case when bp.ReceivedDate is null then null else dateadd(hour, 8, concat(month(bp.ReceivedDate), '-', day(bp.ReceivedDate), '-', year(bp.ReceivedDate))) end as DateReceived, case when bp.DatePaid is null then null else dateadd(hour, 8, concat(month(bp.DatePaid), '-', day(bp.DatePaid), '-', year(bp.DatePaid))) end as DatePaid, DEQAmount
from dbo.BeehivePermit bp
join dbo.ChemigationPermit cp on cast(bp.PermitNumber as int) = cp.ChemigationPermitNumber
join dbo.BeehiveWell bw on bp.WellFeatureID = bw.WellFeatureID
where --PermitHolder is not null and PermitHolderAdd is not null and PermitHolderCity is not null and PermitHolderZip is not null and PermitHolderState is not null and
PermitNumber not in 
(
'0493'
,'0495'
,'0500'
,'0509'
,'0510'
,'0571'
,'0648'
,'0692'
,'0993'
,'0994'
,'0995'
,'0997'
,'1080'
,'1197'
,'1328'
,'1346'
,'1376'
,'1394'
,'1786'
,'1787'
) -- these permit numbers have more than one permitholder
order by PermitNumber, bp.ChemigationYear

insert into dbo.ChemigationPermitAnnualRecordApplicator(ChemigationPermitAnnualRecordID, ApplicatorName, CertificationNumber, ExpirationYear, HomePhone, MobilePhone)
select cpar.ChemigationPermitAnnualRecordID, a.ApplicatorName, a.CertNumber, year(a.ExpirationDate), a.[Home Phone], a.[Mobile Phone]
from dbo.BeehivePermitApplicator a
join dbo.BeehiveWell bw on a.WellFeatureID = bw.WellFeatureID
join dbo.Well w on bw.WellRegistrationID = w.WellRegistrationID
join dbo.ChemigationPermit cp on w.WellID = cp.WellID and cast(a.PermitNumber as int) = cp.ChemigationPermitNumber
join dbo.ChemigationPermitAnnualRecord cpar on cp.ChemigationPermitID = cpar.ChemigationPermitID and a.ChemigationYear = cpar.RecordYear
order by cpar.ChemigationPermitAnnualRecordID, a.ApplicatorName


insert into dbo.ChemigationPermitAnnualRecordChemicalFormulation(ChemigationPermitAnnualRecordID, ChemicalFormulationID, ChemicalUnitID, TotalApplied, AcresTreated)
select cpar.ChemigationPermitAnnualRecordID, cf.ChemicalFormulationID, cu.ChemicalUnitID, sum(a.TotalApplied), sum(a.[Acres Treated])
from dbo.BeehivePermitChemical a
join dbo.BeehiveWell bw on a.WellFeatureID = bw.WellFeatureID
join dbo.Well w on bw.WellRegistrationID = w.WellRegistrationID
join dbo.ChemigationPermit cp on w.WellID = cp.WellID and cast(a.PermitNumber as int) = cp.ChemigationPermitNumber
join dbo.ChemigationPermitAnnualRecord cpar on cp.ChemigationPermitID = cpar.ChemigationPermitID and a.ChemigationYear = cpar.RecordYear
join dbo.ChemicalFormulation cf on a.Formulation = cf.ChemicalFormulationDisplayName
join dbo.ChemicalUnit cu on 
	case 
		when a.Measurement = '#' then 'Pounds' 
		when a.Measurement = 'Ton' then 'Tons' 
		when a.Measurement = 'Pints/Ac.' then 'Pints/Acres' 
		when a.Measurement = 'Oz' then 'Ounces'
		else a.Measurement
	end 	
	= cu.ChemicalUnitPluralName
group by cpar.ChemigationPermitAnnualRecordID, cf.ChemicalFormulationID, cu.ChemicalUnitID
order by cpar.ChemigationPermitAnnualRecordID, cf.ChemicalFormulationID, cu.ChemicalUnitID

drop table dbo.BeehiveWell
drop table dbo.BeehivePermit
drop table dbo.BeehivePermitApplicator
drop table dbo.BeehivePermitChemical