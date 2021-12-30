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
	InspectionNotes varchar(500) null
)

declare @dateCreated datetime
set @dateCreated = '12/9/2021'
insert into dbo.Well(WellRegistrationID, WellGeometry, CreateDate, LastUpdateDate)
select bw.WellRegistrationID, geometry::STGeomFromText(WKT, 4326) as WellGeometry, @dateCreated, @dateCreated
from
(
	select 'G-084200' as WellRegistrationID, 'POINT (-100.762901857 41.089739829)' as WKT union all
	select 'G-102622' as WellRegistrationID, 'POINT (-100.761345418 41.070443277)' as WKT union all
	select 'G-100216' as WellRegistrationID, 'POINT (-101.788511452 41.091186094)' as WKT union all
	select 'G-102624' as WellRegistrationID, 'POINT (41.121577802 -100.776726439)' as WKT union all
	select 'G-101546' as WellRegistrationID, 'POINT (40.945735564 -100.338764465)' as WKT union all
	select 'G-070664' as WellRegistrationID, 'POINT (-101.603866411 41.119885967)' as WKT union all
	select 'G-100213' as WellRegistrationID, 'POINT (-101.792528965 41.091174611)' as WKT union all
	select 'G-100214' as WellRegistrationID, 'POINT (-101.792366745 41.091169751)' as WKT union all
	select 'G-091066' as WellRegistrationID, 'POINT (-100.532371476 41.041852851)' as WKT union all
	select 'G-093191' as WellRegistrationID, 'POINT (-101.730340436 41.106032346)' as WKT
) bw 
left join dbo.Well w on bw.WellRegistrationID = w.WellRegistrationID
where w.WellID is null

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
InspectionNotes
)
select w.WellID,
dateadd(hour, 8, concat(month(bwqi.SampleDate), '-', day(bwqi.SampleDate), '-', year(bwqi.SampleDate))) as InspectionDate,
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
bwqi.Comment

from dbo.[BeehiveWaterQualityInspection] bwqi
join dbo.Well w on bwqi.WellRegistrationID = w.WellRegistrationID
left join dbo.CropType ct on bwqi.Crop = ct.CropTypeDisplayName
join dbo.[User] u on case when bwqi.InspectorUser = 'system' then 'Phil Heimann' when bwqi.InspectorUser = 'Glen - Surface' then 'Glen Bowers' else bwqi.InspectorUser end = u.FirstName + ' ' + u.LastName
where bwqi.SampleDate is not null

update dbo.WaterQualityInspection
set WaterQualityInspectionTypeID = 2
where Month(InspectionDate) in (7, 8)

drop table dbo.[BeehiveWaterQualityInspection]