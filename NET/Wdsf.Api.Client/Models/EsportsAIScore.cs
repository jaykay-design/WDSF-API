namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    /// <summary>
    /// e-Sports AI score: one score per participant and dance, from a single judge.
    /// </summary>
    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public sealed class EsportsAIScore : Score
    {
        public const string SerializerTypeName = "esportsAI";
        /// <summary>
        /// The score from 0 to 100 with at most 3 decimals.
        /// </summary>
        [XmlAttribute("score")]
        [JsonProperty("score")]
        public decimal Value { get; set; }
    }
}
