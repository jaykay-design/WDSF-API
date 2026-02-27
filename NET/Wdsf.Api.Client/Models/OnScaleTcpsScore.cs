namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public sealed class OnScaleTcpsScore : Score
    {
        public const string SerializerTypeName = "onScaleTcps";
        /// <summary>
        /// Technique
        /// </summary>
        [XmlAttribute("t")]
        [JsonProperty("t")]
        public decimal T { get; set; }
        /// <summary>
        /// Creativity
        /// </summary>
        [XmlAttribute("c")]
        [JsonProperty("c")]
        public decimal C { get; set; }
        /// <summary>
        /// Performance
        /// </summary>
        [XmlAttribute("p")]
        [JsonProperty("p")]
        public decimal P { get; set; }
        /// <summary>
        /// Show (leave empty for non-show dances)
        /// </summary>
        [XmlAttribute("s")]
        [JsonProperty("s")]
        public decimal S { get; set; }
    }
}
