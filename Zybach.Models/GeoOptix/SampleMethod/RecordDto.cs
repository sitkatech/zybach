namespace Zybach.Models.GeoOptix.SampleMethod
{
    public class RecordDto
    {
        public string RecordSetCanonicalName { get; set; }
        public string RecordKey { get; set; }

        public RecordInstance RecordInstance { get; set; }

        public NotMeasuredRecordInstance NotMeasuredRecordInstance { get; set; }

        public NotMeasured NotMeasured { get; set; }
        public string Hash { get; set; }

        public RecordDto(Record record, string recordSetCanonicalName)
        {
            RecordSetCanonicalName = recordSetCanonicalName;
            RecordKey = record.RecordKey;
            RecordInstance = record.RecordInstance;
            NotMeasuredRecordInstance = record.NotMeasuredRecordInstance;
            NotMeasured = record.NotMeasured;
            Hash = record.Hash;
        }
    }
}