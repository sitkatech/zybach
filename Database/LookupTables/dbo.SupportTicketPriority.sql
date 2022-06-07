MERGE INTO dbo.SupportTicketPriority AS Target
USING (VALUES
(1, 'High', 'High', 10),
(2, 'Medium', 'Medium', 20),
(3, 'Low', 'Low', 30)
)
AS Source (SupportTicketPriorityID, SupportTicketPriorityName, SupportTicketPriorityDisplayName, SortOrder)
ON Target.SupportTicketPriorityID = Source.SupportTicketPriorityID
WHEN MATCHED THEN
UPDATE SET
	SupportTicketPriorityName = Source.SupportTicketPriorityName,
	SupportTicketPriorityDisplayName = Source.SupportTicketPriorityDisplayName,
	SortOrder = Source.SortOrder
WHEN NOT MATCHED BY TARGET THEN
	INSERT (SupportTicketPriorityID, SupportTicketPriorityName, SupportTicketPriorityDisplayName, SortOrder)
	VALUES (SupportTicketPriorityID, SupportTicketPriorityName, SupportTicketPriorityDisplayName, SortOrder)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE;
