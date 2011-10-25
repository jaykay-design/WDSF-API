namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;

    [XmlType("onScale", Namespace = "http://services.worlddancesport.org/api")]
    public class OnScaleScore : Score
    {
        /// <summary>
        /// Posture Balance Coorination
        /// </summary>
        [XmlAttribute("pb")]
        public virtual decimal PB { get; set; }
        /// <summary>
        /// Quality of movement
        /// </summary>
        [XmlAttribute("qm")]
        public virtual decimal QM { get; set; }
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
    }
}
