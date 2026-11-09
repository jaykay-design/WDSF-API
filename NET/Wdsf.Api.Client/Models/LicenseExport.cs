namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    /// <summary>
    /// An active licence as the person export lists it: type and division only.
    /// </summary>
    [XmlType("license", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("license")]
    public class LicenseExport
    {
        [XmlElement("type")]
        [JsonProperty("type")]
        public string Type { get; set; }

        [XmlElement("division")]
        [JsonProperty("division")]
        public string Division { get; set; }
    }
}
