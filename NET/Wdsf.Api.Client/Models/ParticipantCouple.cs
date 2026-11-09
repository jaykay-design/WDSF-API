namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    [XmlType("participant", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("participant", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("participant")]
    public class ParticipantCouple : ParticipantBase
    {
        [XmlElement("name")]
        [JsonProperty("name")]
        public string Couple { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// The reference a member federation stores for the couple.
        /// </summary>
        [XmlElement("nationalreference")]
        [JsonProperty("nationalreference")]
        public string NationalReference { get; set; }

        [XmlElement("number")]
        [JsonProperty("number")]
        public string StartNumber { get; set; }
    }
}
