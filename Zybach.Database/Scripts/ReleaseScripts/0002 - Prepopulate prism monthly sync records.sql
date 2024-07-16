DECLARE @MigrationName VARCHAR(200);
SET @MigrationName = '0002 - Prepopulate prism monthly sync records'

IF NOT EXISTS(SELECT * FROM dbo.DatabaseMigration DM WHERE DM.ReleaseScriptFileName = @MigrationName)
BEGIN

	PRINT @MigrationName;

    DECLARE @StartYear INT = 2020;
    DECLARE @EndYear INT = YEAR(GETDATE());
    DECLARE @EndMonth INT = MONTH(GETDATE());
    DECLARE @CurrentYear INT;
    DECLARE @CurrentMonth INT;
    DECLARE @PrismDataTypeID INT;

    -- Declare a cursor to iterate over all data types
    DECLARE PrismDataTypeCursor CURSOR FOR
    SELECT PrismDataTypeID FROM [dbo].[PrismDataType];

    OPEN PrismDataTypeCursor;
    FETCH NEXT FROM PrismDataTypeCursor INTO @PrismDataTypeID;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @CurrentYear = @StartYear;
        WHILE @CurrentYear <= @EndYear
        BEGIN
            SET @CurrentMonth = 1;
            WHILE @CurrentMonth <= 12
            BEGIN
                -- Break the loop if we reached the current month of the current year
                IF @CurrentYear = @EndYear AND @CurrentMonth > @EndMonth
                    BREAK;

                INSERT INTO [dbo].[PrismMonthlySync] ([PrismDataTypeID], [Year], [Month])
                VALUES (@PrismDataTypeID, @CurrentYear, @CurrentMonth);

                SET @CurrentMonth = @CurrentMonth + 1;
            END
            SET @CurrentYear = @CurrentYear + 1;
        END

        FETCH NEXT FROM PrismDataTypeCursor INTO @PrismDataTypeID;
    END

    CLOSE PrismDataTypeCursor;
    DEALLOCATE PrismDataTypeCursor;

    INSERT INTO dbo.DatabaseMigration(MigrationAuthorName, ReleaseScriptFileName)
    SELECT 'Mikey Knowles', @MigrationName
END