namespace Wdsf.Api.Client.Converter
{
    using Newtonsoft.Json;
    using System;
    using System.Globalization;

    /// <summary>
    /// Writes a string property holding a number as a JSON number (the v2 API rejects numbers
    /// sent as strings) and reads either a number or a string. A value that is not a number is
    /// written as a string.
    /// </summary>
    internal sealed class NumericStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            return Convert.ToString(reader.Value, CultureInfo.InvariantCulture);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var text = value as string;
            if (string.IsNullOrWhiteSpace(text))
            {
                writer.WriteNull();
            }
            else if (long.TryParse(text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var whole))
            {
                writer.WriteValue(whole);
            }
            else if (decimal.TryParse(text.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture, out var number))
            {
                writer.WriteValue(number);
            }
            else
            {
                writer.WriteValue(text);
            }
        }
    }
}
