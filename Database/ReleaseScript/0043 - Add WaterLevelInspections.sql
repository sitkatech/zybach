create table dbo.WaterQualityInspection
(
	WaterQualityInspectionID int not null identity(1,1) constraint PK_WaterQualityInspection_WaterQualityInspectionID primary key,
	WellID int not null constraint FK_WaterQualityInspection_Well_WellID foreign key references dbo.Well(WellID),
	WaterQualityInspectionTypeID int not null constraint FK_WaterQualityInspection_WaterQualityInspectionType_WaterQualityInspectionTypeID foreign key references dbo.WaterQualityInspectionType(WaterQualityInspectionTypeID),
	InspectionDate datetime not null,
	InspectorUserID int not null constraint FK_WaterQualityInspection_User_InspectorUserID_UserID foreign key references dbo.[User](UserID),
	Temperature decimal(12, 4) NULL,
	PH decimal(12, 4) NULL,
	Conductivity decimal(12, 4) NULL,
	FieldAlkilinity decimal(12, 4) NULL,
	FieldNitrates decimal(12, 4) NULL,
	LabNitrates decimal(12, 4) NULL,
	Salinity decimal(12, 4) NULL,
	MV decimal(12, 4) NULL,
	Sodium decimal(12, 4) NULL,
	Calcium decimal(12, 4) NULL,
	Magnesium decimal(12, 4) NULL,
	Potassium decimal(12, 4) NULL,
	HydrogenCarbonate decimal(12, 4) NULL,
	CalciumCarbonate decimal(12, 4) NULL,
	Sulfate decimal(12, 4) NULL,
	Chloride decimal(12, 4) NULL,
	SiliconDioxide decimal(12, 4) NULL,
	CropTypeID int NULL constraint FK_WaterQualityInspection_CropType_CropTypeID foreign key references dbo.CropType(CropTypeID),
	PreWaterLevel decimal(12, 4) NULL,
	PostWaterLevel decimal(12, 4) NULL,
	InspectionNotes varchar(500) null,
	InspectionNickname varchar(100) null
)

create table dbo.WaterLevelInspection
(
	WaterLevelInspectionID int not null identity(1,1) constraint PK_WaterLevelInspection_WaterLevelInspectionID primary key,
	WellID int not null constraint FK_WaterLevelInspection_Well_WellID foreign key references dbo.Well(WellID),
	InspectionDate datetime not null,
	InspectorUserID int not null constraint FK_WaterLevelInspection_User_InspectorUserID_UserID foreign key references dbo.[User](UserID),
	WaterLevelInspectionStatus varchar(100) NULL,
	MeasuringEquipment varchar(100) NULL,
	Crop varchar(100) NULL,
	HasOil bit not NULL,
	HasBrokenTape bit not NULL,
	Accuracy varchar(100) NULL,
	Method varchar(100) NULL,
	Party varchar(100) NULL,
	SourceAgency varchar(100) NULL,
	SourceCode varchar(100) NULL,
	TimeDatumCode varchar(100) NULL,
	TimeDatumReliability varchar(100) NULL,
	LevelTypeCode varchar(100) NULL,
	AgencyCode varchar(100) NULL,
	Access varchar(100) NULL,
	Hold decimal(12, 4) NULL,
	Cut decimal(12, 4) NULL,
	MP decimal(12, 4) NULL,
	Measurement decimal(12, 4) NULL,
	IsPrimary bit NULL,
	WaterLevel decimal(12, 2) NULL,
	CropTypeID int NULL constraint FK_WaterLevelInspection_CropType_CropTypeID foreign key references dbo.CropType(CropTypeID),
	InspectionNotes varchar(500) null,
	InspectionNickname varchar(100) null
)
GO

insert into dbo.WaterLevelInspection(WellID, InspectionDate,
	WaterLevelInspectionStatus,
	MeasuringEquipment,
	HasOil,
	HasBrokenTape,
	Accuracy,
	Method,
	Party,
	SourceAgency,
	SourceCode,
	TimeDatumCode,
	TimeDatumReliability,
	LevelTypeCode,
	AgencyCode,
	Access,
	Hold,
	Cut,
	MP,
	Measurement,
	WaterLevel,
	CropTypeID,
	InspectorUserID,
	InspectionNotes,
	InspectionNickname
)
select w.WellID,
	case when bwli.InspectionDate is null then bwli.StartDate at TIME ZONE 'Central Standard Time' AT TIME ZONE 'UTC'
		else bwli.InspectionDate at TIME ZONE 'Central Standard Time' AT TIME ZONE 'UTC' end as InspectionDate,
	case when bwli.[Status] = '' then null else bwli.[Status] end,
	case when bwli.MeasuringEquipment = '' then null else bwli.MeasuringEquipment end,
	bwli.Oil,
	bwli.BrokenTape,
	case when bwli.Accuracy = '' then null else bwli.Accuracy end,
	case when bwli.Method = '' then null else bwli.Method end,
	case when bwli.Party = '' then null else bwli.Party end,
	case when bwli.SourceAgency = '' then null else bwli.SourceAgency end,
	case when bwli.SourceCode = '' then null else bwli.SourceCode end,
	case when bwli.TimeDatumCode = '' then null else bwli.TimeDatumCode end,
	case when bwli.TimeDatumReliability = '' then null else bwli.TimeDatumReliability end,
	case when bwli.LevelTypeCode = '' then null else bwli.LevelTypeCode end,
	case when bwli.AgencyCode = '' then null else bwli.AgencyCode end,
	case when bwli.Access = '' then null else bwli.Access end,
	bwli.Hold,
	bwli.Cut,
	bwli.MP,
	bwli.Measurement,
	bwli.WaterLevel,
	ct.CropTypeID,
	u.UserID as InspectorUserID,
	bwli.Comment,
	bwli.InspectionName

from dbo.BeehiveWaterLevelInspection bwli
join dbo.Well w on bwli.WellRegistrationID = w.WellRegistrationID
left join dbo.CropType ct on bwli.Crop = ct.CropTypeDisplayName
join dbo.[User] u on case when bwli.InspectorUser in ('system', 'Phil - Surface') then 'Phil Heimann' when bwli.InspectorUser = 'Glen - Surface' then 'Glen Bowers' else bwli.InspectorUser end = u.FirstName + ' ' + u.LastName
where (bwli.InspectionDate is not null or bwli.StartDate is not null) and isnull(bwli.IsPrimary, 1) = 1
-- ignore these wells that are in there as SiteNumber until we get closure on how to proceed
and bwli.WellRegistrationID not in
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

insert into dbo.WaterLevelInspection(WellID, InspectionDate,
	WaterLevelInspectionStatus,
	MeasuringEquipment,
	HasOil,
	HasBrokenTape,
	Accuracy,
	Method,
	Party,
	SourceAgency,
	SourceCode,
	TimeDatumCode,
	TimeDatumReliability,
	LevelTypeCode,
	AgencyCode,
	Access,
	Hold,
	Cut,
	MP,
	Measurement,
	WaterLevel,
	CropTypeID,
	InspectorUserID,
	InspectionNotes,
	InspectionNickname
)
select w.WellID,
	case when bwli.InspectionDate is null then bwli.StartDate at TIME ZONE 'Central Standard Time' AT TIME ZONE 'UTC'
		else bwli.InspectionDate at TIME ZONE 'Central Standard Time' AT TIME ZONE 'UTC' end as InspectionDate,
	case when bwli.[Status] = '' then null else bwli.[Status] end,
	case when bwli.MeasuringEquipment = '' then null else bwli.MeasuringEquipment end,
	bwli.Oil,
	bwli.BrokenTape,
	case when bwli.Accuracy = '' then null else bwli.Accuracy end,
	case when bwli.Method = '' then null else bwli.Method end,
	case when bwli.Party = '' then null else bwli.Party end,
	case when bwli.SourceAgency = '' then null else bwli.SourceAgency end,
	case when bwli.SourceCode = '' then null else bwli.SourceCode end,
	case when bwli.TimeDatumCode = '' then null else bwli.TimeDatumCode end,
	case when bwli.TimeDatumReliability = '' then null else bwli.TimeDatumReliability end,
	case when bwli.LevelTypeCode = '' then null else bwli.LevelTypeCode end,
	case when bwli.AgencyCode = '' then null else bwli.AgencyCode end,
	case when bwli.Access = '' then null else bwli.Access end,
	bwli.Hold,
	bwli.Cut,
	bwli.MP,
	bwli.Measurement,
	bwli.WaterLevel,
	ct.CropTypeID,
	u.UserID as InspectorUserID,
	bwli.Comment,
	bwli.InspectionName

from dbo.BeehiveWaterLevelInspection bwli
join dbo.BeehiveWell bw on bwli.WellRegistrationID = bw.WellRegistrationID
join dbo.Well w on bw.SiteNumber = w.WellRegistrationID
left join dbo.CropType ct on bwli.Crop = ct.CropTypeDisplayName
join dbo.[User] u on case when bwli.InspectorUser in ('system', 'Phil - Surface') then 'Phil Heimann' when bwli.InspectorUser = 'Glen - Surface' then 'Glen Bowers' else bwli.InspectorUser end = u.FirstName + ' ' + u.LastName
where (bwli.InspectionDate is not null or bwli.StartDate is not null) and isnull(bwli.IsPrimary, 1) = 1
-- ignore these wells that are in there as SiteNumber until we get closure on how to proceed
and bwli.WellRegistrationID in
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



Insert into dbo.CustomRichTextType (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
values

(14, 'WaterLevelInspections', 'Water Level Inspections')

Insert into dbo.CustomRichText(CustomRichTextTypeID, CustomRichTextContent)
values

(14, 'Default content for: Water Level Inspections')



insert into dbo.WaterQualityInspection(WellID, InspectionDate,
Temperature,
PH,
Conductivity,
FieldAlkilinity,
FieldNitrates,
LabNitrates,
Salinity,
MV,
Sodium,
Calcium,
Magnesium,
Potassium,
HydrogenCarbonate,
CalciumCarbonate,
Sulfate,
Chloride,
SiliconDioxide,
CropTypeID,
PreWaterLevel,
PostWaterLevel,
InspectorUserID,
WaterQualityInspectionTypeID,
InspectionNotes,
InspectionNickname
)
select w.WellID,
case when bwqi.InspectionDate is null then bwqi.StartDate at TIME ZONE 'Central Standard Time' AT TIME ZONE 'UTC'
	else bwqi.InspectionDate at TIME ZONE 'Central Standard Time' AT TIME ZONE 'UTC' end as InspectionDate,
bwqi.Temperature,
bwqi.PH,
bwqi.Conductivity,
bwqi.FieldAlkilinity,
bwqi.FieldNitrates,
bwqi.LabNitrates,
bwqi.Salinity,
bwqi.MV,
bwqi.Sodium,
bwqi.Calcium,
bwqi.Magnesium,
bwqi.Potassium,
bwqi.HydrogenCarbonate,
bwqi.CalciumCarbonate,
bwqi.Sulfate,
bwqi.Chloride,
bwqi.SiliconDioxide,
ct.CropTypeID,
bwqi.PreWaterLevel,
bwqi.PostWaterLevel,
u.UserID as InspectorUserID,
1 as WaterQualityInspectionTypeID,
bwqi.Comment,
bwqi.InspectionName

from dbo.[BeehiveWaterQualityInspection] bwqi
join dbo.Well w on bwqi.WellRegistrationID = w.WellRegistrationID
left join dbo.CropType ct on bwqi.Crop = ct.CropTypeDisplayName
join dbo.[User] u on case when bwqi.InspectorUser in ('system', 'Phil - Surface') then 'Phil Heimann' when bwqi.InspectorUser = 'Glen - Surface' then 'Glen Bowers' else bwqi.InspectorUser end = u.FirstName + ' ' + u.LastName
where bwqi.InspectionDate is not null or bwqi.StartDate is not null
-- ignore these wells that are in there as SiteNumber until we get closure on how to proceed
and bwqi.WellRegistrationID not in
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

insert into dbo.WaterQualityInspection(WellID, InspectionDate,
Temperature,
PH,
Conductivity,
FieldAlkilinity,
FieldNitrates,
LabNitrates,
Salinity,
MV,
Sodium,
Calcium,
Magnesium,
Potassium,
HydrogenCarbonate,
CalciumCarbonate,
Sulfate,
Chloride,
SiliconDioxide,
CropTypeID,
PreWaterLevel,
PostWaterLevel,
InspectorUserID,
WaterQualityInspectionTypeID,
InspectionNotes,
InspectionNickname
)
select w.WellID,
case when bwqi.InspectionDate is null then bwqi.StartDate at TIME ZONE 'Central Standard Time' AT TIME ZONE 'UTC'
	else bwqi.InspectionDate at TIME ZONE 'Central Standard Time' AT TIME ZONE 'UTC' end as InspectionDate,
bwqi.Temperature,
bwqi.PH,
bwqi.Conductivity,
bwqi.FieldAlkilinity,
bwqi.FieldNitrates,
bwqi.LabNitrates,
bwqi.Salinity,
bwqi.MV,
bwqi.Sodium,
bwqi.Calcium,
bwqi.Magnesium,
bwqi.Potassium,
bwqi.HydrogenCarbonate,
bwqi.CalciumCarbonate,
bwqi.Sulfate,
bwqi.Chloride,
bwqi.SiliconDioxide,
ct.CropTypeID,
bwqi.PreWaterLevel,
bwqi.PostWaterLevel,
u.UserID as InspectorUserID,
1 as WaterQualityInspectionTypeID,
bwqi.Comment,
bwqi.InspectionName

from dbo.[BeehiveWaterQualityInspection] bwqi
join dbo.BeehiveWell bw on bwqi.WellRegistrationID = bw.WellRegistrationID
join dbo.Well w on bw.SiteNumber = w.WellRegistrationID
left join dbo.CropType ct on bwqi.Crop = ct.CropTypeDisplayName
join dbo.[User] u on case when bwqi.InspectorUser in ('system', 'Phil - Surface') then 'Phil Heimann' when bwqi.InspectorUser = 'Glen - Surface' then 'Glen Bowers' else bwqi.InspectorUser end = u.FirstName + ' ' + u.LastName
where bwqi.InspectionDate is not null or bwqi.StartDate is not null
-- ignore these wells that are in there as SiteNumber until we get closure on how to proceed
and bwqi.WellRegistrationID in
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


update dbo.WaterQualityInspection
set WaterQualityInspectionTypeID = 2
where Month(InspectionDate) in (7, 8)
