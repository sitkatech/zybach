using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects;

public class WellWithSensorStatusDto
{
    public int WellID { get; set; }
    public string WellRegistrationID { get; set; }
    public string AgHubRegisteredUser { get; set; }
    public string FieldName { get; set; }
    public List<SensorStatusDto> Sensors { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}