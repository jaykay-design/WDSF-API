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

        /// <summary>
        /// The id of the condition, one per response condition (see the API wiki, Localisation).
        /// Use it to localize or branch on the outcome; <see cref="Error"/> holds the details.
        /// </summary>
        [XmlElement("messageId")]
        [JsonProperty("messageId")]
        public int MessageId { get; set; }

        [XmlElement("message")]
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Details of the condition (names, dates, ids, or the broken eligibility rules); for an
        /// unexpected error (message id 10001) a reference to quote when reporting the problem.
        /// </summary>
        [XmlElement("error")]
        [JsonProperty("error")]
        public string Error { get; set; }

        [XmlElement("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [XmlElement("link")]
        [JsonProperty("link")]
        public Link Link { get; set; }
    }
}
