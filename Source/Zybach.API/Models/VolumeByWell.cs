using System.Collections.Generic;

namespace Zybach.API.Models
{
    public class VolumeByWell
    {
        public string WellRegistrationID { get; set; }
        public int IntervalCount => IntervalVolumes?.Count ?? 0;
        public List<IntervalVolumeDto> IntervalVolumes { get; set; }
    }
}