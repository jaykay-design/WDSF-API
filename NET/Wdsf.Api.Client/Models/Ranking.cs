namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("rank", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.rank")]
    [JsonObject("rank")]
    public class Ranking
    {
        [XmlElement("link")]
        [JsonProperty("link")]
        public Link[] Links { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Couple { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        [XmlElement("rank")]
        [JsonProperty("rank")]
        public int Rank { get; set; }

        [XmlElement("points")]
        [JsonProperty("points")]
        public int Points { get; set; }

        [XmlElement("manMin")]
        [JsonProperty("manMin")]
        public string ManMin { get; set; }

        [XmlElement("womanMin")]
        [JsonProperty("womanMin")]
        public string WomanMin { get; set; }

        [XmlElement("coupleId")]
        [JsonProperty("coupleId")]
        public string Id { get; set; }
    }
}
