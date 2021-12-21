exec sp_rename 'dbo.PK_ChemigationCounty_ChemigationCountyID', 'PK_County_CountyID', 'OBJECT';
exec sp_rename 'dbo.AK_ChemigationCounty_ChemigationCountyDisplayName', 'AK_County_CountyDisplayName', 'OBJECT';
exec sp_rename 'dbo.AK_ChemigationCounty_ChemigationCountyName', 'AK_County_CountyName', 'OBJECT';
exec sp_rename 'dbo.FK_ChemigationPermit_ChemigationCounty_ChemigationCountyID', 'FK_ChemigationPermit_County_CountyID', 'OBJECT';
exec sp_rename 'dbo.ChemigationCounty.ChemigationCountyID', 'CountyID', 'COLUMN';
exec sp_rename 'dbo.ChemigationCounty.ChemigationCountyName', 'CountyName', 'COLUMN';
exec sp_rename 'dbo.ChemigationCounty.ChemigationCountyDisplayName', 'CountyDisplayName', 'COLUMN';
exec sp_rename 'dbo.ChemigationPermit.ChemigationCountyID', 'CountyID', 'COLUMN';
exec sp_rename 'dbo.ChemigationCounty', 'County';