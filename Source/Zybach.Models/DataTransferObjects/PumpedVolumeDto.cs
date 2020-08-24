using System;
using System.Collections.Generic;
using System.Text;

namespace Zybach.Models.DataTransferObjects
{
    public class PumpedVolumeDto
    {
        public string WellRegistrationID { get; set; }
        public decimal AveragePumpedVolume { get; set; }
        public int ReportingIntervalMinutes { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
