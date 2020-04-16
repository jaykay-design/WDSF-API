namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
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
    }
}
