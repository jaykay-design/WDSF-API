namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    [XmlType("official", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("official")]
    public class Official
    {
        [XmlElement("link")]
        [JsonProperty("link")]
        public Link[] Link { get; set; }

        [XmlElement("id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Nationality { get; set; }
    }
}
