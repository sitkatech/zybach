MERGE INTO dbo.ChemigationPermitAnnualRecordStatus AS Target
USING (VALUES
(1, 'PendingPayment', 'Pending Payment'),
(2, 'ReadyForReview', 'Ready For Review'),
(3, 'PendingInspection', 'Pending Inspection'),
(4, 'Approved', 'Approved'),
(5, 'Canceled', 'Canceled')
)
AS Source (ChemigationPermitAnnualRecordStatusID, ChemigationPermitAnnualRecordStatusName, ChemigationPermitAnnualRecordStatusDisplayName)
ON Target.ChemigationPermitAnnualRecordStatusID = Source.ChemigationPermitAnnualRecordStatusID
WHEN MATCHED THEN
UPDATE SET
	ChemigationPermitAnnualRecordStatusName = Source.ChemigationPermitAnnualRecordStatusName,
	ChemigationPermitAnnualRecordStatusDisplayName = Source.ChemigationPermitAnnualRecordStatusDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (ChemigationPermitAnnualRecordStatusID, ChemigationPermitAnnualRecordStatusName, ChemigationPermitAnnualRecordStatusDisplayName)
	VALUES (ChemigationPermitAnnualRecordStatusID, ChemigationPermitAnnualRecordStatusName, ChemigationPermitAnnualRecordStatusDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
