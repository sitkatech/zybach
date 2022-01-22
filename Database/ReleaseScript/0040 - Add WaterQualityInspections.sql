create table dbo.WaterQualityInspectionType
(
	WaterQualityInspectionTypeID int not null constraint PK_WaterQualityInspectionType_WaterQualityInspectionTypeID primary key,
	WaterQualityInspectionTypeName varchar(50) not null constraint AK_WaterQualityInspectionType_WaterQualityInspectionTypeName unique,
	WaterQualityInspectionTypeDisplayName varchar(50) not null constraint AK_WaterQualityInspectionType_WaterQualityInspectionTypeDisplayName unique
)

go



insert into dbo.WaterQualityInspectionType(WaterQualityInspectionTypeID, WaterQualityInspectionTypeName, WaterQualityInspectionTypeDisplayName)
values
(1, 'FullPanel', 'Full Panel'),
(2, 'NitratesOnly', 'Nitrates Only')