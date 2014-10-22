namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType("onScaleIdo", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("onScaleIdo")]
    public class OnScaleIdoScore : Score
    {
        public override string Kind { get { return "onScaleIdo"; } set { ; } }

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
