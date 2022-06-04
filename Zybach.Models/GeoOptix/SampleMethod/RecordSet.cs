using System.Collections.Generic;

namespace Zybach.Models.GeoOptix.SampleMethod
{
    public class RecordSet
    {
        public string CanonicalName { get; set; }
        public List<Record> Records { get; set; }
        public NotMeasured NotMeasured { get; set; }
        public string Hash { get; set; }

        public RecordSet()
        {
            Records = new List<Record>();
        }

        public RecordSet(string canonicalName, List<Record> records)
        {
            CanonicalName = canonicalName;
            Records = records ?? new List<Record>();
        }
    }
}