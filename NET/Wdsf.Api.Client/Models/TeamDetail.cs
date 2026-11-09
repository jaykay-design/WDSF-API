namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("team", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("team", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.team")]
    [JsonObject("team")]
    public class TeamDetail : TeamBase
    {
        [XmlElement("status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// The team kind (e.g. Formation, Crew).
        /// </summary>
        [XmlElement("kind")]
        [JsonProperty("kind")]
        public string Kind { get; set; }

        /// <summary>
        /// The Hip Hop team kind.
        /// </summary>
        [XmlElement("kindHipHop")]
        [JsonProperty("kindHipHop")]
        public string KindHipHop { get; set; }

        [XmlElement("size")]
        [JsonProperty("size")]
        public string Size { get; set; }

        [XmlElement("division")]
        [JsonProperty("division")]
        public string Division { get; set; }

        [XmlElement("age")]
        [JsonProperty("age")]
        public string AgeGroup { get; set; }

        [XmlElement("retiredOn")]
        [JsonProperty("retireOn")]
        public string RetiredOn { get; set; }
        public bool ShouldSerializeRetiredOn()
        {
            return !string.IsNullOrEmpty(RetiredOn);
        }

        [XmlArray("members")]
        [JsonProperty("members")]
        public List<TeamMember> Members { get; set; }

        public bool ShouldSerializeMembers()
        {
            return Members != null && Members.Count > 0;
        }

    }
}
