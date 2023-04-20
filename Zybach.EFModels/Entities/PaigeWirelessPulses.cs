using Microsoft.EntityFrameworkCore;
using System;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public class PaigeWirelessPulses
{
    public static void Create(ZybachDbContext dbContext, SensorPulseDto sensorPulseDto)
    {
        var paigeWirelessPulse = new PaigeWirelessPulse()
        {
            SensorName = sensorPulseDto.SensorName,
            EventMessage = sensorPulseDto.EventMessage,
            ReceivedDate = sensorPulseDto.ReceivedDate
        };

        dbContext.PaigeWirelessPulses.Add(paigeWirelessPulse);
        dbContext.SaveChanges();
    }
}