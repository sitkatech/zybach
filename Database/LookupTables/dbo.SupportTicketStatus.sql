MERGE INTO dbo.SupportTicketStatus AS Target
USING (VALUES
(1, 'Open', 'Open', 10),
(2, 'InProgress', 'In Progress', 20),
(3, 'PendingAnomalyReview', 'Pending Anomaly Review', 25),
(4, 'Resolved', 'Resolved', 30)
)
AS Source (SupportTicketStatusID, SupportTicketStatusName, SupportTicketStatusDisplayName, SortOrder)
ON Target.SupportTicketStatusID = Source.SupportTicketStatusID
WHEN MATCHED THEN
UPDATE SET
	SupportTicketStatusName = Source.SupportTicketStatusName,
	SupportTicketStatusDisplayName = Source.SupportTicketStatusDisplayName,
	SortOrder = Source.SortOrder
WHEN NOT MATCHED BY TARGET THEN
	INSERT (SupportTicketStatusID, SupportTicketStatusName, SupportTicketStatusDisplayName, SortOrder)
	VALUES (SupportTicketStatusID, SupportTicketStatusName, SupportTicketStatusDisplayName, SortOrder)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
