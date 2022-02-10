#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zybach.Models.DataTransferObjects
{
    public class WellInspectionSummaryDto
    {
        public int WellID { get; set; }
        public string WellRegistrationID { get; set; }
        public string? WellNickname { get; set; }
        public int? WellParticipationID { get; set; }
        public string? WellParticipationName { get; set; }

        public bool HasWaterLevelInspections { get; set; }
        public DateTime? LatestWaterLevelInspectionDate { get; set; }

        public bool HasWaterQualityInspections { get; set; }
        public DateTime? LatestWaterQualityInspectionDate { get; set; }
    }
}
