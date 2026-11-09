namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public sealed class OnScaleMtcScore : Score
    {
        public const string SerializerTypeName = "onScaleMtc";
        /// <summary>
        /// Technique
        /// </summary>
        [XmlAttribute("t")]
        [JsonProperty("t")]
        public decimal T { get; set; }
        /// <summary>
        /// Musicality
        /// </summary>
        [XmlAttribute("m")]
        [JsonProperty("m")]
        public decimal M { get; set; }
        /// <summary>
        /// Creativity
        /// </summary>
        [XmlAttribute("c")]
        [JsonProperty("c")]
        public decimal C { get; set; }
    }
}
