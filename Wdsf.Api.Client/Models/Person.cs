namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("person", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("person", Namespace = "http://services.worlddancesport.org/api")]
    public class Person
    {
        [XmlElement("link")]
        public Link[] Link  { get; set; }

        [XmlElement("id")]
        public string Min { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("country")]
        public string Country { get; set; }

        [XmlElement("activePartner")]
        public string ActivePartner { get; set; }

        [XmlElement("activeCoupleId")]
        public string ActiveCoupleId { get; set; }

        [XmlElement("activeCoupleAgeGroup")]
        public string AgeGroup { get; set; }
    }
}
