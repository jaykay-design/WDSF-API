namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    public class ParticipantBaseDetail
    {
        private List<Round> rounds = new List<Round>();

        [XmlElement("link")]
        [JsonProperty("link")]
        public virtual Link[] Link { get; set; }

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

        [XmlIgnore]
        [JsonIgnore]
        private bool CompetitionIdSpecified { get { return this.CompetitionId != 0; } set { } }

        /// <summary>
        /// <para>Do not use this array to process rounds.</para>
        /// <para>It is used only as a workaround for .NET's XmlSerializer limitations on deserializing lists.</para>
        /// </summary>
        [XmlArray("rounds")]
        [JsonProperty("rounds")]
        public Round[] RoundsForSerialization
        {
            get
            {
                return rounds.Count == 0 ? null : rounds.ToArray();
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                rounds = new List<Round>(value);
            }
        }

        /// <summary>
        /// Contains the scores. Set to null if the scores shall not be updated.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public IList<Round> Rounds
        {
            get
            {
                return rounds;
            }
        }
    }
}
