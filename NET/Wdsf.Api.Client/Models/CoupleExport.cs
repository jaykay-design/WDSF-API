namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("couple", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot(Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.couple.export")]
    public class CoupleExport : CoupleBase
    {
        [XmlElement("displayName")]
        public string Name { get; set; }

        [XmlElement("manName")]
        public string ManName { get; set; }

        [XmlElement("manSurname")]
        public string ManSurname { get; set; }

        [XmlElement("manNationality")]
        public string ManNationality { get; set; }

        [XmlElement("womanName")]
        public string WomanName { get; set; }

        [XmlElement("womanSurname")]
        public string WomanSurname { get; set; }

        [XmlElement("womanNationality")]
        public string WomanNationality { get; set; }

        [XmlElement("country")]
        public string Country { get; set; }

        [XmlElement("manMin")]
        public int ManMin { get; set; }

        [XmlElement("womanMin")]
        public int WomanMin { get; set; }

        [XmlElement("division")]
        public string Division { get; set; }

        [XmlElement("agegroup")]
        public string AgeGroup { get; set; }
    }
}
