using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public class PaigeWirelessPulses
{
    public static PaigeWirelessPulseDto GetLatestBySensorName(ZybachDbContext dbContext, string sensorName)
    {
        return dbContext.PaigeWirelessPulses.Where(x => x.SensorName == sensorName)
            .OrderByDescending(x => x.EventMessage).FirstOrDefault()?.AsDto();
    }

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