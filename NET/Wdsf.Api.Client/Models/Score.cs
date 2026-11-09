namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Linq;
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

        /// <summary>
        /// The score kind, sent as the JSON discriminator. Defaults to the score type's
        /// serializer name.
        /// </summary>
        [XmlIgnore]
        [JsonProperty("kind")]
        public virtual string Kind =>
            GetType().GetCustomAttributes(typeof(XmlTypeAttribute), false).OfType<XmlTypeAttribute>().FirstOrDefault()?.TypeName;

        [XmlElement("link")]
        [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
        public List<Link> Link { get; set; }

        [XmlAttribute("adjudicator")]
        [JsonProperty("adjudicator")]
        public int OfficialId { get; set; }
    }
}
