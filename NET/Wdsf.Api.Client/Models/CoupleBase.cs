namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    public class CoupleBase
    {
        [XmlElement("link")]
        [JsonProperty("link")]
        public virtual Link[] Links { get; set; }

        [XmlElement("id")]
        [JsonProperty("id")]
        public virtual string Id { get; set; }
    }
}
