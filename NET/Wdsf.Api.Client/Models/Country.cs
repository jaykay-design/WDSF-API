namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType("country", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("country")]
    public class Country
    {
        [XmlText]
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
