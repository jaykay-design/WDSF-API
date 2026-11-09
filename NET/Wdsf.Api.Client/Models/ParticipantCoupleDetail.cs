namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("participant", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot(Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("participant")]
    [MediaType("application/vnd.worlddancesport.participant.couple")]
    public class ParticipantCoupleDetail : ParticipantBaseDetail
    {
        [XmlElement("coupleId")]
        [JsonProperty("coupleId")]
        public string CoupleId { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// The reference a member federation stores for the couple. Read only.
        /// </summary>
        [XmlElement("nationalreference")]
        [JsonProperty("nationalreference")]
        public string NationalReference { get; set; }
        public bool ShouldSerializeNationalReference()
        {
            return false;
        }

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
