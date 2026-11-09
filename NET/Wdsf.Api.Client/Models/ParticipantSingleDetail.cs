namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
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
        [JsonConverter(typeof(Converter.NumericStringConverter))]
        public string PersonId { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

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
