namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public sealed class ThreeFoldScore : Score
    {
        public const string SerializerTypeName = "threefold";

        [XmlIgnore]
        [JsonProperty("kind")]
        public override string Kind => SerializerTypeName;

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
        public decimal Physical { get; set; }

        [XmlAttribute("interpretation")]
        [JsonProperty("interpretation")]
        public decimal Interpretation { get; set; }

        [XmlAttribute("artistic")]
        [JsonProperty("artistic")]
        public decimal Artistic { get; set; }

        [XmlAttribute("isred")]
        [JsonProperty("isred")]
        public bool IsRed { get; set; }
    }
}
