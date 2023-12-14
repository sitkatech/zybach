using System.Collections.Generic;
using System.Linq;

namespace Zybach.Swagger.Models
{
    public class VolumeByWell
    {
        public string WellRegistrationID { get; set; }
        public int IntervalCount => IntervalVolumes?.Count ?? 0;
        public List<IntervalVolumeDto> IntervalVolumes { get; set; }

        public int TotalMeasurementValueGallonsForInterval => IntervalVolumes != null && IntervalVolumes.Any()
            ? IntervalVolumes.Sum(x => x.TotalMeasurementValueGallons)
            : 0;
    }
}