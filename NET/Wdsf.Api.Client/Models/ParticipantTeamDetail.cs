namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("participant", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot(Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("participant")]
    [MediaType("application/vnd.worlddancesport.participant.team")]
    public class ParticipantTeamDetail : ParticipantBaseDetail
    {
        [XmlElement("teamId")]
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        /// <summary>
        /// The team's name. In JSON the API sends it as "team".
        /// </summary>
        [XmlElement("name")]
        [JsonProperty("team")]
        public string Team { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// The performance name, null when none was entered. Read only.
        /// </summary>
        [XmlElement("performanceName")]
        [JsonProperty("performanceName")]
        public string PerformanceName { get; set; }
        public bool ShouldSerializePerformanceName()
        {
            return false;
        }

        /// <summary>
        /// The athletes of the entry. Read only.
        /// </summary>
        [XmlArray("athletes")]
        [XmlArrayItem(AthleteInfo.XmlItemName)]
        [JsonProperty("athletes")]
        public List<AthleteInfo> Athletes { get; set; }
        public bool ShouldSerializeAthletes()
        {
            return false;
        }
    }
}
