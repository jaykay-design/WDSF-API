namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    [XmlType("couple", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("couple")]
    public class Couple : CoupleBase
    {
        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        [XmlElement("division")]
        [JsonProperty("division")]
        public string Division { get; set; }

        [XmlElement("ageGroup")]
        [JsonProperty("ageGroup")]
        public string AgeGroup { get; set; }

        [XmlElement("status")]
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
