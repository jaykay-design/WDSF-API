namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    /// <summary>
    /// An entry of an event group's time schedule. Scrutineer accounts only.
    /// </summary>
    /// <remarks>
    /// On an update every property left null keeps its value. An entry belongs either to a WDSF
    /// competition (CompetitionId) or to a custom one (CustomCompetitionId and CustomCompetitionName,
    /// with GroupId).
    /// </remarks>
    [XmlType("schedule", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("schedule", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.schedule")]
    [JsonObject("schedule", ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ScheduleEntry
    {
        [XmlElement("link")]
        [JsonProperty("link")]
        public List<Link> Links { get; set; }
        public bool ShouldSerializeLinks()
        {
            return false;
        }

        [XmlElement("id")]
        [JsonProperty("id")]
        public Guid Id { get; set; }
        public bool ShouldSerializeId()
        {
            return false;
        }

        [XmlElement("groupId")]
        [JsonProperty("groupId", NullValueHandling = NullValueHandling.Ignore)]
        public int? GroupId { get; set; }
        public bool ShouldSerializeGroupId() => GroupId.HasValue;

        [XmlElement("competitionId")]
        [JsonProperty("competitionId", NullValueHandling = NullValueHandling.Ignore)]
        public int? CompetitionId { get; set; }
        public bool ShouldSerializeCompetitionId() => CompetitionId.HasValue;

        /// <summary>
        /// Your own id of a competition not held in the WDSF database (at most 36 characters).
        /// </summary>
        [XmlElement("customCompetitionId")]
        [JsonProperty("customCompetitionId", NullValueHandling = NullValueHandling.Ignore)]
        public string CustomCompetitionId { get; set; }

        [XmlElement("customCompetitionName")]
        [JsonProperty("customCompetitionName", NullValueHandling = NullValueHandling.Ignore)]
        public string CustomCompetitionName { get; set; }

        /// <summary>
        /// The planned start (required when creating an entry).
        /// </summary>
        [XmlElement("time")]
        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? Time { get; set; }
        public bool ShouldSerializeTime() => Time.HasValue;

        [XmlElement("delayedTime")]
        [JsonProperty("delayedTime", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? DelayedTime { get; set; }
        public bool ShouldSerializeDelayedTime() => DelayedTime.HasValue;

        [XmlElement("isCompleted")]
        [JsonProperty("isCompleted", NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsCompleted { get; set; }
        public bool ShouldSerializeIsCompleted() => IsCompleted.HasValue;

        [XmlElement("expectedMinutes")]
        [JsonProperty("expectedMinutes", NullValueHandling = NullValueHandling.Ignore)]
        public int? ExpectedMinutes { get; set; }
        public bool ShouldSerializeExpectedMinutes() => ExpectedMinutes.HasValue;

        [XmlElement("text")]
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public string Text { get; set; }

        [XmlElement("hall")]
        [JsonProperty("hall", NullValueHandling = NullValueHandling.Ignore)]
        public string Hall { get; set; }

        [XmlElement("floor")]
        [JsonProperty("floor", NullValueHandling = NullValueHandling.Ignore)]
        public string Floor { get; set; }

        [XmlElement("round")]
        [JsonProperty("round", NullValueHandling = NullValueHandling.Ignore)]
        public string Round { get; set; }

        [XmlElement("dances")]
        [JsonProperty("dances", NullValueHandling = NullValueHandling.Ignore)]
        public string Dances { get; set; }

        [XmlElement("heats")]
        [JsonProperty("heats", NullValueHandling = NullValueHandling.Ignore)]
        public int? Heats { get; set; }
        public bool ShouldSerializeHeats() => Heats.HasValue;

        /// <summary>
        /// A description of the participants (e.g. "48 couples").
        /// </summary>
        [XmlElement("participants")]
        [JsonProperty("participants", NullValueHandling = NullValueHandling.Ignore)]
        public string Participants { get; set; }

        /// <summary>
        /// The ids of the participants dancing in this entry.
        /// </summary>
        [XmlArray("participantIds")]
        [XmlArrayItem("id")]
        [JsonProperty("participantIds", NullValueHandling = NullValueHandling.Ignore)]
        public List<int> ParticipantIds { get; set; }
        public bool ShouldSerializeParticipantIds() => ParticipantIds != null && ParticipantIds.Count > 0;
    }
}
