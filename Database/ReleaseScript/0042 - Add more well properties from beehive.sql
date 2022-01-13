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
alter table dbo.Well add RequiresChemigation bit null, RequiresWaterLevelInspection bit null, WellDepth decimal(10,4), Clearinghouse varchar(100), PageNumber int, SiteName varchar(100), SiteNumber varchar(100), ScreenInterval varchar(100), ScreenDepth decimal(10,4) 
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
set RequiresChemigation = 0, RequiresWaterLevelInspection = 0, IsReplacement = 0, LastUpdateDate = GETUTCDATE()

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

update dbo.Well
set WellGeometry = geometry::STGeomFromText('POINT (101.1857 40.9267)', 4326)
where WellRegistrationID = 'G-069878'

update w
set w.OwnerName = bw.OwnerName, w.OwnerAddress = bw.OwnerAddress, w.OwnerCity = bw.OwnerCity, w.OwnerState = bw.OwnerState, w.OwnerZipCode = bw.OwnerZip
, w.AdditionalContactName = bw.ContactName, w.AdditionalContactAddress = bw.ContactAddress, w.AdditionalContactCity = bw.ContactCity, w.AdditionalContactState = bw.ContactState, w.AdditionalContactZipCode = bw.ContactZip
, w.IsReplacement = isnull(bw.Replacement, 0), w.WellDepth = bw.WellDepth, w.Clearinghouse = bw.Clearinghouse, w.PageNumber = bw.[Page]
, w.SiteName = bw.SiteName, w.SiteNumber = bw.SiteNumber, w.ScreenInterval = bw.ScreenInterval
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
, w.TownshipRangeSection = ltrim(rtrim(bw.Township + ' ' + bw.[Range] + isnull(bw.[Range Direction], '') + ' ' + bw.Section + ' ' + isnull(bw.[Quarter], '')))
, w.WellNickName = bw.WellName

from dbo.BeehiveWell bw
join dbo.Well w on bw.WellRegistrationID = w.WellRegistrationID
left join dbo.WellUse wu on bw.SecondaryWellUse = wu.WellUseDisplayName
left join dbo.County c on bw.County = c.CountyDisplayName
-- ignore these wells that have duplicate Reg #s and need to be merged
where bw.WellRegistrationID not in
(
  'G-067335'
, 'G-102628'

,'G-050193'
, 'G-068902'
, 'G-069354'

, 'G-048825'
, 'G-064390'
)
-- ignore these wells that are in there as SiteNumber until we get closure on how to proceed
and bw.WellRegistrationID not in
(
 'Vote'
, '36A/4'
, '56-C-81/3'
, '56-C-81/2'
, '30-C-81/1'
, '64-C-81/1'
, '05-C-81/1'
, '06-C-81/1'
, '58-C-81/1'
, '57-C-81/1'
, '32-C-81/2'
, 'USGS-p12'
, 'NPPD-p27'
, 'USGS-p104'
, 'USGS-p15'
, 'Monthly-3'
, 'USGS-p2'
, 'Nichols Son'
, 'Monthly-1'
, 'Monthly-2'
, '07-C-81/1'
)

insert into dbo.WellWaterQualityInspectionType(WellID, WaterQualityInspectionTypeID)
select w.WellID, 1 as WaterQualityInspectionTypeID
from dbo.BeehiveWell bw
join dbo.Well w on bw.WellRegistrationID = w.WellRegistrationID
where bw.WellUse not like '%summer%' 
and bw.WellType like '%quality%' 
and bw.WellRegistrationID not in
(
  'G-067335'
, 'G-102628'

,'G-050193'
, 'G-068902'
, 'G-069354'

, 'G-048825'
, 'G-064390'
)
-- ignore these wells that are in there as SiteNumber until we get closure on how to proceed
and bw.WellRegistrationID not in
(
 'Vote'
, '36A/4'
, '56-C-81/3'
, '56-C-81/2'
, '30-C-81/1'
, '64-C-81/1'
, '05-C-81/1'
, '06-C-81/1'
, '58-C-81/1'
, '57-C-81/1'
, '32-C-81/2'
, 'USGS-p12'
, 'NPPD-p27'
, 'USGS-p104'
, 'USGS-p15'
, 'Monthly-3'
, 'USGS-p2'
, 'Nichols Son'
, 'Monthly-1'
, 'Monthly-2'
, '07-C-81/1'
)

union all
select w.WellID, 2 as WaterQualityInspectionTypeID
from dbo.BeehiveWell bw
join dbo.Well w on bw.WellRegistrationID = w.WellRegistrationID
where bw.WellUse like '%summer%'
and bw.WellType like '%quality%' 
and bw.WellRegistrationID not in
(
  'G-067335'
, 'G-102628'

,'G-050193'
, 'G-068902'
, 'G-069354'

, 'G-048825'
, 'G-064390'
)
-- ignore these wells that are in there as SiteNumber until we get closure on how to proceed
and bw.WellRegistrationID not in
(
 'Vote'
, '36A/4'
, '56-C-81/3'
, '56-C-81/2'
, '30-C-81/1'
, '64-C-81/1'
, '05-C-81/1'
, '06-C-81/1'
, '58-C-81/1'
, '57-C-81/1'
, '32-C-81/2'
, 'USGS-p12'
, 'NPPD-p27'
, 'USGS-p104'
, 'USGS-p15'
, 'Monthly-3'
, 'USGS-p2'
, 'Nichols Son'
, 'Monthly-1'
, 'Monthly-2'
, '07-C-81/1'
)


update w
set w.OwnerName = bw.OwnerName, w.OwnerAddress = bw.OwnerAddress, w.OwnerCity = bw.OwnerCity, w.OwnerState = bw.OwnerState, w.OwnerZipCode = bw.OwnerZip
, w.AdditionalContactName = bw.ContactName, w.AdditionalContactAddress = bw.ContactAddress, w.AdditionalContactCity = bw.ContactCity, w.AdditionalContactState = bw.ContactState, w.AdditionalContactZipCode = bw.ContactZip
, w.IsReplacement = isnull(bw.Replacement, 0), w.WellDepth = bw.WellDepth, w.ClearingHouse = bw.ClearingHouse, w.PageNumber = bw.[Page]
, w.SiteName = bw.SiteName, w.SiteNumber = bw.SiteNumber, w.ScreenInterval = bw.ScreenInterval
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
, w.Notes = bw.WellRegistrationID + + CHAR(13)+CHAR(10) + 
	case 
	when bw.LocationNote is not null and bw.Comment is not null then bw.LocationNote + CHAR(13)+CHAR(10) + bw.Comment
	when bw.LocationNote is not null then bw.LocationNote 
	else isnull(bw.Comment, '') end
, w.CountyID = c.CountyID
, w.TownshipRangeSection = ltrim(rtrim(bw.Township + ' ' + bw.[Range] + isnull(bw.[Range Direction], '') + ' ' + bw.Section + ' ' + isnull(bw.[Quarter], '')))
, w.WellNickName = bw.WellName

from dbo.BeehiveWell bw
join dbo.Well w on bw.SiteNumber = w.WellRegistrationID
left join dbo.WellUse wu on bw.SecondaryWellUse = wu.WellUseDisplayName
left join dbo.County c on bw.County = c.CountyDisplayName
where bw.SiteNumber is not null 
-- ignore these wells that are in there as SiteNumber until we get closure on how to proceed
and bw.WellRegistrationID in
(
 'Vote'
, '36A/4'
, '56-C-81/3'
, '56-C-81/2'
, '30-C-81/1'
, '64-C-81/1'
, '05-C-81/1'
, '06-C-81/1'
, '58-C-81/1'
, '57-C-81/1'
, '32-C-81/2'
, 'USGS-p12'
, 'NPPD-p27'
, 'USGS-p104'
, 'USGS-p15'
, 'Monthly-3'
, 'USGS-p2'
, 'Nichols Son'
, 'Monthly-1'
, 'Monthly-2'
, '07-C-81/1'
)


insert into dbo.WellWaterQualityInspectionType(WellID, WaterQualityInspectionTypeID)
select w.WellID, 1 as WaterQualityInspectionTypeID
from dbo.BeehiveWell bw
join dbo.Well w on bw.SiteNumber = w.WellRegistrationID
where bw.WellUse not like '%summer%' 
and bw.WellType like '%quality%' 
and bw.SiteNumber is not null
and bw.WellRegistrationID in
(
 'Vote'
, '36A/4'
, '56-C-81/3'
, '56-C-81/2'
, '30-C-81/1'
, '64-C-81/1'
, '05-C-81/1'
, '06-C-81/1'
, '58-C-81/1'
, '57-C-81/1'
, '32-C-81/2'
, 'USGS-p12'
, 'NPPD-p27'
, 'USGS-p104'
, 'USGS-p15'
, 'Monthly-3'
, 'USGS-p2'
, 'Nichols Son'
, 'Monthly-1'
, 'Monthly-2'
, '07-C-81/1'
)

union all
select w.WellID, 2 as WaterQualityInspectionTypeID
from dbo.BeehiveWell bw
join dbo.Well w on bw.SiteNumber = w.WellRegistrationID
where bw.WellUse like '%summer%'
and bw.WellType like '%quality%' 
and bw.SiteNumber is not null 
and bw.WellRegistrationID in
(
 'Vote'
, '36A/4'
, '56-C-81/3'
, '56-C-81/2'
, '30-C-81/1'
, '64-C-81/1'
, '05-C-81/1'
, '06-C-81/1'
, '58-C-81/1'
, '57-C-81/1'
, '32-C-81/2'
, 'USGS-p12'
, 'NPPD-p27'
, 'USGS-p104'
, 'USGS-p15'
, 'Monthly-3'
, 'USGS-p2'
, 'Nichols Son'
, 'Monthly-1'
, 'Monthly-2'
, '07-C-81/1'
)


-- special case wells
update w
set w.OwnerName = 'Spurgin, Inc c/o Mark Spurgin', w.OwnerAddress = '790 Road East R South', w.OwnerCity = 'Paxton', w.OwnerState = 'NE', w.OwnerZipCode = '69155-2740'
, w.IsReplacement = 0, w.WellDepth = 375, w.PageNumber = 102
, w.RequiresChemigation = 1
, w.RequiresWaterLevelInspection = 1
, w.Notes = 'http://data.dnr.ne.gov/Wells/MapConnector.aspx?wellid=G-050193'
, w.CountyID = 2
, w.TownshipRangeSection = '12 36W 8'
, w.WellNickName = '123608 (Spurgin)(p102)'

from dbo.Well w
where w.WellRegistrationID = 'G-050193'

update w
set w.OwnerName = 'Goertz Family Farms, LLC', w.OwnerAddress = '9 Eagle Canyon East 3', w.OwnerCity = 'Brule', w.OwnerState = 'NE', w.OwnerZipCode = '69127'
, w.IsReplacement = 0, w.WellDepth = 390
, w.RequiresChemigation = 1
, w.RequiresWaterLevelInspection = 1
, w.Notes = 'http://data.dnr.ne.gov/Wells/MapConnector.aspx?wellid=G-068902'
, w.CountyID = 2
, w.TownshipRangeSection = '12 37W 3'
, w.WellNickName = '123703 (Goertz)(SE)'

from dbo.Well w
where w.WellRegistrationID = 'G-068902'

update w
set w.OwnerName = 'Peterson Land & Cattle Co', w.OwnerAddress = 'PO Box 212', w.OwnerCity = 'Paxton', w.OwnerState = 'NE', w.OwnerZipCode = '69155-0212'
, w.IsReplacement = 0, w.WellDepth = 400, w.PageNumber = 108, w.SiteName = '123612ao', w.SiteNumber = '410145101221001'
, w.RequiresChemigation = 1
, w.RequiresWaterLevelInspection = 1
, w.Notes = 'http://data.dnr.ne.gov/Wells/MapConnector.aspx?wellid=G-069354'
, w.CountyID = 2
, w.TownshipRangeSection = '12 36W 12'
, w.WellNickName = '123612 (Peterson)(Pg 108)'

from dbo.Well w
where w.WellRegistrationID = 'G-069354'

update w
set w.IsReplacement = 0
, w.RequiresChemigation = 1
, w.RequiresWaterLevelInspection = 1
, w.CountyID = 2
, w.TownshipRangeSection = '11 32 15'
, w.WellNickName = '113215 (NCORPE) (15-3)(#4)'

from dbo.Well w
where w.WellRegistrationID = 'G-067335'

update w
set w.OwnerName = 'Tim Thompson'
, w.WellParticipationID = 5
, w.IsReplacement = 0
, w.RequiresChemigation = 0
, w.RequiresWaterLevelInspection = 1
, w.CountyID = 2
, w.TownshipRangeSection = '14 30 21'
, w.WellNickName = '143021 (Thompson)'

from dbo.Well w
where w.WellRegistrationID = 'G-102628'


insert into dbo.WellWaterQualityInspectionType(WellID, WaterQualityInspectionTypeID)
select w.WellID, 1
from dbo.Well w
where w.WellRegistrationID = 'G-102628'


update w
set w.IsReplacement = 0
, w.RequiresChemigation = 0
, w.RequiresWaterLevelInspection = 1
, w.CountyID = 3
, w.TownshipRangeSection = '14 33 07'
, w.WellNickName = '143307 (Kelly)(#1)'
, w.Notes = 'South'
, w.WellParticipationID = 2

from dbo.Well w
where w.WellRegistrationID = 'Kelly-1'

insert into dbo.WellWaterQualityInspectionType(WellID, WaterQualityInspectionTypeID)
select w.WellID, 1
from dbo.Well w
where w.WellRegistrationID = 'Kelly-1'


update dbo.Well
set RequiresWaterLevelInspection = 1
where WellRegistrationID = 'G-048520'


insert into dbo.WellWaterQualityInspectionType(WellID, WaterQualityInspectionTypeID)
select w.WellID, 2
from dbo.Well w
where w.WellRegistrationID = 'G-107985'



update w
set w.OwnerName = 'L & W, Inc', w.OwnerAddress = '658 Road West F South', w.OwnerCity = 'Brule', w.OwnerState = 'NE', w.OwnerZipCode = '69127'
, w.IsReplacement = 0, w.WellDepth = 300
, w.RequiresChemigation = 1
, w.RequiresWaterLevelInspection = 0
, w.Notes = 'http://data.dnr.ne.gov/Wells/MapConnector.aspx?wellid=G-048825'
, w.CountyID = 2
, w.TownshipRangeSection = '12 39W 6'
, w.WellNickName = '123906 (L & W)(NW)'

from dbo.Well w
where w.WellRegistrationID = 'G-048825'


update w
set w.OwnerName = 'McPheeters, LTD', w.OwnerAddress = '261185 S McNickle Rd', w.OwnerCity = 'Gothenburg', w.OwnerState = 'NE', w.OwnerZipCode = '69138'
, w.IsReplacement = 0, w.WellDepth = 320
, w.RequiresChemigation = 1
, w.RequiresWaterLevelInspection = 0
, w.Notes = 'http://data.dnr.ne.gov/Wells/MapConnector.aspx?wellid=G-064390'
, w.CountyID = 3
, w.TownshipRangeSection = '12 27W 22'
, w.WellNickName = '122722 (McPheeters)(NE)'

from dbo.Well w
where w.WellRegistrationID = 'G-064390'
