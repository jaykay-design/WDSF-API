namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("competition", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("competition", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.competition")]
    public class CompetitionDetail
    {
        [XmlElement("link")]
        public Link[] Links { get; set; }

        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("location")]
        public string Location { get; set; }

        [XmlElement("country")]
        public string Country { get; set; }

        [XmlElement("type")]
        public string CompetitionType { get; set; }

        [XmlElement("date")]
        public DateTime Date { get; set; }

        [XmlElement("age")]
        public string AgeClass { get; set; }

        [XmlElement("discipline")]
        public string Discipline { get; set; }

        [XmlElement("division")]
        public string Division { get; set; }

        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("coefficient")]
        public decimal Coefficient { get; set; }

        [XmlElement("lastmodifiedDate")]
        public DateTime LastModifiedDate { get; set; }

    }
}