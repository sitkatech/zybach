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
	InspectionNotes varchar(500) null
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
	InspectionNotes
)
select w.WellID,
dateadd(hour, 8, concat(month(bwli.InspectionDate), '-', day(bwli.InspectionDate), '-', year(bwli.InspectionDate))) as InspectionDate,
	bwli.[Status],
	bwli.MeasuringEquipment,
	bwli.Oil,
	bwli.BrokenTape,
	bwli.Accuracy,
	bwli.Method,
	bwli.Party,
	bwli.SourceAgency,
	bwli.SourceCode,
	bwli.TimeDatumCode,
	bwli.TimeDatumReliability,
	bwli.LevelTypeCode,
	bwli.AgencyCode,
	bwli.Access,
	bwli.Hold,
	bwli.Cut,
	bwli.MP,
	bwli.Measurement,
	bwli.WaterLevel,
	ct.CropTypeID,
	u.UserID as InspectorUserID,
	bwli.Comment

from dbo.BeehiveWaterLevelInspection bwli
join dbo.Well w on bwli.WellRegistrationID = w.WellRegistrationID
left join dbo.CropType ct on bwli.Crop = ct.CropTypeDisplayName
join dbo.[User] u on case when bwli.InspectorUser = 'system' then 'Phil Heimann' when bwli.InspectorUser = 'Glen - Surface' then 'Glen Bowers' else bwli.InspectorUser end = u.FirstName + ' ' + u.LastName
where bwli.InspectionDate is not null and bwli.IsPrimary = 1


drop table dbo.BeehiveWaterLevelInspection

Insert into dbo.CustomRichTextType (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
values

(14, 'WaterLevelInspections', 'Water Level Inspections')

Insert into dbo.CustomRichText(CustomRichTextTypeID, CustomRichTextContent)
values

(14, 'Default content for: Water Level Inspections')