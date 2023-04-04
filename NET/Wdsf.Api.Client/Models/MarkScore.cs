namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public class MarkScore : Score
    {
        public const string SerializerTypeName = "mark";
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
        public override string Kind => SerializerTypeName;

        [XmlAttribute("set")]
        [JsonProperty("set")]
        public bool IsSet { get; set; }
    }
}
