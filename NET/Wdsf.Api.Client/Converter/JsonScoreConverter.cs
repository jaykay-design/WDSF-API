namespace Wdsf.Api.Client.Converter
{
    using System;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Models;

    class JsonScoreConverter:JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsAssignableFrom(typeof(Score));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return new Score();
            }

            JObject jObject = JObject.Load(reader);

            switch (jObject["kind"].Value<string>())
            {
                case "onScale2": return jObject.ToObject<OnScale2Score>(serializer);
                case "onScale": return jObject.ToObject<OnScaleScore>(serializer);
                case "onScaleIdo": return jObject.ToObject<OnScaleIdoScore>(serializer);
//                case "nomark": return jObject.ToObject<NoMarkScore>(serializer);
                case "mark": return jObject.ToObject<MarkScore>(serializer);
                case "final": return jObject.ToObject<FinalScore>(serializer);
                default: throw new NotImplementedException(string.Format("No JSON reader for type {0} implemented.", objectType.FullName));
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
