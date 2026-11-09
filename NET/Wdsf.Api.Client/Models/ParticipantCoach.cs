namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// A confirmed WDSF coach of a competition entry, for the competition's discipline.
    /// </summary>
    [XmlType("coach")]
    [JsonObject("coach")]
    public class ParticipantCoach
    {
        /// <summary>
        /// The link to the coach's person (rel coach/person).
        /// </summary>
        [XmlElement("link")]
        [JsonProperty("link")]
        public List<Link> Links { get; set; }

        [XmlElement("min")]
        [JsonProperty("min")]
        public int Min { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlElement("surname")]
        [JsonProperty("surname")]
        public string Surname { get; set; }

        /// <summary>
        /// The discipline the coach is linked for (its xml name).
        /// </summary>
        [XmlElement("discipline")]
        [JsonProperty("discipline")]
        public string Discipline { get; set; }

        /// <summary>
        /// True when the coach is an adjudicator of a competition in the same event group and may
        /// not act as coach there.
        /// </summary>
        [XmlElement("blocked")]
        [JsonProperty("blocked")]
        public bool Blocked { get; set; }
    }
}
