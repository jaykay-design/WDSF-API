namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("couple", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot(Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("couple")]
    [MediaType("application/vnd.worlddancesport.couple.export")]
    public class CoupleExport : CoupleBase
    {
        [XmlElement("displayName")]
        [JsonProperty("displayName")]
        public string Name { get; set; }

        [XmlElement("manName")]
        [JsonProperty("manName")]
        public string ManName { get; set; }

        [XmlElement("manSurname")]
        [JsonProperty("manSurname")]
        public string ManSurname { get; set; }

        [XmlElement("manNationality")]
        [JsonProperty("manNationality")]
        public string ManNationality { get; set; }

        [XmlElement("womanName")]
        [JsonProperty("womanName")]
        public string WomanName { get; set; }

        [XmlElement("womanSurname")]
        [JsonProperty("womanSurname")]
        public string WomanSurname { get; set; }

        [XmlElement("womanNationality")]
        [JsonProperty("womanNationality")]
        public string WomanNationality { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        [XmlElement("manMin")]
        [JsonProperty("manMin")]
        public int ManMin { get; set; }

        [XmlElement("womanMin")]
        [JsonProperty("womanMin")]
        public int WomanMin { get; set; }

        [XmlElement("division")]
        [JsonProperty("division")]
        public string Division { get; set; }

        [XmlElement("agegroup")]
        [JsonProperty("agegroup")]
        public string AgeGroup { get; set; }
    }
}
