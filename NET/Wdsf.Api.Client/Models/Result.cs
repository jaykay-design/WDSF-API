namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType("result", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("result")]
    public class Result
    {
        [XmlElement("id")]
        [JsonProperty("id")]
        public int ParticipantId { get; set; }

        [XmlElement("rank")]
        [JsonProperty("rank", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Rank { get; set; }

        [XmlArray("rounds")]
        [JsonProperty("rounds")]
        public Round[] Rounds { get; set; }
    }
}
