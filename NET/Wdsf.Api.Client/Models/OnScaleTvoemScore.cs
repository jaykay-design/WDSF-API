namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public sealed class OnScaleTvoemScore : Score
    {
        public const string SerializerTypeName = "onScaleTvoem";
        /// <summary>
        /// Technique
        /// </summary>
        [XmlAttribute("t")]
        [JsonProperty("t")]
        public decimal T { get; set; }
        /// <summary>
        /// Vocabulary
        /// </summary>
        [XmlAttribute("v")]
        [JsonProperty("v")]
        public decimal V { get; set; }
        /// <summary>
        /// Originality
        /// </summary>
        [XmlAttribute("o")]
        [JsonProperty("o")]
        public decimal O { get; set; }
        /// <summary>
        /// Execution
        /// </summary>
        [XmlAttribute("e")]
        [JsonProperty("e")]
        public decimal E { get; set; }
        /// <summary>
        /// Musicality
        /// </summary>
        [XmlAttribute("m")]
        [JsonProperty("m")]
        public decimal M { get; set; }
    }
}
