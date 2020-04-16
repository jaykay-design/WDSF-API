namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public abstract class EntityWithLinks
    {
        [XmlElement("link")]
        [JsonProperty("link")]
        public virtual List<Link> Links { get; set; }

    }
}
