CREATE TABLE [dbo].[PaigeWirelessPulse](
	[PaigeWirelessPulseID] [int] IDENTITY(1,1) NOT NULL,
	[ReceivedDate] [datetime] NOT NULL,
	[SensorName] [varchar](100) NOT NULL,
	[EventMessage] [varchar](max) NOT NULL,
 CONSTRAINT [PK_PaigeWirelessPulse_WellID] PRIMARY KEY CLUSTERED 
(
	[PaigeWirelessPulseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
