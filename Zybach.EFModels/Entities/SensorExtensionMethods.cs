using System.Linq;
using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public partial class SensorExtensionMethods
    {
        static partial void DoCustomSimpleDtoMappings(Sensor sensor,
            SensorSimpleDto sensorSimpleDto)
        {
            sensorSimpleDto.SensorTypeName = sensor.SensorType.SensorTypeDisplayName;
            sensorSimpleDto.WellRegistrationID = sensor.Well?.WellRegistrationID;
            sensorSimpleDto.WellPageNumber = sensor.Well?.PageNumber;
            sensorSimpleDto.WellOwnerName = sensor.Well?.OwnerName;
            sensorSimpleDto.WellTownshipRangeSection = sensor.Well?.TownshipRangeSection;
            sensorSimpleDto.ContinuityMeterStatus = sensor.ContinuityMeterStatus?.AsDto();

            var mostRecentSupportTicket = new SupportTicket();
            if (sensor.SupportTickets != null)
            {
                mostRecentSupportTicket = sensor.SupportTickets
                    .Where(x => x.SupportTicketStatusID != (int)SupportTicketStatusEnum.Resolved)
                    .MaxBy(x => x.DateUpdated);
            }

            sensorSimpleDto.MostRecentSupportTicketID = mostRecentSupportTicket?.SupportTicketID;
            sensorSimpleDto.MostRecentSupportTicketTitle = mostRecentSupportTicket?.SupportTicketTitle;
        }
    }
}
