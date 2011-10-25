namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("official", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("official", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.official")]
    public class OfficialDetail
    {
        [XmlElement("link")]
        public Link[] Link { get; set; }

        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("name")]
        public string Person { get; set; }

        [XmlElement("country")]
        public string Nationality { get; set; }

        [XmlElement("task")]
        public string Task { get; set; }

        [XmlElement("letter")]
        public string AdjudicatorChar { get; set; }

        [XmlElement("min")]
        public int Min { get; set; }

        [XmlElement("competitionId")]
        public int CompetitionId { get; set; }
    }
}
