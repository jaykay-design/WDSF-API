namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    public class CoupleBase: EntityWithLinks
    {
        [XmlElement("id")]
        [JsonProperty("id")]
        public virtual string Id { get; set; }

        /// <summary>
        /// The reference a member federation stores for the couple.
        /// </summary>
        [XmlElement("nationalReference")]
        [JsonProperty("nationalReference")]
        public string NationalReference { get; set; }
    }
}
