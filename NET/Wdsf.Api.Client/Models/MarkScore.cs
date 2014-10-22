namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

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

        public override string Kind { get { return "mark"; } set { ; } }

        [XmlAttribute("set")]
        [JsonProperty("set")]
        public bool IsSet { get; set; }

        [XmlIgnore, JsonIgnore]
        public bool IsSetSpecified { get { return this.IsSet; } set { ;} }
    }
}
