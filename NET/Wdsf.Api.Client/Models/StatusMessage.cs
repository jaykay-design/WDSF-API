namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("status", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot(Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("status")]
    [MediaType("application/vnd.worlddancesport.status")]
    public class StatusMessage
    {
        [XmlElement("code")]
        [JsonProperty("code")]
        public int Code { get; set; }

        [XmlElement("subcode")]
        [JsonProperty("subcode")]
        public int SubCode { get; set; }

        [XmlElement("message")]
        [JsonProperty("message")]
        public string Message { get; set; }

        [XmlElement("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [XmlElement("link")]
        [JsonProperty("link")]
        public Link Link { get; set; }
    }
}
