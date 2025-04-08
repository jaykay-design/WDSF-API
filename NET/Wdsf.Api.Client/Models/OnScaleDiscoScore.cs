namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public sealed class OnScaleDiscoScore : Score
    {
        public const string SerializerTypeName = "onScaleDisco";
        /// <summary>
        /// Technique
        /// </summary>
        [XmlAttribute("t")]
        [JsonProperty("t")]
        public decimal T { get; set; }
        /// <summary>
        /// Performance
        /// </summary>
        [XmlAttribute("p")]
        [JsonProperty("p")]
        public decimal P { get; set; }
        /// <summary>
        /// Choreography
        /// </summary>
        [XmlAttribute("c")]
        [JsonProperty("c")]
        public decimal C { get; set; }
    }
}
