namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class ParticipantBaseDetail
    {
        private List<Round> rounds = new List<Round>();

        [XmlElement("link")]
        public virtual Link[] Link { get; set; }

        [XmlElement("id")]
        public int Id { get; set; }

        [XmlElement("number")]
        public int StartNumber { get; set; }

        /// <summary>
        /// Present, Noshow, Excused, Disqualified
        /// </summary>
        [XmlElement("status")]
        public string Status { get; set; }

        [XmlElement("basepoints")]
        public string Points { get; set; }

        [XmlElement("rank")]
        public string Rank { get; set; }

        [XmlElement("competitionId")]
        public int CompetitionId { get; set; }

        [XmlIgnore]
        private bool CompetitionIdSpecified { get { return this.CompetitionId != 0; } set { } }

        /// <summary>
        /// <para>Do not use this array to process rounds.</para>
        /// <para>It is used only as a workaround for .NET's XmlSerializer limitations on deserializing lists.</para>
        /// </summary>
        [XmlArray("rounds")]
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
        public IList<Round> Rounds
        {
            get
            {
                return rounds;
            }
        }
    }
}
