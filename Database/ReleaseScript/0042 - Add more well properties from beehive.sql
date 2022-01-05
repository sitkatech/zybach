create table dbo.WellParticipation
(
	WellParticipationID int not null constraint PK_WellParticipation_WellParticipationID primary key,
	WellParticipationName varchar(50) not null constraint AK_WellParticipation_WellParticipationName unique,
	WellParticipationDisplayName varchar(50) not null constraint AK_WellParticipation_WellParticipationDisplayName unique
)

create table dbo.WellUse
(
	WellUseID int not null constraint PK_WellUse_WellUseID primary key,
	WellUseName varchar(50) not null constraint AK_WellUse_WellUseName unique,
	WellUseDisplayName varchar(50) not null constraint AK_WellUse_WellUseDisplayName unique
)

create table dbo.WellWaterQualityInspectionType
(
	WellWaterQualityInspectionTypeID int not null identity(1,1) constraint PK_WellWaterQualityInspectionType_WellWaterQualityInspectionTypeID primary key,
	WellID int not null constraint FK_WellWaterQualityInspectionType_Well_WellID foreign key references dbo.Well(WellID),
	WaterQualityInspectionTypeID int not null constraint FK_WellWaterQualityInspectionType_WaterQualityInspectionType_WaterQualityInspectionTypeID foreign key references dbo.WaterQualityInspectionType(WaterQualityInspectionTypeID)
)

alter table dbo.Well add WellNickname varchar(100), TownshipRangeSection varchar(100)
alter table dbo.Well add CountyID int null constraint FK_Well_County_CountyID foreign key references dbo.County(CountyID)
alter table dbo.Well add WellParticipationID int null constraint FK_Well_WellParticipation_WellParticipationID foreign key references dbo.WellParticipation(WellParticipationID)
alter table dbo.Well add WellUseID int null constraint FK_Well_WellUse_WellUseID foreign key references dbo.WellUse(WellUseID)
alter table dbo.Well add RequiresChemigation bit null, RequiresWaterLevelInspection bit null, WellDepth decimal(10,4), Clearinghouse varchar(100), PageNumber int, SiteName varchar(100), SiteNumber varchar(100)
alter table dbo.Well add OwnerName varchar(100), OwnerAddress varchar(100), OwnerCity varchar(100), OwnerState varchar(20), OwnerZipCode varchar(10)
alter table dbo.Well add AdditionalContactName varchar(100), AdditionalContactAddress varchar(100), AdditionalContactCity varchar(100), AdditionalContactState varchar(20), AdditionalContactZipCode varchar(10)
alter table dbo.Well add IsReplacement bit null, Notes varchar(1000)

GO


insert into dbo.WellParticipation(WellParticipationID, WellParticipationName, WellParticipationDisplayName)
values
(1, 'GGS', 'GGS'),
(2, 'Kelly', 'Kelly'),
(3, 'Alluvial', 'Alluvial'),
(4, 'Target', 'Target'),
(5, 'Transect', 'Transect'),
(6, 'Monthly', 'Monthly'),
(7, 'COHYST', 'COHYST')


insert into dbo.WellUse(WellUseID, WellUseName, WellUseDisplayName)
values
(1, 'Irrigation', 'Irrigation'), 
(2, 'Public Supply', 'Public Supply'), 
(3, 'Domestic', 'Domestic'), 
(4, 'Monitoring', 'Monitoring')

update dbo.Well
set RequiresChemigation = 0, RequiresWaterLevelInspection = 0, IsReplacement = 0

alter table dbo.Well alter column RequiresChemigation bit not null
alter table dbo.Well alter column RequiresWaterLevelInspection bit not null
alter table dbo.Well alter column IsReplacement bit not null
GO

update dbo.BeehiveWell
set OwnerZip = '68509'
where OwnerZip = '68509.000000000000000000'

update dbo.BeehiveWell
set OwnerZip = null
where OwnerZip = '0.000000000000000000'

update w
set w.OwnerName = bw.OwnerName, w.OwnerAddress = bw.OwnerAddress, w.OwnerCity = bw.OwnerCity, w.OwnerState = bw.OwnerState, w.OwnerZipCode = bw.OwnerZip
, w.AdditionalContactName = bw.ContactName, w.AdditionalContactAddress = bw.ContactAddress, w.AdditionalContactCity = bw.ContactCity, w.AdditionalContactState = bw.ContactState, w.AdditionalContactZipCode = bw.ContactZip
, w.IsReplacement = isnull(bw.Replacement, 0), w.WellDepth = bw.WellDepth, w.Clearinghouse = bw.Clearinghouse, w.PageNumber = bw.[Page]
, w.SiteName = bw.SiteName, w.SiteNumber = bw.SiteNumber
, w.WellUseID = wu.WellUseID
, w.WellParticipationID = case 
	when bw.WellUse like '%ggs%' then 1
	when bw.WellUse like '%kelly%' then 2
	when bw.WellUse like '%Alluvial%' then 3
	when bw.WellUse like '%Target%' then 4
	when bw.WellUse like '%Transect%' then 5
	when bw.WellUse like '%Month%' then 6
	when bw.WellUse like '%Cohyst%' then 7
	else null
  end
, w.RequiresChemigation = case when bw.WellType like '%chemigation%' then 1 else 0 end
, w.RequiresWaterLevelInspection = case when bw.WellType like '%quantity%' then 1 else 0 end
, w.Notes = case 
	when bw.LocationNote is not null and bw.Comment is not null then bw.LocationNote + CHAR(13)+CHAR(10) + bw.Comment
	when bw.LocationNote is not null then bw.LocationNote 
	else bw.Comment end
, w.CountyID = c.CountyID
, w.TownshipRangeSection = ltrim(rtrim(bw.Township + ' ' + bw.[Range] + ' ' + bw.Section + ' ' + bw.[Quarter]))
, w.WellNickName = bw.WellName

--select bw.WellRegistrationID, bw.OwnerName, bw.OwnerAddress, bw.OwnerCity, bw.OwnerState, bw.OwnerZip
--, bw.ContactName, bw.ContactAddress, bw.ContactCity, bw.ContactState, bw.ContactZip
--, bw.Replacement as IsReplacement, bw.WellDepth, bw.ClearingHouse, bw.[Page] as PageNumber, bw.SiteName, bw.SiteNumber
--, bw.SecondaryWellUse, wu.WellUseID, bw.WellUse
--, case 
--	when bw.WellUse like '%ggs%' then 1
--	when bw.WellUse like '%kelly%' then 2
--	when bw.WellUse like '%Alluvial%' then 3
--	when bw.WellUse like '%Target%' then 4
--	when bw.WellUse like '%Transect%' then 5
--	when bw.WellUse like '%Month%' then 6
--	when bw.WellUse like '%Cohyst%' then 7
--	else null
--  end as WellParticipationID
--, bw.WellType
--, case 
--	when bw.WellType like '%chemigation%' then 1
--	else 0
--end as RequiresChemigation
--, case 
--	when bw.WellType like '%quantity%' then 1
--	else 0
--end as RequiresWaterLevelInspection
from dbo.BeehiveWell bw
join dbo.Well w on bw.WellRegistrationID = w.WellRegistrationID
left join dbo.WellUse wu on bw.SecondaryWellUse = wu.WellUseDisplayName
left join dbo.County c on bw.County = c.CountyDisplayName

insert into dbo.WellWaterQualityInspectionType(WellID, WaterQualityInspectionTypeID)
select w.WellID, 1 as WaterQualityInspectionTypeID
from dbo.BeehiveWell bw
join dbo.Well w on bw.WellRegistrationID = w.WellRegistrationID
where bw.WellUse not like '%summer%' 
and bw.WellType like '%quality%' 
union all
select w.WellID, 2 as WaterQualityInspectionTypeID
from dbo.BeehiveWell bw
join dbo.Well w on bw.WellRegistrationID = w.WellRegistrationID
where bw.WellUse like '%summer%'
and bw.WellType like '%quality%' 

update w
set w.OwnerName = bw.OwnerName, w.OwnerAddress = bw.OwnerAddress, w.OwnerCity = bw.OwnerCity, w.OwnerState = bw.OwnerState, w.OwnerZipCode = bw.OwnerZip
, w.AdditionalContactName = bw.ContactName, w.AdditionalContactAddress = bw.ContactAddress, w.AdditionalContactCity = bw.ContactCity, w.AdditionalContactState = bw.ContactState, w.AdditionalContactZipCode = bw.ContactZip
, w.IsReplacement = isnull(bw.Replacement, 0), w.WellDepth = bw.WellDepth, w.ClearingHouse = bw.ClearingHouse, w.PageNumber = bw.[Page]
, w.SiteName = bw.SiteName, w.SiteNumber = bw.SiteNumber
, w.WellUseID = wu.WellUseID
, w.WellParticipationID = case 
	when bw.WellUse like '%ggs%' then 1
	when bw.WellUse like '%kelly%' then 2
	when bw.WellUse like '%Alluvial%' then 3
	when bw.WellUse like '%Target%' then 4
	when bw.WellUse like '%Transect%' then 5
	when bw.WellUse like '%Month%' then 6
	when bw.WellUse like '%Cohyst%' then 7
	else null
  end
, w.RequiresChemigation = case when bw.WellType like '%chemigation%' then 1 else 0 end
, w.RequiresWaterLevelInspection = case when bw.WellType like '%quantity%' then 1 else 0 end
, w.Notes = case 
	when bw.LocationNote is not null and bw.Comment is not null then bw.LocationNote + CHAR(13)+CHAR(10) + bw.Comment
	when bw.LocationNote is not null then bw.LocationNote 
	else bw.Comment end
, w.CountyID = c.CountyID
, w.TownshipRangeSection = ltrim(rtrim(bw.[Quarter] + ' ' + bw.Section + ' ' + bw.Township + ' ' + bw.[Range]))
, w.WellNickName = bw.WellName

--select bw.WellRegistrationID, bw.OwnerName, bw.OwnerAddress, bw.OwnerCity, bw.OwnerState, bw.OwnerZip
--, bw.ContactName, bw.ContactAddress, bw.ContactCity, bw.ContactState, bw.ContactZip
--, bw.Replacement as IsReplacement, bw.WellDepth, bw.ClearingHouse, bw.[Page] as PageNumber, bw.SiteName, bw.SiteNumber
--, bw.SecondaryWellUse, wu.WellUseID, bw.WellUse
--, case 
--	when bw.WellUse like '%ggs%' then 1
--	when bw.WellUse like '%kelly%' then 2
--	when bw.WellUse like '%Alluvial%' then 3
--	when bw.WellUse like '%Target%' then 4
--	when bw.WellUse like '%Transect%' then 5
--	when bw.WellUse like '%Month%' then 6
--	when bw.WellUse like '%Cohyst%' then 7
--	else null
--  end as WellParticipationID
--, bw.WellType
--, case 
--	when bw.WellType like '%chemigation%' then 1
--	else 0
--end as RequiresChemigation
--, case 
--	when bw.WellType like '%quantity%' then 1
--	else 0
--end as RequiresWaterLevelInspection
from dbo.BeehiveWell bw
join dbo.Well w on bw.SiteNumber = w.WellRegistrationID
left join dbo.WellUse wu on bw.SecondaryWellUse = wu.WellUseDisplayName
left join dbo.County c on bw.County = c.CountyDisplayName
where bw.SiteNumber is not null and ((bw.WellRegistrationID not like 'a%' and bw.WellRegistrationID not like 'g%') or bw.WellRegistrationID is null)

insert into dbo.WellWaterQualityInspectionType(WellID, WaterQualityInspectionTypeID)
select w.WellID, 1 as WaterQualityInspectionTypeID
from dbo.BeehiveWell bw
join dbo.Well w on bw.SiteNumber = w.WellRegistrationID
where bw.WellUse not like '%summer%' 
and bw.WellType like '%quality%' 
and bw.SiteNumber is not null and ((bw.WellRegistrationID not like 'a%' and bw.WellRegistrationID not like 'g%') or bw.WellRegistrationID is null)
union all
select w.WellID, 2 as WaterQualityInspectionTypeID
from dbo.BeehiveWell bw
join dbo.Well w on bw.SiteNumber = w.WellRegistrationID
where bw.WellUse like '%summer%'
and bw.WellType like '%quality%' 
and bw.SiteNumber is not null and ((bw.WellRegistrationID not like 'a%' and bw.WellRegistrationID not like 'g%') or bw.WellRegistrationID is null)

drop table dbo.BeehiveWell
