namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    [XmlType(SerializerTypeName, Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject(SerializerTypeName)]
    public class FinalScore : Score
    {
        public const string SerializerTypeName = "final";
        public FinalScore() :
            base()
        {

        }
        public FinalScore(int officialId, int rank) :
            base(officialId)
        {
            this.Rank = rank;
        }

        [XmlIgnore]
        [JsonProperty("kind")]
        public override string Kind => SerializerTypeName;

        [XmlAttribute("rank")]
        [JsonProperty("rank")]
        public int Rank { get; set; }
    }
}
