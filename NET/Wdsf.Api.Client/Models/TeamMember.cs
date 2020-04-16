namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    [JsonObject("member")]
    [XmlType("member", Namespace = "http://services.worlddancesport.org/api")]
    public class TeamMember : EntityWithLinks
    {
        [XmlElement("id")]
        [JsonProperty("id")]
        public int Min { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlElement("nationality")]
        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [XmlElement("ageGroup")]
        [JsonProperty("ageGroup")]
        public string AgeGroup { get; set; }

        [XmlElement("status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        [XmlElement("expiresOn")]
        [JsonProperty("expiresOn")]
        public string ExpiresOn { get; set; }
    }
}
