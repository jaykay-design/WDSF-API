namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    [XmlType("mark", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("mark")]
    public class MarkScore : Score
    {
        public MarkScore()
            : base()
        {

        }
        public MarkScore(int officialId, bool isSet)
            : base(officialId)
        {
            this.IsSet = isSet;
        }

        [XmlIgnore]
        [JsonProperty("kind")]
        public override string Kind { get { return "mark"; } set {; } }

        [XmlAttribute("set")]
        [JsonProperty("set")]
        public bool IsSet { get; set; }

        public bool IsSetSpecified { get { return this.IsSet; }  }
    }
}
