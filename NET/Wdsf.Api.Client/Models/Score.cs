namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

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
