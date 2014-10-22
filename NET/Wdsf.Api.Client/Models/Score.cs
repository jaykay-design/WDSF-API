namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    public class Score
    {
        public Score()
        {

        }
        public Score(int officialId)
        {
            this.OfficialId = officialId;
        }

        [XmlIgnore]
        [JsonProperty("kind")]
        public virtual string Kind { get; set; }

        [XmlElement("link")]
        [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
        public Link[] Link { get; set; }

        [XmlAttribute("adjudicator")]
        [JsonProperty("adjudicator")]
        public int OfficialId { get; set; }
    }
}
