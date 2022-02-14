create table dbo.WaterLevelMeasuringEquipment
(
	WaterLevelMeasuringEquipmentID int not null identity (1,1) constraint PK_WaterLevelMeasuringEquipment_WaterLevelMeasuringEquipmentID primary key,
	WaterLevelMeasuringEquipmentName varchar(50) not null constraint AK_WaterLevelMeasuringEquipment_WaterLevelMeasuringEquipmentName unique,
	WaterLevelMeasuringEquipmentDisplayName varchar(50) not null constraint AK_WaterLevelMeasuringEquipment_WaterLevelMeasuringEquipmentDisplayName unique
)

INSERT INTO dbo.WaterLevelMeasuringEquipment(WaterLevelMeasuringEquipmentName, WaterLevelMeasuringEquipmentDisplayName)
VALUES
('Electric', 'Electric'),
('Fiberglass', 'Fiberglass'),
('Logger', 'Logger'),
('Steel', 'Steel')
