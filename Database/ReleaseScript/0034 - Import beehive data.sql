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

-- no well reg ids but have sitenumbers and don't exist in zybach
insert into dbo.Well(WellRegistrationID, WellGeometry, CreateDate, LastUpdateDate)
select bw.SiteNumber as WellRegistrationID, 'POINT (' + replace([Long/Lat], ',', '') + ')' as WKT, @dateCreated, @dateCreated
from dbo.BeehiveWell bw
left join dbo.Well w on bw.SiteNumber = w.WellRegistrationID
where bw.SiteNumber is not null and ((bw.WellRegistrationID not like 'a%' and bw.WellRegistrationID not like 'g%') or bw.WellRegistrationID is null)
and w.WellID is null
order by bw.SiteNumber

/*
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
*/
-- permit 1913 has two different counties
insert into dbo.ChemigationPermit([ChemigationPermitNumber], [ChemigationPermitStatusID], [ChemigationCountyID], WellID, [DateCreated])
select PermitNumber, 1 as [ChemigationPermitStatusID], c.ChemigationCountyID, w.WellID, dateadd(hour, 8, min(bp.[Date])) as DateCreated
from dbo.BeehivePermit bp
join dbo.ChemigationCounty c on bp.County = c.ChemigationCountyDisplayName
join dbo.Well w on bp.WellRegistrationID = w.WellRegistrationID
where PermitNumber != '1913'-- and ChemigationYear != 2019
group by PermitNumber, c.ChemigationCountyID, w.WellID
union all
select PermitNumber, 1 as [ChemigationPermitStatusID], c.ChemigationCountyID, w.WellID, dateadd(hour, 8, min(bp.[Date])) as DateCreated
from dbo.BeehivePermit bp
join dbo.ChemigationCounty c on bp.County = c.ChemigationCountyDisplayName
join dbo.Well w on bp.WellRegistrationID = w.WellRegistrationID
where PermitNumber = '1913' and ChemigationYear != 2019
group by PermitNumber, c.ChemigationCountyID, w.WellID
order by PermitNumber, c.ChemigationCountyID, w.WellID

insert into dbo.ChemigationPermitAnnualRecord([ChemigationPermitID], [RecordYear], [ChemigationPermitAnnualRecordStatusID], [PivotName], [ChemigationInjectionUnitTypeID], [ApplicantFirstName], [ApplicantLastName], [ApplicantCompany], [ApplicantMailingAddress], [ApplicantCity], [ApplicantState], [ApplicantZipCode], ApplicantPhone, ApplicantMobilePhone, [DateReceived], [DatePaid], NDEEAmount, TownshipRangeSection, AnnualNotes, DateApproved)
select cp.ChemigationPermitID, case when bp.PermitNumber = '1944' then 2022 else bp.ChemigationYear end as RecordYear, 
case when bp.PermitNumber = '1944' then 1 when bp.[Status] in ('Inactive', 'Perm Inact') then 5 
	when bp.[Status] = 'Pending' and bp.ChemigationYear < 2021 then 5 
	when bp.[Status] = 'Pending' and bp.ChemigationYear >= 2021 then 1 
	else 4 end as [ChemigationPermitAnnualRecordStatusID]
, case when len(bw.PivotName) = 0 then null else bw.PivotName end as PivotName, 1 as [ChemigationInjectionUnitTypeID]
, PermitHolderFirstName, PermitHolderLastName, PermitHolderCompany, PermitHolderAdd, PermitHolderCity, PermitHolderState, PermitHolderZip, PermitHolderHomePhone, case when len(PermitHolderMobilePhone) = 8 then '(308) ' + PermitHolderMobilePhone when len(PermitHolderMobilePhone) = 0 then null else PermitHolderMobilePhone end
, case when bp.ReceivedDate is null then null else dateadd(hour, 8, concat(month(bp.ReceivedDate), '-', day(bp.ReceivedDate), '-', year(bp.ReceivedDate))) end as DateReceived, case when bp.DatePaid is null then null else dateadd(hour, 8, concat(month(bp.DatePaid), '-', day(bp.DatePaid), '-', year(bp.DatePaid))) end as DatePaid, DEQAmount
, ltrim(rtrim(bp.[Quarter] + ' ' + bp.Section + ' ' + bp.Township + ' ' + bp.[Range])) as TownshipRangeSection, bp.Note as AnnualNotes
, case when bp.ApprovedDate is null then null else dateadd(hour, 8, concat(month(bp.ApprovedDate), '-', day(bp.ApprovedDate), '-', year(bp.ApprovedDate))) end as DateApproved
from dbo.BeehivePermit bp
join dbo.ChemigationPermit cp on cast(bp.PermitNumber as int) = cp.ChemigationPermitNumber
join dbo.BeehiveWell bw on bp.WellFeatureID = bw.WellFeatureID
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


-- parse out company names and first lastnames
update cpar
set cpar.ApplicantCompany = a.ApplicantCompanyNew
, cpar.ApplicantFirstName = a.ApplicantFirstNameNew
, cpar.ApplicantLastName = a.ApplicantLastNameNew
from dbo.ChemigationPermitAnnualRecord cpar
join 
(
	select ChemigationPermitAnnualRecordID, ApplicantCompany, ApplicantFirstName, ApplicantLastName
		, ApplicantFirstName + ' ' + substring(ApplicantLastName, 1, charindex('(', ApplicantLastName) - 2) as ApplicantCompanyNew
		,  case when charindex(' ', ApplicantLastName, charindex('(', ApplicantLastName) + 1) = 0 
				then replace(substring(ApplicantLastName, charindex('(', ApplicantLastName) + 1, 100), ')', '')
			else 
			substring(ApplicantLastName, charindex('(', ApplicantLastName) + 1,  
			 charindex(' ', ApplicantLastName, charindex('(', ApplicantLastName) + 1) - (charindex('(', ApplicantLastName) + 1)
			 )
			end
			as ApplicantFirstNameNew
		,	case when charindex(' ', ApplicantLastName, charindex('(', ApplicantLastName) + 1) = 0
			then null
			else
			replace(substring(ApplicantLastName, charindex(' ', ApplicantLastName, charindex('(', ApplicantLastName) + 1) + 1, 100), ')', '')
			end as ApplicantLastNameNew
	from dbo.ChemigationPermitAnnualRecord
	where ApplicantLastName like '%(%' and ApplicantLastName not like '(%'
	union
	select ChemigationPermitAnnualRecordID, ApplicantCompany, ApplicantFirstName, ApplicantLastName, ApplicantFirstName as ApplicantCompanyNew
	, case when charindex(' ', ApplicantLastName) = 0 then replace(replace(ApplicantLastName, '(', ''), ')', '') else substring(ApplicantLastName, 2, charindex(' ', ApplicantLastName) - 2) end as ApplicantFirstNameNew
	, case when charindex(' ', ApplicantLastName) = 0 then null else replace(substring(ApplicantLastName, charindex(' ', ApplicantLastName) + 1, len(ApplicantLastName)), ')', '') end as ApplicantLastNameNew
	from dbo.ChemigationPermitAnnualRecord
	where ApplicantLastName like '(%'
) a on cpar.ChemigationPermitAnnualRecordID = a.ChemigationPermitAnnualRecordID

-- update the permit status
update cp
set cp.ChemigationPermitStatusID = 2
from dbo.ChemigationPermit cp
left join dbo.ChemigationPermitAnnualRecord cpar on cp.ChemigationPermitID = cpar.ChemigationPermitID and cpar.RecordYear in (2021, 2022)
where cpar.ChemigationPermitAnnualRecordID is null

update cp
set cp.ChemigationPermitStatusID = case when bp.[Status] = 'Inactive' then 2 else 1 end
from dbo.ChemigationPermit cp
join dbo.ChemigationPermitAnnualRecord cpar on cp.ChemigationPermitID = cpar.ChemigationPermitID and cpar.RecordYear = 2021
join dbo.Well w on cp.WellID = w.WellID
join dbo.BeehivePermit bp on w.WellRegistrationID = bp.WellRegistrationID and cp.ChemigationPermitNumber = bp.PermitNumber and cpar.RecordYear = bp.ChemigationYear

update cp
set cp.ChemigationPermitStatusID = 3
from dbo.ChemigationPermit cp
join dbo.Well w on cp.WellID = w.WellID
join dbo.BeehivePermit bp on 
w.WellRegistrationID = bp.WellRegistrationID 
and cp.ChemigationPermitNumber = bp.PermitNumber 
and bp.[Status] = 'Perm Inact'


-- updating more applicant names
update cpar
set cpar.ApplicantCompany = case when cpar.ApplicantCompany is null or len(cpar.ApplicantCompany) = 0 then '' else cpar.ApplicantCompany + ' ' end + case when len(cpar.ApplicantFirstName) = 0 then null else cpar.ApplicantFirstName end, cpar.ApplicantFirstName = null, cpar.ApplicantLastName = null
from dbo.ChemigationPermitAnnualRecord cpar
join dbo.ChemigationPermit cp on cpar.ChemigationPermitID = cp.ChemigationPermitID
where ApplicantLastName = '' or ApplicantLastName is null

update cpar
set cpar.ApplicantCompany = case when len(cpar.ApplicantFirstName) = 0 then '' else cpar.ApplicantFirstName + ' ' end + cpar.ApplicantLastName, cpar.ApplicantFirstName = null, cpar.ApplicantLastName = null
from dbo.ChemigationPermitAnnualRecord cpar
join dbo.ChemigationPermit cp on cpar.ChemigationPermitID = cp.ChemigationPermitID
where ApplicantLastName like '%Farms%'

update cpar
set cpar.ApplicantCompany = case when cpar.ApplicantCompany is null or len(cpar.ApplicantCompany) = 0 then '' else cpar.ApplicantCompany + ' ' end + cpar.ApplicantFirstName + ' ' + cpar.ApplicantLastName, cpar.ApplicantFirstName = null, cpar.ApplicantLastName = null
from dbo.ChemigationPermitAnnualRecord cpar
join dbo.ChemigationPermit cp on cpar.ChemigationPermitID = cp.ChemigationPermitID
where ApplicantLastName in ('Land & Cattle', 'Livestock Feeders', 'Inc', 'Farming Trust', 'Ag Inc', 'Land & Cattle Co', 'Ranch Inc', 'Ranch LLC', 'Holdings LLC', 'Cattle Co', 'Land & Livestock', 'LLC', 'Creek Ranch', 'Hecox Inc', 'Family Farm', 'Family Trust', 'Land & Seed LLC', 'Ranch', 'Seed')

update cpar
set cpar.ApplicantCompany = cpar.ApplicantLastName, cpar.ApplicantFirstName = null, cpar.ApplicantLastName = null
from dbo.ChemigationPermitAnnualRecord cpar
join dbo.ChemigationPermit cp on cpar.ChemigationPermitID = cp.ChemigationPermitID
where ApplicantLastName in ('Double Dog Ranch LLC') and ApplicantFirstName is null and ApplicantCompany is null

update cpar
set cpar.ApplicantCompany = cpar.ApplicantFirstName, cpar.ApplicantFirstName = 'Rick', cpar.ApplicantLastName = 'Johnson'
from dbo.ChemigationPermitAnnualRecord cpar
join dbo.ChemigationPermit cp on cpar.ChemigationPermitID = cp.ChemigationPermitID
where ApplicantFirstName in ('Farmer''s National Co') and ApplicantLastName = 'Rick Johnson'



drop table dbo.BeehivePermitApplicator
drop table dbo.BeehivePermitChemical