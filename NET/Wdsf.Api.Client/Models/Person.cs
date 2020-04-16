namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Xml.Serialization;

    [XmlType("person", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("person", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("person")]
    public class Person : EntityWithLinks
    {
        [XmlElement("id")]
        [JsonProperty("id")]
        public int Min { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlElement("sex")]
        [JsonProperty("sex")]
        public string Sex { get; set; }

        public bool IsMale
        {
            get
            {
                if (Sex.Equals(Gender.Male, System.StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                else if (Sex.Equals(Gender.Female, System.StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                else
                {
                    throw new ApplicationException("The person's sex not specified.");
                }
            }
            set
            {
                Sex = (value ? Gender.Male : Gender.Female);
            }
        }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        [XmlElement("activePartner")]
        [JsonProperty("activePartner")]
        public string ActivePartner { get; set; }

        [XmlElement("activeCoupleId")]
        [JsonProperty("activeCoupleId")]
        public string ActiveCoupleId { get; set; }

        [XmlElement("activeCoupleAgeGroup")]
        [JsonProperty("activeCoupleAgeGroup")]
        public string AgeGroup { get; set; }
    }
}
