namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType("breakingseed", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("breakingseed")]
    public sealed class BreakingSeedScore : Score
    {
        [XmlIgnore]
        [JsonProperty("kind")]
        public override string Kind { get { return "breakingseed"; } }

        [XmlAttribute("score")]
        [JsonProperty("score")]
        public int Score { get; set; }
    }
}
