alter table dbo.Sensor
add RetirementDate datetime null

go 

update dbo.Sensor
set RetirementDate = wsm.ReadingDate
from dbo.Sensor s
left join 
(
	select SensorName, datefromparts(ReadingYear, ReadingMonth, ReadingDay) as ReadingDate,
		rank() over(partition by SensorName order by ReadingYear desc, ReadingMonth desc, ReadingDay desc) as Ranking
	from dbo.WellSensorMeasurement
) wsm on s.SensorName = wsm.SensorName and Ranking = 1
where s.IsActive = 0

go

update dbo.Sensor
set RetirementDate = CreateDate
where IsActive = 0 and RetirementDate is null


