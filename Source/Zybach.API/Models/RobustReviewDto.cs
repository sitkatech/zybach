using System.Collections.Generic;
using Zybach.Models.DataTransferObjects;

namespace Zybach.API.Models
{
    public class RobustReviewDto
    {
        public string WellRegistrationID { get; set; }
        public string WellTPID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DataSource { get; set; }
        public List<MonthlyPumpedVolume> MonthlyPumpedVolumeGallons { get; set; }
    }
}