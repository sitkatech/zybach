using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Zybach.Models.GeoOptix.SampleMethod
{
    public class VisibilityRuleConditionConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            FieldInstance result = null;
            if (serializer.Deserialize<JObject>(reader) is JObject obj)
            {
                var property = obj.Properties().FirstOrDefault();
                if (property != null)
                {
                    result = new FieldInstance
                    {
                        CanonicalName = property.Name,
                        RawValue = property.Value
                    };
                }
            }
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            serializer.DateTimeZoneHandling = DateTimeZoneHandling.Local;

            if (value is FieldInstance property)
            {
                var obj = new JObject
                {
                    [property.CanonicalName] = JToken.FromObject(property.RawValue)
                };
                serializer.Serialize(writer, obj);
            }
        }

        public override bool CanConvert(Type t)
        {
            return typeof(FieldInstance).IsAssignableFrom(t);
        }

        public override bool CanRead => true;
    }
}
