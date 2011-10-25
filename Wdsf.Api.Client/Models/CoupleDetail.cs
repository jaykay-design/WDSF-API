namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("couple", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot(Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.couple")]
    public class CoupleDetail : CoupleBase
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("country")]
        public string Country { get; set; }

        [XmlElement("age")]
        public string AgeGroup { get; set; }

        [XmlElement("division")]
        public string Division { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("man")]
        public int ManMin { get; set; }

        [XmlElement("customManName")]
        public string UnknownManName { get; set; }

        [XmlElement("woman")]
        public int WomanMin { get; set; }

        [XmlElement("customWomanName")]
        public string UnknownWomanName { get; set; }

        [XmlIgnore]
        public bool UnknownWomanNameSpecified { get { return !string.IsNullOrEmpty(UnknownWomanName); } set { } }

        [XmlIgnore]
        public bool UnknownManNameSpecified { get { return !string.IsNullOrEmpty(UnknownManName); } set { } }

        [XmlIgnore]
        public bool ManMinSpecified { get { return ManMin != 0; } set { } }

        [XmlIgnore]
        public bool WomanMinSpecified { get { return WomanMin != 0; } set { } }
    }
}
