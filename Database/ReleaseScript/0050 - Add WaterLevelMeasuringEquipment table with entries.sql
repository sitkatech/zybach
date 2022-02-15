create table dbo.WaterLevelMeasuringEquipment
(
	WaterLevelMeasuringEquipmentID int not null constraint PK_WaterLevelMeasuringEquipment_WaterLevelMeasuringEquipmentID primary key,
	WaterLevelMeasuringEquipmentName varchar(50) not null constraint AK_WaterLevelMeasuringEquipment_WaterLevelMeasuringEquipmentName unique,
	WaterLevelMeasuringEquipmentDisplayName varchar(50) not null constraint AK_WaterLevelMeasuringEquipment_WaterLevelMeasuringEquipmentDisplayName unique
)

INSERT INTO dbo.WaterLevelMeasuringEquipment(WaterLevelMeasuringEquipmentName, WaterLevelMeasuringEquipmentDisplayName)
VALUES
(1, 'Electric', 'Electric'),
(2, 'Fiberglass', 'Fiberglass'),
(3, 'Logger', 'Logger'),
(4, 'Steel', 'Steel')
