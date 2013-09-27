namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;

    [XmlType("onScale2", Namespace = "http://services.worlddancesport.org/api")]
    public class OnScale2Score : Score
    {
        /// <summary>
        /// Techincal Quality
        /// </summary>
        [XmlAttribute("tq")]
        public virtual decimal TQ { get; set; }
        /// <summary>
        /// Movement to music
        /// </summary>
        [XmlAttribute("mm")]
        public virtual decimal MM { get; set; }
        /// <summary>
        /// Partnering skill
        /// </summary>
        [XmlAttribute("ps")]
        public virtual decimal PS { get; set; }
        /// <summary>
        /// Choreography and Presentation
        /// </summary>
        [XmlAttribute("cp")]
        public virtual decimal CP { get; set; }
        /// <summary>
        /// Reduction by Chairman
        /// </summary>
        [XmlAttribute("reduction")]
        public virtual decimal Reduction { get; set; }
        /// <summary>
        /// This participant was allowed to skip the round it set to true
        /// </summary>
        [XmlAttribute("set")]
        public virtual bool IsSet { get; set; }

    }
}
