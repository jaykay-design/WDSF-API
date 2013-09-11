namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;

    [XmlType("onScaleIdo", Namespace = "http://services.worlddancesport.org/api")]
    public class OnScaleIdoScore : Score
    {
        /// <summary>
        /// Technique
        /// </summary>
        [XmlAttribute("t")]
        public virtual decimal T { get; set; }
        /// <summary>
        /// Composition
        /// </summary>
        [XmlAttribute("c")]
        public virtual decimal C { get; set; }
        /// <summary>
        /// Image
        /// </summary>
        [XmlAttribute("i")]
        public virtual decimal I { get; set; }
        /// <summary>
        /// Show
        /// </summary>
        [XmlAttribute("s")]
        public virtual decimal S { get; set; }
        /// <summary>
        /// Reduction by Chairman
        /// </summary>
        [XmlAttribute("reduction")]
        public virtual decimal Reduction { get; set; }
    }
}
