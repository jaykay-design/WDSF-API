namespace Wdsf.Api.Client.Converter
{
    using Newtonsoft.Json;
    using System;
    using System.Globalization;

    /// <summary>
    /// Writes an int property as a JSON string, for fields the v2 API types as text (a team
    /// member's MIN), and reads either a number or a string.
    /// </summary>
    internal sealed class IntAsStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(int);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return 0;
            }

            var text = Convert.ToString(reader.Value, CultureInfo.InvariantCulture);
            return int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value) ? value : 0;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((int)value).ToString(CultureInfo.InvariantCulture));
        }
    }
}
