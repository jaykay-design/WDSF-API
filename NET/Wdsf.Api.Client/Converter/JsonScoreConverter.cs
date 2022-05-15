namespace Wdsf.Api.Client.Converter
{
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;

    class JsonScoreConverter : JsonConverter
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

            var jObject = JObject.Load(reader);

            switch (jObject["kind"].Value<string>())
            {
                case "onScale3": return jObject.ToObject<OnScale3Score>(serializer);
                case "onScale2": return jObject.ToObject<OnScale2Score>(serializer);
                case "onScale": return jObject.ToObject<OnScaleScore>(serializer);
                case "onScaleIdo": return jObject.ToObject<OnScaleIdoScore>(serializer);
                //                case "nomark": return jObject.ToObject<NoMarkScore>(serializer);
                case "mark": return jObject.ToObject<MarkScore>(serializer);
                case "final": return jObject.ToObject<FinalScore>(serializer);
                case "trivium": return jObject.ToObject<TriviumScore>(serializer);
                case "threefold": return jObject.ToObject<ThreeFoldScore>(serializer);
                default: throw new NotImplementedException($"No JSON reader for type '{objectType.FullName}' implemented.");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
