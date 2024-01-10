using System.Collections.Generic;

namespace Zybach.Swagger.Models
{
    public class StructuredResults
    {
        public int IntervalCountTotal { get; set; }
        public string IntervalStart { get; set; }
        public string IntervalEnd { get; set; }
        public int WellCount { get; set; }
        public List<VolumeByWell> VolumesByWell { get; set; }
    }
}