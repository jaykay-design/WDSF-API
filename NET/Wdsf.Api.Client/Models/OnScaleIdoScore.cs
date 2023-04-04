namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public class OnScaleIdoScore : Score
    {
        public const string SerializerTypeName = "onScaleIdo";

        [XmlIgnore]
        [JsonProperty("kind")]
        public override string Kind => SerializerTypeName;

        /// <summary>
        /// Technique
        /// </summary>
        [XmlAttribute("t")]
        [JsonProperty("t")]
        public virtual decimal T { get; set; }
        /// <summary>
        /// Composition
        /// </summary>
        [XmlAttribute("c")]
        [JsonProperty("c")]
        public virtual decimal C { get; set; }
        /// <summary>
        /// Image
        /// </summary>
        [XmlAttribute("i")]
        [JsonProperty("i")]
        public virtual decimal I { get; set; }
        /// <summary>
        /// Show
        /// </summary>
        [XmlAttribute("s")]
        [JsonProperty("s")]
        public virtual decimal S { get; set; }
        /// <summary>
        /// Reduction by Chairman
        /// </summary>
        [XmlAttribute("reduction")]
        [JsonProperty("reduction")]
        public virtual decimal Reduction { get; set; }
    }
}
