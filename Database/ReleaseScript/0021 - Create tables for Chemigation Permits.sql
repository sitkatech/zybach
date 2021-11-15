create table dbo.ChemigationPermitStatus
(
	ChemigationPermitStatusID int not null identity (1,1) constraint PK_ChemigationPermitStatus_ChemigationPermitStatusID primary key,
	ChemigationPermitStatusName varchar(50) not null constraint AK_ChemigationPermitStatus_ChemigationPermitStatusName unique,
	ChemigationPermitStatusDisplayName varchar(50) not null constraint AK_ChemigationPermitStatus_ChemigationPermitStatusDisplayName unique
)

create table dbo.ChemigationCounty
(
	ChemigationCountyID int not null identity (1,1) constraint PK_ChemigationCounty_ChemigationCountyID primary key,
	ChemigationCountyName varchar(50) not null constraint AK_ChemigationCounty_ChemigationCountyName unique,
	ChemigationCountyDisplayName varchar(50) not null constraint AK_ChemigationCounty_ChemigationCountyDisplayName unique
)

create table dbo.ChemigationPermit
(
	ChemigationPermitID int not null identity (1,1) constraint PK_ChemigationPermit_ChemigationPermitID primary key,
	ChemigationPermitNumber int not null constraint AK_ChemigationPermit_ChemigationPermitNumber unique,
	ChemigationPermitStatusID int not null constraint FK_ChemigationPermit_ChemigationPermitStatus_ChemigationPermitStatusID foreign key
		references dbo.ChemigationPermitStatus(ChemigationPermitStatusID),
	DateCreated datetime not null,
	TownshipRangeSection varchar(100) not null,
	ChemigationCounty int not null constraint FK_ChemigationPermit_ChemigationCounty_ChemigationCountyID foreign key
		references dbo.ChemigationCounty(ChemigationCountyID),
)

GO

Insert into dbo.CustomRichTextType (CustomRichTextTypeID, CustomRichTextTypeName, CustomRichTextTypeDisplayName)
values

(9, 'Chemigation', 'Chemigation')

Insert into dbo.CustomRichText(CustomRichTextTypeID, CustomRichTextContent)
values

(9, 'Default content for: Chemigation')

INSERT INTO dbo.ChemigationPermitStatus(ChemigationPermitStatusName, ChemigationPermitStatusDisplayName)
VALUES
('Active', 'Active'),
('Inactive', 'Inactive'),
('PermInactive', 'Permanently Inactive')

INSERT INTO dbo.ChemigationCounty(ChemigationCountyName, ChemigationCountyDisplayName)
VALUES
('Arthur', 'Arthur'),
('Keith', 'Keith'),
('Lincoln', 'Lincoln'),
('McPherson', 'McPherson')