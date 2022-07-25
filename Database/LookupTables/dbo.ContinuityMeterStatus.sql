MERGE INTO dbo.ContinuityMeterStatus AS Target
USING (VALUES
(1, 'ReportingNormally', 'Reporting Normally'),
(2, 'AlwaysOn', 'Always On'),
(3, 'AlwaysOff', 'Always Off')
)
AS Source (ContinuityMeterStatusID, ContinuityMeterStatusName, ContinuityMeterStatusDisplayName)
ON Target.ContinuityMeterStatusID = Source.ContinuityMeterStatusID
WHEN MATCHED THEN
UPDATE SET
	ContinuityMeterStatusName = Source.ContinuityMeterStatusName,
	ContinuityMeterStatusDisplayName = Source.ContinuityMeterStatusDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (ContinuityMeterStatusID, ContinuityMeterStatusName, ContinuityMeterStatusDisplayName)
	VALUES (ContinuityMeterStatusID, ContinuityMeterStatusName, ContinuityMeterStatusDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
