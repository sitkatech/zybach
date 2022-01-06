drop table dbo.ChemigationInspection
go

create table dbo.ChemigationInspectionType
(
	ChemigationInspectionTypeID int not null constraint PK_ChemigationInspectionType_ChemigationInspectionTypeID primary key,
	ChemigationInspectionTypeName varchar(50) not null constraint AK_ChemigationInspectionType_ChemigationInspectionTypeName unique,
	ChemigationInspectionTypeDisplayName varchar(50) not null constraint AK_ChemigationInspectionType_ChemigationInspectionTypeDisplayName unique
)

create table dbo.ChemigationMainlineCheckValve
(
	ChemigationMainlineCheckValveID int not null constraint PK_ChemigationMainlineCheckValve_ChemigationMainlineCheckValveID primary key,
	ChemigationMainlineCheckValveName varchar(50) not null constraint AK_ChemigationMainlineCheckValve_ChemigationMainlineCheckValveName unique,
	ChemigationMainlineCheckValveDisplayName varchar(50) not null constraint AK_ChemigationMainlineCheckValve_ChemigationMainlineCheckValveDisplayName unique
)

create table dbo.ChemigationLowPressureValve
(
	ChemigationLowPressureValveID int not null constraint PK_ChemigationLowPressureValve_ChemigationLowPressureValveID primary key,
	ChemigationLowPressureValveName varchar(50) not null constraint AK_ChemigationLowPressureValve_ChemigationLowPressureValveName unique,
	ChemigationLowPressureValveDisplayName varchar(50) not null constraint AK_ChemigationLowPressureValve_ChemigationLowPressureValveDisplayName unique
)

create table dbo.ChemigationInjectionValve
(
	ChemigationInjectionValveID int not null constraint PK_ChemigationInjectionValve_ChemigationInjectionValveID primary key,
	ChemigationInjectionValveName varchar(50) not null constraint AK_ChemigationInjectionValve_ChemigationInjectionValveName unique,
	ChemigationInjectionValveDisplayName varchar(50) not null constraint AK_ChemigationInjectionValve_ChemigationInjectionValveDisplayName unique
)

create table dbo.ChemigationInterlockType
(
	ChemigationInterlockTypeID int not null constraint PK_ChemigationInterlockType_ChemigationInterlockTypeID primary key,
	ChemigationInterlockTypeName varchar(50) not null constraint AK_ChemigationInterlockType_ChemigationInterlockTypeName unique,
	ChemigationInterlockTypeDisplayName varchar(50) not null constraint AK_ChemigationInterlockType_ChemigationInterlockTypeDisplayName unique
)


create table dbo.Tillage
(
	TillageID int not null constraint PK_Tillage_TillageID primary key,
	TillageName varchar(50) not null constraint AK_Tillage_TillageName unique,
	TillageDisplayName varchar(50) not null constraint AK_Tillage_TillageDisplayName unique
)

create table dbo.CropType
(
	CropTypeID int not null constraint PK_CropType_CropTypeID primary key,
	CropTypeName varchar(50) not null constraint AK_CropType_CropTypeName unique,
	CropTypeDisplayName varchar(50) not null constraint AK_CropType_CropTypeDisplayName unique
)

create table dbo.ChemigationInspectionFailureReason
(
	ChemigationInspectionFailureReasonID int not null constraint PK_ChemigationInspectionFailureReason_ChemigationInspectionFailureReasonID primary key,
	ChemigationInspectionFailureReasonName varchar(50) not null constraint AK_ChemigationInspectionFailureReason_ChemigationInspectionFailureReasonName unique,
	ChemigationInspectionFailureReasonDisplayName varchar(50) not null constraint AK_ChemigationInspectionFailureReason_ChemigationInspectionFailureReasonDisplayName unique
)

create table dbo.ChemigationInspectionStatus
(
	ChemigationInspectionStatusID int not null constraint PK_ChemigationInspectionStatus_ChemigationInspectionStatusID primary key,
	ChemigationInspectionStatusName varchar(50) not null constraint AK_ChemigationInspectionStatus_ChemigationInspectionStatusName unique,
	ChemigationInspectionStatusDisplayName varchar(50) not null constraint AK_ChemigationInspectionStatus_ChemigationInspectionStatusDisplayName unique
)

create table dbo.ChemigationInspection
(
	ChemigationInspectionID int not null identity(1,1) constraint PK_ChemigationInspection_ChemigationInspectionID primary key,
	ChemigationPermitAnnualRecordID int not null constraint FK_ChemigationInspection_ChemigationPermitAnnualRecord_ChemigationPermitAnnualRecordID foreign key references dbo.ChemigationPermitAnnualRecord (ChemigationPermitAnnualRecordID),
	ChemigationInspectionStatusID int not null constraint FK_ChemigationInspection_ChemigationInspectionStatus_ChemigationInspectionStatusID foreign key references dbo.ChemigationInspectionStatus (ChemigationInspectionStatusID),
	ChemigationInspectionTypeID int null constraint FK_ChemigationInspection_ChemigationInspectionType_ChemigationInspectionTypeID foreign key references dbo.ChemigationInspectionType (ChemigationInspectionTypeID),
	InspectionDate datetime null,
	InspectorUserID int null constraint FK_ChemigationInspection_User_InspectorUserID_UserID foreign key references dbo.[User] (UserID),
	ChemigationMainlineCheckValveID int null constraint FK_ChemigationInspection_ChemigationMainlineCheckValve_ChemigationMainlineCheckValveID foreign key references dbo.ChemigationMainlineCheckValve (ChemigationMainlineCheckValveID),
	HasVacuumReliefValve bit null,
	HasInspectionPort bit null,
	ChemigationLowPressureValveID int null constraint FK_ChemigationInspection_ChemigationLowPressureValve_ChemigationLowPressureValveID foreign key references dbo.ChemigationLowPressureValve(ChemigationLowPressureValveID),
	ChemigationInjectionValveID int null constraint FK_ChemigationInspection_ChemigationInjectionValve_ChemigationInjectionValveID foreign key references dbo.ChemigationInjectionValve (ChemigationInjectionValveID),
	ChemigationInterlockTypeID int null constraint FK_ChemigationInspection_ChemigationInterlockType_ChemigationInterlockTypeID foreign key references dbo.ChemigationInterlockType (ChemigationInterlockTypeID),
	TillageID int null constraint FK_ChemigationInspection_Tillage_TillageID foreign key references dbo.Tillage (TillageID),
	CropTypeID int null constraint FK_ChemigationInspection_CropType_CropTypeID foreign key references dbo.CropType (CropTypeID),
	InspectionNotes varchar(500) null
)

GO

insert into dbo.ChemigationInspectionStatus(ChemigationInspectionStatusID, ChemigationInspectionStatusName, ChemigationInspectionStatusDisplayName)
values
(1, 'Pending',  'Pending'),
(2, 'Pass', 'Pass'),
(3, 'Fail', 'Fail')

insert into dbo.ChemigationInspectionFailureReason(ChemigationInspectionFailureReasonID, ChemigationInspectionFailureReasonName, ChemigationInspectionFailureReasonDisplayName)
values
(1, 'Mainline Check Valve', 'Mainline Check Valve'),
(2, 'Vacuum Relief Valve', 'Vacuum Relief Valve'),
(3, 'Inspection Port', 'Inspection Port'),
(4, 'Low Pressure Valve', 'Low Pressure Valve'),
(5, 'Chemigation Injection Valve', 'Chemigation Injection Valve'),
(6, 'Other', 'Other')

insert into dbo.ChemigationMainlineCheckValve(ChemigationMainlineCheckValveID, ChemigationMainlineCheckValveName, ChemigationMainlineCheckValveDisplayName)
values
(15, 'KROY (ALUMINUM)', 'KROY (ALUMINUM)'),
(9, 'GHEEN', 'GHEEN'),
(12, 'NORTHERN', 'NORTHERN'),
(7, 'BLUE RIVER', 'BLUE RIVER'),
(24, 'VALMONT', 'VALMONT'),
(18, 'PIERCE', 'PIERCE'),
(10, 'MIDWEST', 'MIDWEST'),
(25, 'KROY-MIDWEST', 'KROY-MIDWEST'),
(26, 'LINDSEY', 'LINDSEY'),
(27, 'T&L', 'T&L'),
(28, 'WATERMAN', 'WATERMAN')

insert into dbo.ChemigationInjectionValve(ChemigationInjectionValveID, ChemigationInjectionValveName, ChemigationInjectionValveDisplayName)
values
(46, 'MR. MISTER (PLASTIC)',  'MR. MISTER (PLASTIC)'),
(32, 'NEPTUNE',  'NEPTUNE'),
(38, 'JOHN BLUE',  'JOHN BLUE'),
(44, 'SHUR MIX',  'SHUR MIX'),
(47, 'MR. MISTER (STAINLESS)',  'MR. MISTER (STAINLESS)'),
(48, 'CHEM CHECK',  'CHEM CHECK'),
(34, 'AMIAD',  'AMIAD')

insert into dbo.ChemigationInspectionType(ChemigationInspectionTypeID, ChemigationInspectionTypeName, ChemigationInspectionTypeDisplayName)
values
(1,	'EQUIPMENT REPAIR OR REPLACE', 'EQUIPMENT REPAIR OR REPLACE'),
(2,	'NEW - INITIAL OR RE-ACTIVATION',	'NEW - INITIAL OR RE-ACTIVATION'),
(3, 'RENEWAL - ROUTINE MONITORING', 'RENEWAL - ROUTINE MONITORING')

insert into dbo.ChemigationLowPressureValve(ChemigationLowPressureValveID, ChemigationLowPressureValveName, ChemigationLowPressureValveDisplayName)
values
(1, 'RUBBER DAM', 'RUBBER DAM'),
(2, 'SPRING LOADED', 'SPRING LOADED')

insert into dbo.ChemigationInterlockType(ChemigationInterlockTypeID, ChemigationInterlockTypeName, ChemigationInterlockTypeDisplayName)
values
(1, 'MECHANICAL', 'MECHANICAL'),
(2, 'ELECTRICAL', 'ELECTRICAL')

insert into dbo.Tillage(TillageID, TillageName, TillageDisplayName)
values
(1, 'No-till',  'No-till'),
(2, 'Strip-till',  'Strip-till'),
(3, 'Conventional-till',  'Conventional-till'),
(4, 'Vertical-till',  'Vertical-till')

insert into dbo.CropType(CropTypeID, CropTypeName, CropTypeDisplayName)
values
(1, 'Corn', 'Corn'),
(2, 'Beans', 'Beans'),
(3, 'Sugar Beets', 'Sugar Beets'),
(4, 'Grass/Hay', 'Grass/Hay'),
(5, 'Alfalfa', 'Alfalfa'),
(6, 'Other', 'Other'),
(7, 'Wheat', 'Wheat')


insert into dbo.ChemigationInspection(ChemigationPermitAnnualRecordID,  ChemigationInspectionTypeID, ChemigationInspectionStatusID,  InspectionDate, InspectorUserID, 
ChemigationMainlineCheckValveID, HasVacuumReliefValve, HasInspectionPort, ChemigationLowPressureValveID, ChemigationInjectionValveID, TillageID, CropTypeID, InspectionNotes, ChemigationInterlockTypeID)
select cpar.ChemigationPermitAnnualRecordID, cit.ChemigationInspectionTypeID, case when a.Reinspected1 is not null or a.Reinspected2 is not null then 3 else 2 end as ChemigationInspectionStatusID, 
a.Inspected at TIME ZONE 'Central Standard Time' AT TIME ZONE 'UTC' as InspectionDate, iu.UserID as InspectorUserID,
cmcv.ChemigationMainlineCheckValveID, 1 as HasVacuumReliefValve, 1 as HasInspectionPort, clpv.ChemigationLowPressureValveID, civ.ChemigationInjectionValveID, t.TillageID, ct.CropTypeID, a.ReasonFailed as InspectionNotes
, city.ChemigationInterlockTypeID
from dbo.BeehivePermit a
join dbo.Well w on a.WellRegistrationID = w.WellRegistrationID
join dbo.ChemigationPermit cp on w.WellID = cp.WellID and cast(a.PermitNumber as int) = cp.ChemigationPermitNumber
join dbo.ChemigationPermitAnnualRecord cpar on cp.ChemigationPermitID = cpar.ChemigationPermitID and a.ChemigationYear = cpar.RecordYear
join dbo.[User] iu on a.InspectedBy = iu.FirstName + ' ' + iu.LastName
left join dbo.ChemigationInspectionType cit on a.InspectionType = cit.ChemigationInspectionTypeDisplayName
left join dbo.ChemigationMainlineCheckValve cmcv on a.MainlineValve = cmcv.ChemigationMainlineCheckValveDisplayName
left join dbo.ChemigationLowPressureValve clpv on a.LowPressureDrain = clpv.ChemigationLowPressureValveDisplayName
left join dbo.ChemigationInjectionValve civ on a.ChemicalValve = civ.ChemigationInjectionValveDisplayName
left join dbo.ChemigationInterlockType city on a.InterlockType = city.ChemigationInterlockTypeDisplayName
left join dbo.Tillage t on a.Tillage = t.TillageDisplayName
left join dbo.CropType ct on a.Crop = ct.CropTypeDisplayName
where a.Inspected is not null

insert into dbo.ChemigationInspection(ChemigationPermitAnnualRecordID,  ChemigationInspectionTypeID, ChemigationInspectionStatusID,  InspectionDate, InspectorUserID, 
ChemigationMainlineCheckValveID, HasVacuumReliefValve, HasInspectionPort, ChemigationLowPressureValveID, ChemigationInjectionValveID, TillageID, CropTypeID, InspectionNotes, ChemigationInterlockTypeID)
select cpar.ChemigationPermitAnnualRecordID, cit.ChemigationInspectionTypeID, case when a.Reinspected2 is not null then 3 else 2 end as ChemigationInspectionStatusID, 
a.Reinspected1 at TIME ZONE 'Central Standard Time' AT TIME ZONE 'UTC' as InspectionDate, iu.UserID as InspectorUserID,
cmcv.ChemigationMainlineCheckValveID, 1 as HasVacuumReliefValve, 1 as HasInspectionPort, clpv.ChemigationLowPressureValveID, civ.ChemigationInjectionValveID, t.TillageID, ct.CropTypeID, a.ReasonFailed as InspectionNotes
, city.ChemigationInterlockTypeID
from dbo.BeehivePermit a
join dbo.Well w on a.WellRegistrationID = w.WellRegistrationID
join dbo.ChemigationPermit cp on w.WellID = cp.WellID and cast(a.PermitNumber as int) = cp.ChemigationPermitNumber
join dbo.ChemigationPermitAnnualRecord cpar on cp.ChemigationPermitID = cpar.ChemigationPermitID and a.ChemigationYear = cpar.RecordYear
join dbo.[User] iu on a.InspectedBy = iu.FirstName + ' ' + iu.LastName
left join dbo.ChemigationInspectionType cit on a.InspectionType = cit.ChemigationInspectionTypeDisplayName
left join dbo.ChemigationMainlineCheckValve cmcv on a.MainlineValve = cmcv.ChemigationMainlineCheckValveDisplayName
left join dbo.ChemigationLowPressureValve clpv on a.LowPressureDrain = clpv.ChemigationLowPressureValveDisplayName
left join dbo.ChemigationInjectionValve civ on a.ChemicalValve = civ.ChemigationInjectionValveDisplayName
left join dbo.ChemigationInterlockType city on a.InterlockType = city.ChemigationInterlockTypeDisplayName
left join dbo.Tillage t on a.Tillage = t.TillageDisplayName
left join dbo.CropType ct on a.Crop = ct.CropTypeDisplayName
where a.Inspected is not null and a.Reinspected1 is not null

insert into dbo.ChemigationInspection(ChemigationPermitAnnualRecordID,  ChemigationInspectionTypeID, ChemigationInspectionStatusID,  InspectionDate, InspectorUserID, 
ChemigationMainlineCheckValveID, HasVacuumReliefValve, HasInspectionPort, ChemigationLowPressureValveID, ChemigationInjectionValveID, TillageID, CropTypeID, InspectionNotes, ChemigationInterlockTypeID)
select cpar.ChemigationPermitAnnualRecordID, cit.ChemigationInspectionTypeID, 2 as ChemigationInspectionStatusID, 
a.Reinspected2 at TIME ZONE 'Central Standard Time' AT TIME ZONE 'UTC' as InspectionDate, iu.UserID as InspectorUserID,
cmcv.ChemigationMainlineCheckValveID, 1 as HasVacuumReliefValve, 1 as HasInspectionPort, clpv.ChemigationLowPressureValveID, civ.ChemigationInjectionValveID, t.TillageID, ct.CropTypeID, a.ReasonFailed as InspectionNotes
, city.ChemigationInterlockTypeID
from dbo.BeehivePermit a
join dbo.Well w on a.WellRegistrationID = w.WellRegistrationID
join dbo.ChemigationPermit cp on w.WellID = cp.WellID and cast(a.PermitNumber as int) = cp.ChemigationPermitNumber
join dbo.ChemigationPermitAnnualRecord cpar on cp.ChemigationPermitID = cpar.ChemigationPermitID and a.ChemigationYear = cpar.RecordYear
join dbo.[User] iu on a.InspectedBy = iu.FirstName + ' ' + iu.LastName
left join dbo.ChemigationInspectionType cit on a.InspectionType = cit.ChemigationInspectionTypeDisplayName
left join dbo.ChemigationMainlineCheckValve cmcv on a.MainlineValve = cmcv.ChemigationMainlineCheckValveDisplayName
left join dbo.ChemigationLowPressureValve clpv on a.LowPressureDrain = clpv.ChemigationLowPressureValveDisplayName
left join dbo.ChemigationInjectionValve civ on a.ChemicalValve = civ.ChemigationInjectionValveDisplayName
left join dbo.ChemigationInterlockType city on a.InterlockType = city.ChemigationInterlockTypeDisplayName
left join dbo.Tillage t on a.Tillage = t.TillageDisplayName
left join dbo.CropType ct on a.Crop = ct.CropTypeDisplayName
where a.Inspected is not null and a.Reinspected1 is not null and a.Reinspected2 is not null


drop table dbo.BeehivePermit
