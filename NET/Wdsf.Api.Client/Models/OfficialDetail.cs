namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("official", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("official", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("official")]
    [MediaType("application/vnd.worlddancesport.official")]
    public class OfficialDetail
    {
        [XmlElement("link")]
        [JsonProperty("link")]
        public Link[] Link { get; set; }

        [XmlElement("id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Person { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Nationality { get; set; }

        [XmlElement("task")]
        [JsonProperty("task")]
        public string Task { get; set; }

        [XmlElement("letter")]
        [JsonProperty("letter")]
        public string AdjudicatorChar { get; set; }

        [XmlElement("min")]
        [JsonProperty("min")]
        public int Min { get; set; }

        [XmlElement("competitionId")]
        [JsonProperty("competitionId")]
        public int CompetitionId { get; set; }
    }
}
