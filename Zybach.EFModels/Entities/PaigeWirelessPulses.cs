using System;
using System.Collections.Generic;
using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public class PaigeWirelessPulses
{
    public static PaigeWirelessPulseDto GetLatestBySensorName(ZybachDbContext dbContext, string sensorName)
    {
        return dbContext.PaigeWirelessPulses.SingleOrDefault(x => x.SensorName == sensorName)?.AsDto();
    }

    public static IDictionary<string, int> GetLastMessageAgesBySensorName(ZybachDbContext dbContext)
    {
        var currentDate = DateTime.UtcNow;

        return dbContext.PaigeWirelessPulses.AsEnumerable()
            .ToDictionary(x => x.SensorName, y =>
            {
                var messageAge = currentDate - y.ReceivedDate;
                return (int)messageAge.TotalMinutes;
            });
    }

    public static int? GetLastMessageAgeBySensorName(ZybachDbContext dbContext, string sensorName)
    {
        var lastReceivedDate = dbContext.PaigeWirelessPulses.SingleOrDefault(x => x.SensorName == sensorName)?.ReceivedDate;
        if (lastReceivedDate == null) return null;

        var messageAge = DateTime.UtcNow - (DateTime)lastReceivedDate;
        return (int)messageAge.TotalMinutes;
    }

    public static void Create(ZybachDbContext dbContext, SensorPulseDto sensorPulseDto)
    {
        var existingPaigeWirelessPulse = dbContext.PaigeWirelessPulses.SingleOrDefault(x => x.SensorName == sensorPulseDto.SensorName);
        if (existingPaigeWirelessPulse != null)
        {
            Update(dbContext, existingPaigeWirelessPulse, sensorPulseDto);
            return;
        }

        var paigeWirelessPulse = new PaigeWirelessPulse()
        {
            SensorName = sensorPulseDto.SensorName,
            EventMessage = sensorPulseDto.EventMessage,
            ReceivedDate = sensorPulseDto.ReceivedDate
        };

        dbContext.PaigeWirelessPulses.Add(paigeWirelessPulse);
        dbContext.SaveChanges();
    }

    private static void Update(ZybachDbContext dbContext, PaigeWirelessPulse paigeWirelessPulse, SensorPulseDto sensorPulseDto)
    {
        paigeWirelessPulse.EventMessage = sensorPulseDto.EventMessage;
        paigeWirelessPulse.ReceivedDate = sensorPulseDto.ReceivedDate;

        dbContext.SaveChanges();
    }

}