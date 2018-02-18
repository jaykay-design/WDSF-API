namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Wdsf.Api.Client.Attributes;

    [XmlType("participant", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot(Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("participant")]
    [MediaType("application/vnd.worlddancesport.participant.team")]
    public class ParticipantTeamDetail : ParticipantBaseDetail
    {
        [XmlElement("link")]
        [JsonProperty("link")]
        public override Link[] Link { get; set; }

        [XmlElement("teamId")]
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Team { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }
    }
}
