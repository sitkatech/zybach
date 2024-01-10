DECLARE @MigrationName VARCHAR(200);
SET @MigrationName = '0001 - Run MakeValid() on AgHubIrrigationUnit geometries'

IF NOT EXISTS(SELECT * FROM dbo.DatabaseMigration DM WHERE DM.ReleaseScriptFileName = @MigrationName)
BEGIN

	PRINT @MigrationName;


    update dbo.AgHubIrrigationUnitGeometry
    set IrrigationUnitGeometry = IrrigationUnitGeometry.MakeValid()
    where IrrigationUnitGeometry.STIsValid () = 0


    INSERT INTO dbo.DatabaseMigration(MigrationAuthorName, ReleaseScriptFileName, MigrationReason)
    SELECT 'Jamie Quishenberry', @MigrationName, 'Run MakeValid() on invalid AgHubIrrigationUnit geometries'
END