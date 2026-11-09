namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    [XmlType("country", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("country")]
    public class Country
    {
        /// <summary>
        /// The country's WDSF short name (e.g. "GER").
        /// </summary>
        [XmlElement("Name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The ISO 3166-1 alpha-2 code.
        /// </summary>
        [XmlElement("Iso2Code")]
        [JsonProperty("iso2Code")]
        public string Iso2Code { get; set; }

        /// <summary>
        /// The country has an active WDSF member body.
        /// </summary>
        [XmlElement("IsActiveNMB")]
        [JsonProperty("isActiveNMB")]
        public bool IsActiveNMB { get; set; }
    }
}
