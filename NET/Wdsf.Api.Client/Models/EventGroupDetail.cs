namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    /// <summary>
    /// An event group with its competitions.
    /// </summary>
    // The API names the XML root "schedule".
    [XmlType("schedule", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("schedule", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.group")]
    [JsonObject("group")]
    public class EventGroupDetail : EntityWithLinks
    {
        [XmlElement("id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [XmlElement("startDate", DataType = "date")]
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [XmlElement("endDate", DataType = "date")]
        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }

        [XmlElement("location")]
        [JsonProperty("location")]
        public string Location { get; set; }

        /// <summary>
        /// The country's name.
        /// </summary>
        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// The group's competitions (provisional ones excluded).
        /// </summary>
        [XmlArray("competitions")]
        [XmlArrayItem("competition")]
        [JsonProperty("competitions")]
        public List<EventGroupCompetition> Competitions { get; set; }
    }
}
