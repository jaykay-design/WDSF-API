namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    [XmlType("final", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("final")]
    public class FinalScore : Score
    {
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
        public override string Kind { get { return "final"; } }

        [XmlAttribute("rank")]
        [JsonProperty("rank")]
        public int Rank { get; set; }
    }
}
