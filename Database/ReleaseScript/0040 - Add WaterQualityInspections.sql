create table dbo.WaterQualityInspectionType
(
	WaterQualityInspectionTypeID int not null constraint PK_WaterQualityInspectionType_WaterQualityInspectionTypeID primary key,
	WaterQualityInspectionTypeName varchar(50) not null constraint AK_WaterQualityInspectionType_WaterQualityInspectionTypeName unique,
	WaterQualityInspectionTypeDisplayName varchar(50) not null constraint AK_WaterQualityInspectionType_WaterQualityInspectionTypeDisplayName unique
)

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


insert into dbo.WaterQualityInspectionType(WaterQualityInspectionTypeID, WaterQualityInspectionTypeName, WaterQualityInspectionTypeDisplayName)
values
(1, 'FullPanel', 'Full Panel'),
(2, 'NitratesOnly', 'Nitrates Only')


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
join dbo.[User] u on case when bwqi.InspectorUser = 'system' then 'Phil Heimann' when bwqi.InspectorUser = 'Glen - Surface' then 'Glen Bowers' else bwqi.InspectorUser end = u.FirstName + ' ' + u.LastName
where bwqi.InspectionDate is not null or bwqi.StartDate is not null

update dbo.WaterQualityInspection
set WaterQualityInspectionTypeID = 2
where Month(InspectionDate) in (7, 8)
