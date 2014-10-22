namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Wdsf.Api.Client.Attributes;

    [XmlType("age", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("age")]
    [MediaType("application/vnd.worlddancesport.age")]
    public class AgeClass
    {
        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [XmlElement("fromAge")]
        [JsonProperty("fromAge")]
        public int FromAge { get; set; }
        
        [XmlElement("toAge")]
        [JsonProperty("toAge")]
        public int ToAge { get; set; }

        [XmlElement("minBirthdate", DataType = "date" )]
        [JsonProperty("minBirthdate")]
        public DateTime MinBirthDate { get; set; }

        [XmlElement("maxBirthdate", DataType = "date" )]
        [JsonProperty("maxBirthdate")]
        public DateTime MaxBirthDate { get; set; }

        [XmlArray("allowedToDanceIn")]
        [XmlArrayItem("age")]
        [JsonProperty("allowedToDanceIn")]
        public string[] AllowedToDanceIn { get; set; }
    }
}
