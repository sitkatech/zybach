MERGE INTO dbo.WaterQualityInspectionType AS Target
USING (VALUES
(1, 'FullPanel', 'Full Panel'),
(2, 'NitratesOnly', 'Nitrates Only')
)
AS Source (WaterQualityInspectionTypeID, WaterQualityInspectionTypeName, WaterQualityInspectionTypeDisplayName)
ON Target.WaterQualityInspectionTypeID = Source.WaterQualityInspectionTypeID
WHEN MATCHED THEN
UPDATE SET
	WaterQualityInspectionTypeName = Source.WaterQualityInspectionTypeName,
	WaterQualityInspectionTypeDisplayName = Source.WaterQualityInspectionTypeDisplayName
WHEN NOT MATCHED BY TARGET THEN
	INSERT (WaterQualityInspectionTypeID, WaterQualityInspectionTypeName, WaterQualityInspectionTypeDisplayName)
	VALUES (WaterQualityInspectionTypeID, WaterQualityInspectionTypeName, WaterQualityInspectionTypeDisplayName)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
