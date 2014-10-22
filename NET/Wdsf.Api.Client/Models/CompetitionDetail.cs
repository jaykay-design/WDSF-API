namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Wdsf.Api.Client.Attributes;

    [XmlType("competition", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("competition", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("competition")]
    [MediaType("application/vnd.worlddancesport.competition")]
    public class CompetitionDetail
    {
        [XmlElement("link")]
        [JsonProperty("link")]
        public Link[] Links { get; set; }

        [XmlElement("id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [XmlElement("location")]
        [JsonProperty("location")]
        public string Location { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        [XmlElement("type")]
        [JsonProperty("type")]
        public string CompetitionType { get; set; }

        [XmlElement("date")]
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [XmlElement("age")]
        [JsonProperty("age")]
        public string AgeClass { get; set; }

        [XmlElement("discipline")]
        [JsonProperty("discipline")]
        public string Discipline { get; set; }

        [XmlElement("division")]
        [JsonProperty("division")]
        public string Division { get; set; }

        [XmlElement("status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        [XmlElement("coefficient")]
        [JsonProperty("coefficient")]
        public decimal Coefficient { get; set; }

        [XmlElement("lastmodifiedDate")]
        [JsonProperty("lastmodifiedDate")]
        public DateTime LastModifiedDate { get; set; }

    }
}