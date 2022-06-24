create table #new_years
(
	[Year] int not null
)

insert into #new_years ([Year]) 
select y.WaterYear from
(
	select 2016 as WaterYear union 
	select 2017 union
	select 2018 union
	select 2019 union
	select 2020 union
	select 2021 union
	select 2022
) y 
left join dbo.WaterYearMonth wym on y.WaterYear = wym.[Year] 
where wym.WaterYearMonthID is null

go

insert into dbo.WaterYearMonth ([Year], [Month])
select nwy.[Year], m.MonthNumber
from #new_years nwy
cross join 
(
    SELECT 1 as MonthNumber UNION 
    SELECT 2 as MonthNumber UNION 
    SELECT 3 as MonthNumber UNION 
    SELECT 4 as MonthNumber UNION 
    SELECT 5 as MonthNumber UNION 
    SELECT 6 as MonthNumber UNION 
    SELECT 7 as MonthNumber UNION 
    SELECT 8 as MonthNumber UNION 
    SELECT 9 as MonthNumber UNION 
    SELECT 10 as MonthNumber UNION 
    SELECT 11 as MonthNumber UNION 
    SELECT 12 as MonthNumber
) m
left join dbo.WaterYearMonth wym on nwy.[Year] = wym.[Year] and m.MonthNumber = wym.[Month]
where wym.WaterYearMonthID is null
order by nwy.[Year], m.MonthNumber

drop table #new_years