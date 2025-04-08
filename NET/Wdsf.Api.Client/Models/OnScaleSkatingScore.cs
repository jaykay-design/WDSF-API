namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public sealed class OnScaleSkatingScore : Score
    {
        public const string SerializerTypeName = "onScaleSkating";
        /// <summary>
        /// Technical quality
        /// </summary>
        [XmlAttribute("tq")]
        [JsonProperty("tq")]
        public decimal TQ { get; set; }
        /// <summary>
        /// Movement to music
        /// </summary>
        [XmlAttribute("mm")]
        [JsonProperty("mm")]
        public decimal MM { get; set; }
        /// <summary>
        /// Partnering skill
        /// </summary>
        [XmlAttribute("ps")]
        [JsonProperty("ps")]
        public decimal PS { get; set; }
        /// <summary>
        /// Choreography and Presentation
        /// </summary>
        [XmlAttribute("cp")]
        [JsonProperty("cp")]
        public decimal CP { get; set; }

        /// <summary>
        /// Reduction by Chairman
        /// </summary>
        [XmlAttribute("reduction")]
        [JsonProperty("reduction")]
        public decimal Reduction { get; set; }

        /// <summary>
        /// If true this participant was allowed to skip the round
        /// </summary>
        [XmlAttribute("set")]
        [JsonProperty("set")]
        public bool IsSet { get; set; }
    }
}
