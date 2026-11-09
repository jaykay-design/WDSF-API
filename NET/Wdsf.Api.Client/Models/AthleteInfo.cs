namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// An athlete of a competition entry.
    /// </summary>
    public class AthleteInfo
    {
        /// <summary>
        /// The element name the API uses for an athlete inside the athletes list (XML).
        /// </summary>
        public const string XmlItemName = "AthleteInfoViewModel";

        [JsonProperty("name")]
        [System.Xml.Serialization.XmlElement("name")]
        public string Name { get; set; }

        [JsonProperty("surname")]
        [System.Xml.Serialization.XmlElement("surname")]
        public string Surname { get; set; }

        [JsonProperty("min")]
        [System.Xml.Serialization.XmlElement("min")]
        public int Min { get; set; }

        [JsonProperty("country")]
        [System.Xml.Serialization.XmlElement("country")]
        public string Country { get; set; }

        /// <summary>
        /// The athlete's e-mail address. Returned to scrutiny and service-provider accounts only.
        /// </summary>
        [JsonProperty("email")]
        [System.Xml.Serialization.XmlElement("email")]
        public string Email { get; set; }
    }
}
