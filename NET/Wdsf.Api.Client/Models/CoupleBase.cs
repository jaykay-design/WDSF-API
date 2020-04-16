namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    public class CoupleBase: EntityWithLinks
    {
        [XmlElement("id")]
        [JsonProperty("id")]
        public virtual string Id { get; set; }
    }
}
