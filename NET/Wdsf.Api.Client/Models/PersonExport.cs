namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// A row of the person export.
    /// </summary>
    [XmlType("person", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("person")]
    public class PersonExport : EntityWithLinks
    {
        [XmlElement("id")]
        [JsonProperty("id")]
        public int Min { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlElement("surname")]
        [JsonProperty("surname")]
        public string Surname { get; set; }

        [XmlElement("nickname")]
        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        [XmlElement("sex")]
        [JsonProperty("sex")]
        public string Sex { get; set; }

        [XmlElement("ageGroup")]
        [JsonProperty("ageGroup")]
        public string AgeGroup { get; set; }

        /// <summary>
        /// The type and division of every active licence the person holds.
        /// </summary>
        [XmlArray("licenses")]
        [JsonProperty("licenses")]
        public List<LicenseExport> Licenses { get; set; }
    }
}
