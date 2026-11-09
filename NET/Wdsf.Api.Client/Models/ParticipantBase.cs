namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    public class ParticipantBase : EntityWithLinks
    {
        [XmlIgnore]
        [JsonIgnore]
        public int CompetitionId { get; set; }

        [XmlElement("id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Present, Noshow, Excused, Disqualified, Entered, ...
        /// </summary>
        [XmlElement("status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// Whether the entry has a confirmed WDSF coach for the competition's discipline.
        /// </summary>
        [XmlElement("coach")]
        [JsonProperty("coach")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public ParticipantCoachStatus Coach { get; set; }
    }
}
