alter table dbo.MeasurementType add UnitsDisplay varchar(50), UnitsDisplayPlural varchar(50)
alter table dbo.SensorType add ChartColor varchar(50), AnomalousChartColor varchar(50), YAxisTitle varchar(100), ReverseYAxisScale bit
GO

update dbo.SensorType
set ChartColor = '#13B5EA', AnomalousChartColor = '#294263', YAxisTitle = 'Gallons', ReverseYAxisScale = 0
where SensorTypeID	= 1

update dbo.SensorType
set ChartColor = '#4AAA42', AnomalousChartColor = '#255521', YAxisTitle = 'Gallons', ReverseYAxisScale = 0
where SensorTypeID	= 2

update dbo.SensorType
set ChartColor = '#0076C0', AnomalousChartColor = '#0076C0', YAxisTitle = 'Gallons', ReverseYAxisScale = 0
where SensorTypeID	= 3

update dbo.MeasurementType
set UnitsDisplay = 'gallon', UnitsDisplayPlural = 'gallons'
where MeasurementTypeID	= 1

update dbo.MeasurementType
set UnitsDisplay = 'gallon', UnitsDisplayPlural = 'gallons'
where MeasurementTypeID	= 2

update dbo.MeasurementType
set UnitsDisplay = 'gallon', UnitsDisplayPlural = 'gallons'
where MeasurementTypeID	= 3

update dbo.MeasurementType
set UnitsDisplay = 'foot', UnitsDisplayPlural = 'feet'
where MeasurementTypeID	= 4

update dbo.MeasurementType
set UnitsDisplay = 'millivolt', UnitsDisplayPlural = 'millivolts'
where MeasurementTypeID	= 5


alter table dbo.SensorType alter column ChartColor varchar(50) not null
alter table dbo.SensorType alter column AnomalousChartColor varchar(50) not null
alter table dbo.SensorType alter column YAxisTitle varchar(100) not null
alter table dbo.SensorType alter column ReverseYAxisScale bit not null
alter table dbo.MeasurementType alter column UnitsDisplay varchar(50) not null
alter table dbo.MeasurementType alter column UnitsDisplayPlural varchar(50) not null
GO