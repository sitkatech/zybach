CREATE TABLE dbo.Tmp_County
	(
	CountyID int NOT NULL,
	CountyName varchar(50) NOT NULL,
	CountyDisplayName varchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_County SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.County)
	 EXEC('INSERT INTO dbo.Tmp_County (CountyID, CountyName, CountyDisplayName)
		SELECT CountyID, CountyName, CountyDisplayName FROM dbo.County WITH (HOLDLOCK TABLOCKX)')
GO
ALTER TABLE dbo.Well
	DROP CONSTRAINT FK_Well_County_CountyID
GO
ALTER TABLE dbo.ChemigationPermit
	DROP CONSTRAINT FK_ChemigationPermit_County_CountyID
GO
DROP TABLE dbo.County
GO
EXECUTE sp_rename N'dbo.Tmp_County', N'County', 'OBJECT' 
GO
ALTER TABLE dbo.County ADD CONSTRAINT
	PK_County_CountyID PRIMARY KEY CLUSTERED 
	(
	CountyID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.County ADD CONSTRAINT
	AK_County_CountyDisplayName UNIQUE NONCLUSTERED 
	(
	CountyDisplayName
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.County ADD CONSTRAINT
	AK_County_CountyName UNIQUE NONCLUSTERED 
	(
	CountyName
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO

ALTER TABLE dbo.Well ADD CONSTRAINT
	FK_Well_County_CountyID FOREIGN KEY
	(
	CountyID
	) REFERENCES dbo.County
	(
	CountyID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Well SET (LOCK_ESCALATION = TABLE)
GO


CREATE TABLE dbo.Tmp_ChemigationPermitStatus
	(
	ChemigationPermitStatusID int NOT NULL,
	ChemigationPermitStatusName varchar(50) NOT NULL,
	ChemigationPermitStatusDisplayName varchar(50) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_ChemigationPermitStatus SET (LOCK_ESCALATION = TABLE)
GO
IF EXISTS(SELECT * FROM dbo.ChemigationPermitStatus)
	 EXEC('INSERT INTO dbo.Tmp_ChemigationPermitStatus (ChemigationPermitStatusID, ChemigationPermitStatusName, ChemigationPermitStatusDisplayName)
		SELECT ChemigationPermitStatusID, ChemigationPermitStatusName, ChemigationPermitStatusDisplayName FROM dbo.ChemigationPermitStatus WITH (HOLDLOCK TABLOCKX)')
GO
ALTER TABLE dbo.ChemigationPermit
	DROP CONSTRAINT FK_ChemigationPermit_ChemigationPermitStatus_ChemigationPermitStatusID
GO
DROP TABLE dbo.ChemigationPermitStatus
GO
EXECUTE sp_rename N'dbo.Tmp_ChemigationPermitStatus', N'ChemigationPermitStatus', 'OBJECT' 
GO
ALTER TABLE dbo.ChemigationPermitStatus ADD CONSTRAINT
	PK_ChemigationPermitStatus_ChemigationPermitStatusID PRIMARY KEY CLUSTERED 
	(
	ChemigationPermitStatusID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ChemigationPermitStatus ADD CONSTRAINT
	AK_ChemigationPermitStatus_ChemigationPermitStatusDisplayName UNIQUE NONCLUSTERED 
	(
	ChemigationPermitStatusDisplayName
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ChemigationPermitStatus ADD CONSTRAINT
	AK_ChemigationPermitStatus_ChemigationPermitStatusName UNIQUE NONCLUSTERED 
	(
	ChemigationPermitStatusName
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ChemigationPermit ADD CONSTRAINT
	FK_ChemigationPermit_ChemigationPermitStatus_ChemigationPermitStatusID FOREIGN KEY
	(
	ChemigationPermitStatusID
	) REFERENCES dbo.ChemigationPermitStatus
	(
	ChemigationPermitStatusID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ChemigationPermit ADD CONSTRAINT
	FK_ChemigationPermit_County_CountyID FOREIGN KEY
	(
	CountyID
	) REFERENCES dbo.County
	(
	CountyID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.ChemigationPermit SET (LOCK_ESCALATION = TABLE)
GO
