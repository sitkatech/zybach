using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zybach.Models.GeoOptix.SampleMethod
{
    [JsonConverter(typeof(NotMeasuredRecordInstanceConverter))]
    public class NotMeasuredRecordInstance
    {
        public NotMeasuredRecordInstance()
        {
            Fields = new List<NotMeasuredFieldInstance>();
        }

        public List<NotMeasuredFieldInstance> Fields { get; set; }
    }
}