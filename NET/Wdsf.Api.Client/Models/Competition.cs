namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Xml.Serialization;

    [XmlType("competition", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("competition", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("competition")]
    public class Competition : EntityWithLinks
    {
        [XmlElement("id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlElement("discipline")]
        [JsonProperty("discipline")]
        public string Discipline { get; set; }

        /// <summary>
        /// The dance form (its xml name). Omitted when the competition has none.
        /// </summary>
        [XmlElement("danceform")]
        [JsonProperty("danceform")]
        public string DanceForm { get; set; }

        [XmlElement("age")]
        [JsonProperty("age")]
        public string AgeClass { get; set; }

        [XmlElement("coefficient")]
        [JsonProperty("coefficient")]
        public decimal Coefficient { get; set; }

        [XmlElement("date", DataType = "date")]
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [XmlElement("status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        [XmlElement("division")]
        [JsonProperty("division")]
        public string Division { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }

        [XmlElement("location")]
        [JsonProperty("location")]
        public string Location { get; set; }

        [XmlElement("competitionType")]
        [JsonProperty("competitionType")]
        public string CompetitionType { get; set; }

        [XmlElement("lastmodifiedDate")]
        [JsonProperty("lastmodifiedDate")]
        public DateTime LastModifiedDate { get; set; }

    }
}