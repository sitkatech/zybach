using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Zybach.Models.GeoOptix.SampleMethod
{
    public class NotMeasuredRecordInstanceConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new NotMeasuredRecordInstance();
            if (serializer.Deserialize<JObject>(reader) is JObject obj)
            {
                var properties = obj.Properties();
                foreach (var property in properties)
                {
                    result.Fields.Add(new NotMeasuredFieldInstance
                    {
                        Name = property.Name,
                        RawValue = property.Value.ToObject<NotMeasured>()
                    });
                }
            }
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            serializer.DateTimeZoneHandling = DateTimeZoneHandling.Local;

            if (value is NotMeasuredRecordInstance definition)
            {
                var obj = new JObject();
                foreach (var definitionValue in definition.Fields)
                {
                    var objectToAdd = definitionValue.RawValue != null
                        ? JToken.FromObject(definitionValue.RawValue)
                        : null;

                    obj.Add(definitionValue.Name, objectToAdd);
                }
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
