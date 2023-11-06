create table #syncYear (
	[SyncYear] int NOT NULL
)
create table #syncMonth (
	[SyncMonth] int NOT NULL
)

insert into #syncYear (SyncYear) values (2020), (2021), (2022), (2023)
insert into #syncMonth (SyncMonth) values (1), (2), (3), (4), (5), (6), (7), (8), (9), (10), (11), (12)

go

-- create an OpenETSync record for each year/month/data type combo
insert into dbo.OpenETSync ([Year], [Month], OpenETDataTypeID)
select sy.SyncYear, sm.SyncMonth, oedt.OpenETDataTypeID
from #syncYear sy
full join #syncMonth sm on 1 = 1
full join OpenETDataType oedt on 1 = 1