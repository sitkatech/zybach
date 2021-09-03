using Newtonsoft.Json;

namespace Zybach.Models.GeoOptix.SampleMethod
{
    public class Record
    {
        public string RecordKey { get; set; }

        [JsonProperty(PropertyName = "Fields")]
        public RecordInstance RecordInstance { get; set; }

        [JsonProperty(PropertyName = "NotMeasuredFields")]
        public NotMeasuredRecordInstance NotMeasuredRecordInstance { get; set; }

        public NotMeasured NotMeasured { get; set; }
        public string Hash { get; set; }

        public Record()
        {
            RecordInstance = new RecordInstance();
        }

        public Record(string recordKey) : this()
        {
            RecordKey = recordKey;
        }
    }
}