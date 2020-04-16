namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("participant", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("participant", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("participant")]
    [MediaType("application/vnd.worlddancesport.participant.single")]
    public class ParticipantSingleDetail : ParticipantBaseDetail
    {
        [XmlElement("personId")]
        [JsonProperty("personId")]
        public string PersonId { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }
    }
}
