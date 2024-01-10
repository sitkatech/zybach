MERGE INTO dbo.ChemigationInspectionType AS Target
USING (VALUES
(1, 'EquipmentRepairOrReplace', 'Equipment Repair or Replace'),
(2, 'NewInitialOrReactivation', 'New - Initial or Re-activation'),
(3, 'RenewalRoutineMonitoring', 'Renewal - Routine Monitoring')
)
AS Source (ChemigationInspectionTypeID, ChemigationInspectionTypeName, ChemigationInspectionTypeDisplayName)
ON Target.ChemigationInspectionTypeID = Source.ChemigationInspectionTypeID
WHEN MATCHED THEN
UPDATE SET
	ChemigationInspectionTypeName = Source.ChemigationInspectionTypeName,
	ChemigationInspectionTypeDisplayName = Source.ChemigationInspectionTypeDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (ChemigationInspectionTypeID, ChemigationInspectionTypeName, ChemigationInspectionTypeDisplayName)
	VALUES (ChemigationInspectionTypeID, ChemigationInspectionTypeName, ChemigationInspectionTypeDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
