using System;
using System.Collections.Generic;
using System.Text;

namespace Zybach.Models.DataTransferObjects
{
    public class PumpedVolumeDto
    {
        public int ReportingIntervalMinutes { get; set; }
        public PumpedVolumeTimePoint[] PumpedVolumeTimeSeries { get; set; }
        
    }

    public class PumpedVolumeTimePoint
    {
        public DateTime Time { get; set; }
        public decimal PumpedVolumeGallons { get; set; }
    }

    public class WellSummaryStatisticsDto
    {
        public decimal PumpingRateGallonsPerMinute { get; set; }
    }
}
