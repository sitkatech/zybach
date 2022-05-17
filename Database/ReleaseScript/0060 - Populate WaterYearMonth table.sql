create table #years
(
	[Year] int not null
)

insert into #years ([Year]) values (2021), (2022)
go

WITH Nbrs ( Number ) AS (
    SELECT 1 UNION ALL
    SELECT 1 + Number FROM Nbrs WHERE Number < 12
)

insert into dbo.WaterYearMonth ([Year], [Month])
select [Year], Number
from #years
cross join Nbrs
order by [Year], Number

drop table #years