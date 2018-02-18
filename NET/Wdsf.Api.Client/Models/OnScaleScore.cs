namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType("onScale", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("onScale")]
    public class OnScaleScore : Score
    {
        [XmlIgnore]
        [JsonProperty("kind")]
        public override string Kind { get { return "onScale"; } set { ; } }

        /// <summary>
        /// Posture Balance Coorination
        /// </summary>
        [XmlAttribute("pb")]
        [JsonProperty("pb")]
        public virtual decimal PB { get; set; }
        /// <summary>
        /// Quality of movement
        /// </summary>
        [XmlAttribute("qm")]
        [JsonProperty("qm")]
        public virtual decimal QM { get; set; }
        /// <summary>
        /// Movement to music
        /// </summary>
        [XmlAttribute("mm")]
        [JsonProperty("mm")]
        public virtual decimal MM { get; set; }
        /// <summary>
        /// Partnering skill
        /// </summary>
        [XmlAttribute("ps")]
        [JsonProperty("ps")]
        public virtual decimal PS { get; set; }
        /// <summary>
        /// Choreography and Presentation
        /// </summary>
        [XmlAttribute("cp")]
        [JsonProperty("cp")]
        public virtual decimal CP { get; set; }
        /// <summary>
        /// Reduction by Chairman
        /// </summary>
        [XmlAttribute("reduction")]
        [JsonProperty("reduction")]
        public virtual decimal Reduction { get; set; }
    }
}
