namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public sealed class TriviumScore : Score
    {
        public const string SerializerTypeName = "trivium";

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

        [XmlAttribute("variety")]
        [JsonProperty("variety")]
        public int Variety { get; set; }

        [XmlAttribute("performativity")]
        [JsonProperty("performativity")]
        public int Performativity { get; set; }

        [XmlAttribute("musicality")]
        [JsonProperty("musicality")]
        public int Musicality { get; set; }

        [XmlAttribute("creativity")]
        [JsonProperty("creativity")]
        public int Creativity { get; set; }

        [XmlAttribute("personality")]
        [JsonProperty("personality")]
        public int Personality { get; set; }

        [XmlAttribute("crash1")]
        [JsonProperty("crash1")]
        public int Crash1 { get; set; }

        [XmlAttribute("crash2")]
        [JsonProperty("crash2")]
        public int Crash2 { get; set; }

        [XmlAttribute("crash3")]
        [JsonProperty("crash3")]
        public int Crash3 { get; set; }

        [XmlAttribute("misbehaviour")]
        [JsonProperty("misbehaviour")]
        public int Misbehaviour { get; set; }

        [XmlAttribute("repeat")]
        [JsonProperty("repeat")]
        public int Repeat { get; set; }

        [XmlAttribute("bite")]
        [JsonProperty("bite")]
        public int Bite { get; set; }

        [XmlAttribute("spontaneity")]
        [JsonProperty("spontaneity")]
        public int Spontaneity { get; set; }

        [XmlAttribute("confidence")]
        [JsonProperty("confidence")]
        public int Confidence { get; set; }

        [XmlAttribute("execution")]
        [JsonProperty("execution")]
        public int Execution { get; set; }

        [XmlAttribute("form")]
        [JsonProperty("form")]
        public int Form { get; set; }

        [XmlAttribute("isred", DataType = "boolean")]
        [JsonProperty("isred")]
        public bool IsRed { get; set; }
    }
}
