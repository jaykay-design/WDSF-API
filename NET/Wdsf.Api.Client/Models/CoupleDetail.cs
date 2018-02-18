namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Wdsf.Api.Client.Attributes;

    [XmlType("couple", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot(Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("couple")]
    [MediaType("application/vnd.worlddancesport.couple")]
    public class CoupleDetail : CoupleBase
    {
        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        [XmlElement("age")]
        [JsonProperty("age")]
        public string AgeGroup { get; set; }

        [XmlElement("division")]
        [JsonProperty("division")]
        public string Division { get; set; }

        [XmlElement("status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        [XmlElement("retiredOn")]
        [JsonProperty("retiredOn")]
        public string RetiredOn { get; set; }

        [XmlElement("wrlBLockedUntil")]
        [JsonProperty("wrlBLockedUntil")]
        public string WrlBlockedUntil { get; set; }

        [XmlElement("cupOrChampionshipBlockedUntil")]
        [JsonProperty("cupOrChampionshipBlockedUntil")]
        public string CupOrChampionshipBlockedUntil { get; set; }

        [XmlElement("man")]
        [JsonProperty("man")]
        public int ManMin { get; set; }

        [XmlElement("customManName")]
        [JsonProperty("customManName")]
        public string UnknownManName { get; set; }

        [XmlElement("woman")]
        [JsonProperty("woman")]
        public int WomanMin { get; set; }

        [XmlElement("customWomanName")]
        [JsonProperty("customWomanName")]
        public string UnknownWomanName { get; set; }

        [XmlIgnore]
        [JsonIgnore]
        public bool UnknownWomanNameSpecified { get { return !string.IsNullOrEmpty(UnknownWomanName); } set { } }

        [XmlIgnore]
        [JsonIgnore]
        public bool UnknownManNameSpecified { get { return !string.IsNullOrEmpty(UnknownManName); } set { } }

        [XmlIgnore]
        [JsonIgnore]
        public bool ManMinSpecified { get { return ManMin != 0; } set { } }

        [XmlIgnore]
        [JsonIgnore]
        public bool WomanMinSpecified { get { return WomanMin != 0; } set { } }
    }
}
