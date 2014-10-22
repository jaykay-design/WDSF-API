namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    public class ParticipantBase
    {
        [XmlElement("link")]
        [JsonProperty("link")]
        public virtual Link[] Link { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public int CompetitionId { get; set; }

        [XmlElement("id")]
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
