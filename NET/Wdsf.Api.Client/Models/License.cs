namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType("license", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("link")]
    public class License
    {
        [XmlIgnore,JsonIgnore]
        public int Id { get; set; }

        [XmlElement("type")]
        [JsonProperty("type")]
        public string Type { get; set; }

        [XmlElement("status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        [XmlElement("division")]
        [JsonProperty("division")]
        public string Division { get; set; }

        [XmlElement("expiresOn")]
        [JsonProperty("expiresOn")]
        public string ExpiresOn { get; set; }

        [XmlElement("wrlBLockedUntil")]
        [JsonProperty("wrlBLockedUntil")]
        public string WrlBlockedUntil { get; set; }

        [XmlElement("cupOrChampionshipBlockedUntil")]
        [JsonProperty("cupOrChampionshipBlockedUntil")]
        public string CupOrChampionshipBlockedUntil { get; set; }
    }
}