using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Zybach.Models.GeoOptix.SampleMethod
{
    public class NotMeasuredFieldInstanceConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            NotMeasuredFieldInstance result = null;
            if (serializer.Deserialize<JObject>(reader) is JObject obj)
            {
                var property = obj.Properties().FirstOrDefault();
                if (property != null)
                {
                    result = new NotMeasuredFieldInstance
                    {
                        Name = property.Name,
                        RawValue = property.Value.ToObject<NotMeasured>()
                    };
                }
            }
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            serializer.DateTimeZoneHandling = DateTimeZoneHandling.Local;

            if (value is NotMeasuredFieldInstance property)
            {
                var obj = new JObject
                {
                    [property.Name] = JToken.FromObject(property.RawValue)
                };
                serializer.Serialize(writer, obj);
            }
        }

        public override bool CanConvert(Type t)
        {
            return typeof(NotMeasuredFieldInstance).IsAssignableFrom(t);
        }

        public override bool CanRead => true;
    }
}
