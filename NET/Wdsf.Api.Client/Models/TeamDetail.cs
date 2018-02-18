namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;
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
    }
}
