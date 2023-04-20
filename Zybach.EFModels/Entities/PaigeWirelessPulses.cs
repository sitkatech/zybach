using Microsoft.EntityFrameworkCore;
using System;

namespace Zybach.EFModels.Entities;

public class PaigeWirelessPulses
{
    public static void Create(ZybachDbContext dbContext, string sensorName, string eventMessage)
    {
        var paigeWirelessPulse = new PaigeWirelessPulse()
        {
            SensorName = sensorName,
            EventMessage = eventMessage,
            ReceivedDate = DateTime.UtcNow
        };

        dbContext.PaigeWirelessPulses.Add(paigeWirelessPulse);
        dbContext.SaveChanges();
    }
}