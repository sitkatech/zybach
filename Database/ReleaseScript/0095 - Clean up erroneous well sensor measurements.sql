  
  
  delete from dbo.WellSensorMeasurement
  where WellRegistrationID = 'G-067653'
  and 
	DATEFROMPARTS(ReadingYear, ReadingMonth, ReadingDay) >= DATEFROMPARTS(2021, 10, 17)
  and 
	DATEFROMPARTS(ReadingYear, ReadingMonth, ReadingDay) <= DATEFROMPARTS(2022, 4, 22)

