namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    [XmlType("country", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("country")]
    public class Country
    {
        [XmlText]
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
