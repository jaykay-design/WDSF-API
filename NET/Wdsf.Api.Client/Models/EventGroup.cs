namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// An event group (the competitions held together at one place) as the group list returns it.
    /// </summary>
    [XmlType("group", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("group")]
    public class EventGroup : EntityWithLinks
    {
        [XmlElement("id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [XmlElement("location")]
        [JsonProperty("location")]
        public string Location { get; set; }

        /// <summary>
        /// The country's name.
        /// </summary>
        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        [XmlElement("startDate", DataType = "date")]
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [XmlElement("endDate", DataType = "date")]
        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }
    }
}
