namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    /// <summary>
    /// A row of a solo (single athlete) world ranking.
    /// </summary>
    [XmlType("rank", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("rank")]
    public class RankingSingle : EntityWithLinks
    {
        /// <summary>
        /// The athlete's MIN.
        /// </summary>
        [XmlElement("id")]
        [JsonProperty("id")]
        public int Min { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        [XmlElement("rank")]
        [JsonProperty("rank")]
        public int Rank { get; set; }

        [XmlElement("points")]
        [JsonProperty("points")]
        public int Points { get; set; }
    }
}
