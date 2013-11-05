namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("person", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("person", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.person")]
    public class PersonDetail 
    {
        [XmlElement("link")]
        public Link[] Link  { get; set; }

        [XmlElement("id")]
        public int Min { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("surname")]
        public string Surname { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("nationality")]
        public string Nationality { get; set; }

        [XmlElement("country")]
        public string Country { get; set; }

        [XmlElement("ageGroup")]
        public string AgeGroup { get; set; }

        [XmlArray("licenses")]
        public License[] Licenses { get; set; }
    }
}
