create table dbo.ChemigationPermitStatus
(
	ChemigationPermitStatusID int not null identity (1,1) constraint PK_ChemigationPermitStatus_ChemigationPermitStatusID primary key,
	ChemigationPermitStatusName varchar(50) not null constraint AK_ChemigationPermitStatus_ChemigationPermitStatusName unique,
	ChemigationPermitStatusDisplayName varchar(50) not null constraint AK_ChemigationPermitStatus_ChemigationPermitStatusDisplayName unique
)

create table dbo.ChemigationPermit
(
	ChemigationPermitID int not null identity (1,1) constraint PK_ChemigationPermit_ChemigationPermitID primary key,
	ChemigationPermitNumber int not null constraint AK_ChemigationPermit_ChemigationPermitNumber unique,
	ChemigationPermitStatusID int not null constraint FK_ChemigationPermit_ChemigationPermitStatus_ChemigationPermitStatusID foreign key
		references dbo.ChemigationPermitStatus(ChemigationPermitStatusID),
	DateReceived datetime not null,
	TownshipRangeSection varchar(100) not null
)

Insert into dbo.CustomRichTextType (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
values

(8, 'Chemigation', 'Chemigation')

Insert into dbo.CustomRichText(CustomRichTextTypeID, CustomRichTextContent)
values

(8, 'Default content for: Chemigation')