namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public sealed class WdsfBreakingScore : Score
    {
        public const string SerializerTypeName = "breakingseed";

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


        [XmlAttribute("technique")]
        [JsonProperty("technique")]
        public int Technique { get; set; }

        [XmlAttribute("vocabulary")]
        [JsonProperty("vocabulary")]
        public int Vocabulary { get; set; }

        [XmlAttribute("originality")]
        [JsonProperty("originality")]
        public int Originality { get; set; }

        [XmlAttribute("execution")]
        [JsonProperty("execution")]
        public int Execution { get; set; }

        [XmlAttribute("musicality")]
        [JsonProperty("musicality")]
        public int Musicality { get; set; }


        [XmlAttribute("misbehaviour")]
        [JsonProperty("misbehaviour")]
        public int Misbehaviour { get; set; }

        [XmlAttribute("isred")]
        [JsonProperty("isred")]
        public bool IsRed { get; set; }
    }
}
