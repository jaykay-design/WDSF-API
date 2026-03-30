namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("person", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("person", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.person")]
    [JsonObject("person")]
    public class PersonDetail : EntityWithLinks
    {
        [XmlElement("id")]
        [JsonProperty("id")]
        public int Min { get; set; }

        [XmlElement("nickname")]
        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlElement("surname")]
        [JsonProperty("surname")]
        public string Surname { get; set; }

        [XmlElement("sex")]
        [JsonProperty("sex")]
        public string Sex { get; set; }

        [XmlElement("title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [XmlElement("nationality")]
        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        [XmlElement("ageGroup")]
        [JsonProperty("ageGroup")]
        public string AgeGroup { get; set; }

        [XmlElement("yearOfBirth")]
        [JsonProperty("yearOfBirth")]
        public int YearOfBirth { get; set; }

        [XmlElement("nationalReference")]
        [JsonProperty("nationalReference")]
        public string NationalReference { get; set; }

        [XmlArray("licenses")]
        [JsonProperty("licenses")]
        public List<License> Licenses { get; set; }
        public bool ShouldSerializeLicenses()
        {
            return Licenses != null && Licenses.Count > 0;
        }

    }
}
