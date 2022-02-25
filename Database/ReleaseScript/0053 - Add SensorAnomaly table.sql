create table dbo.SensorAnomaly (
	SensorAnomalyID int not null constraint PK_SensorAnomaly_SensorAnomalyID primary key,
	SensorID int not null constraint FK_SensorAnomaly_Sensor_SensorID foreign key references dbo.Sensor(SensorID),
	StartDate datetime not null,
	EndDate datetime not null,
	Notes varchar(500) null
)