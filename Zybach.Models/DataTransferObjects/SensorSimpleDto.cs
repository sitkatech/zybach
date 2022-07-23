using System;
using System.Collections.Generic;

namespace Zybach.Models.DataTransferObjects
{
    public partial class SensorSimpleDto
    {
        public string WellRegistrationID { get; set; }
        public string SensorTypeName { get; set; }
        public int? MessageAge { get; set; }
        public double? LastVoltageReading { get; set; }
        public DateTime? LastVoltageReadingDate { get; set; }
        public DateTime? FirstReadingDate { get; set; }
        public DateTime? LastReadingDate { get; set; }
        public int? WellPageNumber { get; set; }
        public string WellOwnerName { get; set; }
        public string WellTownshipRangeSection { get; set; }
        public int? MostRecentSupportTicketID { get; set; }
        public string MostRecentSupportTicketTitle { get; set; }
        public ContinuityMeterStatusDto ContinuityMeterStatus { get; set; }
        public DateTime? LastOnReadingDate { get; set; }
        public DateTime? LastOffReadingDate { get; set; }
    }
}
