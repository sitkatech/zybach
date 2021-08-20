using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zybach.Models.GeoOptix.SampleMethod
{
    [JsonConverter(typeof(RecordInstanceConverter))]
    public class RecordInstance
    {
        public List<FieldInstance> Fields { get; set; }
        public RecordInstance()
        {
            Fields = new List<FieldInstance>();
        }
    }
}