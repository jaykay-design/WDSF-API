namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    [XmlType("link")]
    [JsonObject("link")]
    public class Link
    {
        [XmlAttribute("href", Namespace = "http://www.w3.org/1999/xlink")]
        [JsonProperty("href")]
        public string HRef { get; set; }

        [XmlAttribute("rel")]
        [JsonProperty("rel")]
        public string Rel { get; set; }

        [XmlAttribute("type")]
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
