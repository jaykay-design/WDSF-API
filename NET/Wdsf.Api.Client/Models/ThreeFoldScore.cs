namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType("threefold", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("threefold")]
    public sealed class ThreeFoldScore : Score
    {
        [XmlIgnore]
        [JsonProperty("kind")]
        public override string Kind { get { return "threefold"; } }

        /// <summary>
        /// Seed, RoundRobin, TopX
        /// </summary>
        [XmlAttribute("mode")]
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [XmlAttribute("branch")]
        [JsonProperty("branch")]
        public int Branch { get; set; }

        [XmlAttribute("subround")]
        [JsonProperty("subround")]
        public int SubRound { get; set; }


        [XmlAttribute("physical")]
        [JsonProperty("physical")]
        public int Physical { get; set; }

        [XmlAttribute("interpretation")]
        [JsonProperty("interpretation")]
        public int Interpretation { get; set; }

        [XmlAttribute("artistic")]
        [JsonProperty("artistic")]
        public int Artistic { get; set; }

        [XmlAttribute("isred")]
        [JsonProperty("isred")]
        public bool IsRed { get; set; }
    }
}
