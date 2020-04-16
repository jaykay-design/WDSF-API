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

        [XmlElement("division")]
        [JsonProperty("division")]
        public string Division { get; set; }

        [XmlArray("members")]
        [JsonProperty("members")]
        public List<TeamMember> Members { get; set; }

        public bool ShouldSerializeMembers()
        {
            return Members != null && Members.Count > 0;
        }

    }
}
