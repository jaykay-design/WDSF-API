﻿namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class ParticipantBaseDetail : EntityWithLinks
    {
        [XmlElement("id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [XmlElement("number")]
        [JsonProperty("number")]
        public int StartNumber { get; set; }

        /// <summary>
        /// Present, Noshow, Excused, Disqualified
        /// </summary>
        [XmlElement("status")]
        [JsonProperty("status")]
        public string Status { get; set; }

        [XmlElement("basepoints")]
        [JsonProperty("basepoints")]
        public string Points { get; set; }

        [XmlElement("rank")]
        [JsonProperty("rank")]
        public string Rank { get; set; }

        [XmlElement("competitionId")]
        [JsonProperty("competitionId")]
        public int CompetitionId { get; set; }

        public bool CompetitionIdSpecified { get { return this.CompetitionId != 0; } }

        [XmlArray("rounds")]
        [JsonProperty("rounds")]
        /// <summary>
        /// Contains the scores. Set to null if the scores shall not be updated. Empty list will clear rounds.
        /// </summary>
        public List<Round> Rounds { get; set; }

        public bool ShouldSerializeRounds()
        {
            return Rounds != null;
        }
        public bool ShouldSerializePoints()
        {
            return !string.IsNullOrEmpty(Points);
        }
        public bool ShouldSerializeRank()
        {
            return !string.IsNullOrEmpty(Rank);
        }
        public bool ShouldSerializeStatus()
        {
            return !string.IsNullOrEmpty(Status);
        }
        public bool ShouldSerializeCompetitionId()
        {
            return CompetitionId != 0;
        }
        public bool ShouldSerializeStartNumber()
        {
            return StartNumber != 0;
        }
        public bool ShouldSerializeId()
        {
            return Id != 0;
        }

    }
}
