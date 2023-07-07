using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities;

public static class vSensorExtensionMethods
{
    public static SensorSimpleDto AsSimpleDto(this vSensor sensor)
    {
        var sensorSimpleDto = new SensorSimpleDto
        {
            SensorID = sensor.SensorID,
            SensorName = sensor.SensorName,
            SensorTypeID = sensor.SensorTypeID,
            WellID = sensor.WellID,
            InGeoOptix = sensor.InGeoOptix,
            CreateDate = sensor.CreateDate,
            LastUpdateDate = sensor.LastUpdateDate,
            IsActive = sensor.IsActive,
            RetirementDate = sensor.RetirementDate,
            ContinuityMeterStatusID = sensor.ContinuityMeterStatusID,
            ContinuityMeterStatusLastUpdated = sensor.ContinuityMeterStatusLastUpdated,
            SnoozeStartDate = sensor.SnoozeStartDate,
            SensorTypeName = SensorType.AllLookupDictionary[sensor.SensorTypeID].SensorTypeDisplayName,
            WellRegistrationID = sensor.WellRegistrationID,
            WellPageNumber = sensor.PageNumber,
            WellOwnerName = sensor.OwnerName,
            WellTownshipRangeSection = sensor.TownshipRangeSection,
            ContinuityMeterStatus = sensor.ContinuityMeterStatusID.HasValue ? ContinuityMeterStatus.AllLookupDictionary[sensor.ContinuityMeterStatusID.Value].AsDto() : null,
            MostRecentSupportTicketID = sensor.MostRecentSupportTicketID,
            MostRecentSupportTicketTitle = sensor.MostRecentSupportTicketTitle,
            FirstReadingDate = sensor.FirstReadingDate,
            LastReadingDate = sensor.LastReadingDate,
            LastVoltageReadingDate = sensor.LastVoltageReadingDate,
            LastVoltageReading = sensor.LastVoltageReading,
            LastMessageAgeInHours = sensor.LastMessageAgeInHours
        };

        return sensorSimpleDto;
    }
}