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
    }
}
