//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [dbo].[PaigeWirelessPulse]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class PaigeWirelessPulseExtensionMethods
    {
        public static PaigeWirelessPulseDto AsDto(this PaigeWirelessPulse paigeWirelessPulse)
        {
            var paigeWirelessPulseDto = new PaigeWirelessPulseDto()
            {
                PaigeWirelessPulseID = paigeWirelessPulse.PaigeWirelessPulseID,
                ReceivedDate = paigeWirelessPulse.ReceivedDate,
                SensorName = paigeWirelessPulse.SensorName,
                EventMessage = paigeWirelessPulse.EventMessage
            };
            DoCustomMappings(paigeWirelessPulse, paigeWirelessPulseDto);
            return paigeWirelessPulseDto;
        }

        static partial void DoCustomMappings(PaigeWirelessPulse paigeWirelessPulse, PaigeWirelessPulseDto paigeWirelessPulseDto);

        public static PaigeWirelessPulseSimpleDto AsSimpleDto(this PaigeWirelessPulse paigeWirelessPulse)
        {
            var paigeWirelessPulseSimpleDto = new PaigeWirelessPulseSimpleDto()
            {
                PaigeWirelessPulseID = paigeWirelessPulse.PaigeWirelessPulseID,
                ReceivedDate = paigeWirelessPulse.ReceivedDate,
                SensorName = paigeWirelessPulse.SensorName,
                EventMessage = paigeWirelessPulse.EventMessage
            };
            DoCustomSimpleDtoMappings(paigeWirelessPulse, paigeWirelessPulseSimpleDto);
            return paigeWirelessPulseSimpleDto;
        }

        static partial void DoCustomSimpleDtoMappings(PaigeWirelessPulse paigeWirelessPulse, PaigeWirelessPulseSimpleDto paigeWirelessPulseSimpleDto);
    }
}