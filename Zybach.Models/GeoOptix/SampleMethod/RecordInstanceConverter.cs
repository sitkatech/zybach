using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Zybach.Models.GeoOptix.SampleMethod
{
    public class RecordInstanceConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new RecordInstance();
            if (serializer.Deserialize<JObject>(reader) is JObject obj)
            {
                var properties = obj.Properties();
                foreach (var property in properties)
                {
                    result.Fields.Add(new FieldInstance
                    {
                        CanonicalName = property.Name,
                        RawValue = property.Value
                    });
                }
            }
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            serializer.DateTimeZoneHandling = DateTimeZoneHandling.Local;

            if (value is RecordInstance definition)
            {
                var obj = new JObject();
                foreach (var definitionValue in definition.Fields)
                {
                    var objectToAdd = definitionValue.RawValue != null
                        ? JToken.FromObject(definitionValue.RawValue)
                        : null;

                    obj.Add(definitionValue.CanonicalName, objectToAdd);
                }
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
