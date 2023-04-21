//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PaigeWirelessPulse]
using System;


namespace Zybach.Models.DataTransferObjects
{
    public partial class PaigeWirelessPulseDto
    {
        public int PaigeWirelessPulseID { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string SensorName { get; set; }
        public string EventMessage { get; set; }
    }

    public partial class PaigeWirelessPulseSimpleDto
    {
        public int PaigeWirelessPulseID { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string SensorName { get; set; }
        public string EventMessage { get; set; }
    }

}