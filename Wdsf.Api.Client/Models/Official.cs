namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("official", Namespace = "http://services.worlddancesport.org/api")]
    public class Official
    {
        [XmlElement("link")]
        public Link[] Link { get; set; }

        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("country")]
        public string Nationality { get; set; }
    }
}
