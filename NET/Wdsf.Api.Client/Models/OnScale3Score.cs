namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType("onScale3", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("onScale3")]
    public class OnScale3Score : Score
    {
        public override string Kind { get { return "onScale3"; } set { ; } }

        /// <summary>
        /// Techincal Quality
        /// </summary>
        [XmlAttribute("tq")]
        [JsonProperty("tq")]
        public virtual decimal TQ { get; set; }
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
        /// <summary>
        /// This participant was allowed to skip the round it set to true
        /// </summary>
        [XmlAttribute("set")]
        [JsonProperty("set")]
        public virtual bool IsSet { get; set; }

    }
}
