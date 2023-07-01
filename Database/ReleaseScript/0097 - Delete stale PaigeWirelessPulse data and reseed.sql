-- save only most recent pulse for each sensor name
select ReceivedDate, SensorName, EventMessage 
into #paigeWirelessPulse
from dbo.PaigeWirelessPulse 
where PaigeWirelessPulseID in (
	select PaigeWirelessPulseID from (
		select PaigeWirelessPulseID, rank() over (partition by SensorName order by ReceivedDate desc) as Ranking
		from dbo.PaigeWirelessPulse
	) pwp where Ranking = 1
)

go 

-- delete pulse records and reseed table
delete from dbo.PaigeWirelessPulse
DBCC CHECKIDENT ('PaigeWirelessPulse', RESEED, 0)

go 

-- reinsert saved pulse records
insert into dbo.PaigeWirelessPulse (ReceivedDate, SensorName, EventMessage)
select ReceivedDate, SensorName, EventMessage from #paigeWirelessPulse

go 

drop table #paigeWirelessPulse


