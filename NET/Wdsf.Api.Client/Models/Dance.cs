namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Linq;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    [XmlType("dance", Namespace = "http://services.worlddancesport.org/api")]
    public class Dance
    {
        private List<Score> scores = new List<Score>();

        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// <para>Do not use this array to process scores.</para>
        /// <para>It is used only as a workaround for .NET's XmlSerializer limitations on deserializing lists.</para>
        /// </summary>
        [XmlArray("scores")]
        [XmlArrayItem("mark", typeof(MarkScore))]
        [XmlArrayItem("final", typeof(FinalScore))]
        [XmlArrayItem("onScale", typeof(OnScaleScore))]
        public Score[] ScoresForSerialization
        {
            get
            {
                return scores.Count == 0 ? null : scores.ToArray();
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                scores = new List<Score>(value);
            }
        }

        [XmlIgnore]
        public IList<Score> Scores
        {
            get
            {
                return scores;
            }
        }
    }
}
