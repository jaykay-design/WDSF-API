namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
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

        [XmlElement("minBirthdate", DataType = "date")]
        [JsonProperty("minBirthdate")]
        public DateTime MinBirthDate { get; set; }

        [XmlElement("maxBirthdate", DataType = "date")]
        [JsonProperty("maxBirthdate")]
        public DateTime MaxBirthDate { get; set; }

        [XmlArray("allowedToDanceIn")]
        [XmlArrayItem("age")]
        [JsonProperty("allowedToDanceIn")]
        public List<string> AllowedToDanceIn { get; set; }

        /// <summary>
        /// The divisions the age group exists in.
        /// </summary>
        [XmlElement("divisions")]
        [JsonProperty("divisions")]
        public List<string> Divisions { get; set; }

        /// <summary>
        /// The age group applies to a person.
        /// </summary>
        [XmlElement("isPerson")]
        [JsonProperty("isPerson")]
        public bool IsPerson { get; set; }

        /// <summary>
        /// The age group applies to a couple.
        /// </summary>
        [XmlElement("isCouple")]
        [JsonProperty("isCouple")]
        public bool IsCouple { get; set; }

        /// <summary>
        /// The age group applies to a team.
        /// </summary>
        [XmlElement("isTeam")]
        [JsonProperty("isTeam")]
        public bool IsTeam { get; set; }

        /// <summary>
        /// The age group can be used for a competition.
        /// </summary>
        [XmlElement("isCompetition")]
        [JsonProperty("isCompetition")]
        public bool IsCompetition { get; set; }
    }
}
