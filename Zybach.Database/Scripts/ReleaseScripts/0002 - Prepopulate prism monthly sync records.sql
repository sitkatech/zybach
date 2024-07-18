DECLARE @MigrationName VARCHAR(200);
SET @MigrationName = '0002 - Prepopulate prism monthly sync records'

IF NOT EXISTS(SELECT * FROM dbo.DatabaseMigration DM WHERE DM.ReleaseScriptFileName = @MigrationName)
BEGIN

	PRINT @MigrationName;

    with months
    as
    (
        select 1 as [month] union select 2 union select 3 union select 4 union select 5 union select 6 union select 7 union select 8 union select 9 union select 10 union select 11 union select 12
    ),
    years
    as (
        select 2020 as [year] union select 2021 union select 2022 union select 2023 union select 2024
    )
    INSERT INTO [dbo].[PrismMonthlySync] ([PrismDataTypeID], [Year], [Month])
    SELECT PrismDataTypeID, [Year], [Month]
    FROM months
    cross join years
    cross join dbo.PrismDataType
    ORDER BY [Year], [Month], [PrismDataTypeID]

    INSERT INTO dbo.DatabaseMigration(MigrationAuthorName, ReleaseScriptFileName)
    SELECT 'Mikey Knowles', @MigrationName
END