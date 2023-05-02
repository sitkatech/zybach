update SupportTicket
set WellID = 11341
where WellID = 12960

update SupportTicket
set WellID = 11774
where WellID = 12964

update SupportTicket
set WellID = 10876
where WellID = 12965

GO

delete from dbo.Well 
where WellRegistrationID in ('PW015008', 'PW015065', 'PW015318')


