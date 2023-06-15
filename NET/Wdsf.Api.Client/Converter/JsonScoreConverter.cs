namespace Wdsf.Api.Client.Converter
{
    using Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;

    class JsonScoreConverter : JsonConverter
    {
        private static readonly Dictionary<string, Type> typeMap = new Dictionary<string, Type>()
            {
                { WdsfBreakingScore.SerializerTypeName, typeof(WdsfBreakingScore) },
                { TriviumScore.SerializerTypeName,     typeof(TriviumScore) },
                { ThreeFoldScore.SerializerTypeName, typeof(ThreeFoldScore) },
                { BreakingSeedScore.SerializerTypeName, typeof(BreakingSeedScore) },
                { BreakingSeedByScoreScore.SerializerTypeName, typeof(BreakingSeedByScoreScore) },

                { OnScaleScore.SerializerTypeName, typeof(OnScaleScore) },
                { OnScale2Score.SerializerTypeName, typeof(OnScale2Score) },
                { OnScale3Score.SerializerTypeName, typeof(OnScale3Score) },
                { OnScaleIdoScore.SerializerTypeName, typeof(OnScaleIdoScore) },
                { MarkScore.SerializerTypeName, typeof(MarkScore) },
                { FinalScore.SerializerTypeName, typeof(FinalScore) }
            };

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
            var serializedTypeName = jObject["kind"].Value<string>();

            if (!typeMap.ContainsKey(serializedTypeName))
            {
                throw new NotImplementedException(string.Format("No JSON reader for type {0} implemented.", objectType.FullName));
            }

            return jObject.ToObject(typeMap[serializedTypeName], serializer);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
