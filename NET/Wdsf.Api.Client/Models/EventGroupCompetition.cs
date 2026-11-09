namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    /// <summary>
    /// A competition of an event group; its link leads to the competition.
    /// </summary>
    [XmlType("groupCompetition", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("competition")]
    public class EventGroupCompetition : EntityWithLinks
    {
        [XmlElement("id")]
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
