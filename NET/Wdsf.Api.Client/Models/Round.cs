namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("round", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("round")]
    public class Round
    {
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Maximum allowed deviation of scrores in AJS 3.0
        /// </summary>
        [XmlAttribute("maxDeviation")]
        [JsonProperty("maxDeviation")]
        public decimal MaxDeviation { get; set; }
        [XmlIgnore, JsonIgnore]
        public bool MaxDeviationSpecified { get { return this.MaxDeviation != 0; } set {; } }

        [XmlArray("dances")]
        [JsonProperty("dances")]
        public List<Dance> Dances { get; set; }

        public bool ShouldSerializeDances()
        {
            return Dances != null && Dances.Count > 0;
        }
    }
}
