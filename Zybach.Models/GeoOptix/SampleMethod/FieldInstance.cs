using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Zybach.Models.GeoOptix.SampleMethod
{
    [JsonConverter(typeof(VisibilityRuleConditionConverter))]
    public class FieldInstance
    {
        [DisplayName("Name"), Required]
        public string CanonicalName { get; set; }

        [DisplayName("RawValue"), Required]
        public JToken RawValue { get; set; }

        public FieldInstance()
        {
            
        }

        public FieldInstance(string canonicalName, JToken rawValue)
        {
            CanonicalName = canonicalName;
            RawValue = rawValue;
        }
    }

    [JsonConverter(typeof(NotMeasuredFieldInstanceConverter))]
    public class NotMeasuredFieldInstance
    {
        [DisplayName("Name"), Required]
        public string Name { get; set; }

        [DisplayName("RawValue"), Required]
        public NotMeasured RawValue { get; set; }
    }
}