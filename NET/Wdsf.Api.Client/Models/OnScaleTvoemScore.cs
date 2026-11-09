namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public sealed class OnScaleTvoemScore : Score
    {
        /// <summary>
        /// The kind the v2 API uses for this score (the legacy v1 API used <see cref="LegacySerializerTypeName"/>).
        /// </summary>
        public const string SerializerTypeName = "hipHop";
        /// <summary>
        /// The kind the legacy v1 API used for this score. Still accepted when reading JSON.
        /// </summary>
        public const string LegacySerializerTypeName = "onScaleTvoem";
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
