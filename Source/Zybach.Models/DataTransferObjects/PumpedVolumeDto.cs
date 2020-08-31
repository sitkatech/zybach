using System;
using System.Collections.Generic;
using System.Text;

namespace Zybach.Models.DataTransferObjects
{
    public class PumpedVolumeDto
    {
        public PumpedVolumeTimePoint[] AveragePumpedVolumeTimeSeries { get; set; }
        public decimal PumpingRateGallonsPerMinute { get; set; }
    }

    public class PumpedVolumeTimePoint
    {
        public DateTime Time { get; set; }
        public decimal PumpedVolumeGallons { get; set; }
    }
}
