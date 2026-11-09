namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("license", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("license")]
    public class License
    {
        [XmlElement("type")]
        [JsonProperty("type")]
        public string Type { get; set; }

        [XmlElement("status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        [XmlElement("division")]
        [JsonProperty("division")]
        public string Division { get; set; }

        /// <summary>
        /// The licence grade; for an ITC licence the level: C Instructor, B Trainer, A Coach.
        /// Omitted when the licence has none.
        /// </summary>
        [XmlElement("grade")]
        [JsonProperty("grade")]
        public string Grade { get; set; }

        /// <summary>
        /// The disciplines the licence covers. Omitted when it names none.
        /// </summary>
        [XmlArray("disciplines")]
        [JsonProperty("disciplines")]
        public List<string> Disciplines { get; set; }

        [XmlElement("expiresOn")]
        [JsonProperty("expiresOn")]
        public string ExpiresOn { get; set; }

        [XmlElement("wrlBLockedUntil")]
        [JsonProperty("wrlBLockedUntil")]
        public string WrlBlockedUntil { get; set; }

        [XmlElement("cupOrChampionshipBlockedUntil")]
        [JsonProperty("cupOrChampionshipBlockedUntil")]
        public string CupOrChampionshipBlockedUntil { get; set; }
    }
}