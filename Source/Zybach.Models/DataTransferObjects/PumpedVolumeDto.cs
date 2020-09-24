using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Zybach.Models.DataTransferObjects
{
    public class PumpedVolumeDto
    {
        public int ReportingIntervalMinutes { get; set; }
        public List<PumpedVolumeTimePoint> PumpedVolumeTimeSeries { get; set; }
        
    }

    public class FlowMeterDto
    {
        [JsonProperty("time")]
        public DateTime ReadingTime { get; set; }
        [JsonProperty("gallons")]
        public decimal Gallons { get; set; }
        [JsonProperty("gallonsperhour")]
        public decimal GallonsPerHour { get; set; }
    }

    public class PumpedVolumeTimePoint
    {
        public DateTime StartTime { get; set; }
        public decimal PumpedVolumeGallons { get; set; }
    }

    public class WellSummaryStatisticsDto
    {
        public decimal PumpingRateGallonsPerMinute { get; set; }
    }

    public class SensorDto
    {
        public string CanonicalName { get; set; }

        [JsonProperty("Definition")]
        public SensorDefinitionDto SensorDefinitionDto { get; set; }
    }

    public class SensorDefinitionDto
    {
        [JsonProperty("sensorType")]
        public string SensorType { get; set; }
    }

    public class FolderDto
    {
        public string CanonicalName { get; set; }
    }
}
