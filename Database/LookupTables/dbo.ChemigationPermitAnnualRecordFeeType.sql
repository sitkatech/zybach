MERGE INTO dbo.ChemigationPermitAnnualRecordFeeType AS Target
USING (VALUES
(1, 'New', 'New ($40)', 40),
(2, 'Renewal', 'Renewal ($20)', 20),
(3, 'Emergency', 'Emergency ($100)', 100)
)
AS Source (ChemigationPermitAnnualRecordFeeTypeID, ChemigationPermitAnnualRecordFeeTypeName, ChemigationPermitAnnualRecordFeeTypeDisplayName, FeeAmount)
ON Target.ChemigationPermitAnnualRecordFeeTypeID = Source.ChemigationPermitAnnualRecordFeeTypeID
WHEN MATCHED THEN
UPDATE SET
	ChemigationPermitAnnualRecordFeeTypeName = Source.ChemigationPermitAnnualRecordFeeTypeName,
	ChemigationPermitAnnualRecordFeeTypeDisplayName = Source.ChemigationPermitAnnualRecordFeeTypeDisplayName,
	FeeAmount = Source.FeeAmount
WHEN NOT MATCHED BY TARGET THEN
	INSERT (ChemigationPermitAnnualRecordFeeTypeID, ChemigationPermitAnnualRecordFeeTypeName, ChemigationPermitAnnualRecordFeeTypeDisplayName, FeeAmount)
	VALUES (ChemigationPermitAnnualRecordFeeTypeID, ChemigationPermitAnnualRecordFeeTypeName, ChemigationPermitAnnualRecordFeeTypeDisplayName, FeeAmount)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
