namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Linq;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    [XmlType("dance", Namespace = "http://services.worlddancesport.org/api")]
    public class Dance
    {
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
                return Scores == null ? null : Scores.ToArray();
            }
            set
            {
                Scores = new List<Score>(value);
            }
        }

        [XmlIgnore]
        public List<Score> Scores { get; set; }

        public Dance()
        {
            this.Scores = new List<Score>();
        }
    }
}
