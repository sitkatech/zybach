CREATE TABLE dbo.PaigeWirelessPulse (
	PaigeWirelessPulseID int IDENTITY(1,1) NOT NULL CONSTRAINT PK_PaigeWirelessPulse_WellID PRIMARY KEY,
	ReceivedDate datetime not null,
    SensorName varchar(100) not null,
    EventMessage varchar(max) not null
)
