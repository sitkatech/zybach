using System;
using System.Collections.Generic;
using System.Text;

namespace Zybach.Models.DataTransferObjects
{
    public class PumpedVolumeDto
    {
        public PumpedVolumeTimePoint[] AveragePumpedVolumeTimeSeries { get; set; }
    }

    public class PumpedVolumeTimePoint
    {
        public DateTime Time { get; set; }
        public decimal PumpedVolume { get; set; }
    }
}
