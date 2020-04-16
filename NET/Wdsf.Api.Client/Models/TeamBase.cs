namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    public class TeamBase : EntityWithLinks
    {
        [XmlElement("id")]
        [JsonProperty("id")]
        public virtual int Id { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }
    }
}
