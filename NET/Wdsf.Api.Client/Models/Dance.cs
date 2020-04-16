namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("dance", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("dance")]
    public class Dance
    {
        private List<Score> scores = new List<Score>();

        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlAttribute("isGroupDance")]
        [JsonProperty("isGroupDance")]
        public bool IsGroupDance { get; set; }

        /// <summary>
        /// <para>Do not use this array to process scores.</para>
        /// <para>It is used only as a workaround for .NET's XmlSerializer limitations on deserializing lists.</para>
        /// </summary>
        [XmlArray("scores")]
        [XmlArrayItem("mark", typeof(MarkScore))]
        [XmlArrayItem("final", typeof(FinalScore))]
        [XmlArrayItem("onScale", typeof(OnScaleScore))]
        [XmlArrayItem("onScaleIdo", typeof(OnScaleIdoScore))]
        [XmlArrayItem("onScale2", typeof(OnScale2Score))]
        [XmlArrayItem("onScale3", typeof(OnScale3Score))]
        [JsonProperty("scores", ItemConverterType = typeof(Converter.JsonScoreConverter))]
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

        [XmlIgnore, JsonIgnore]
        public IList<Score> Scores
        {
            get
            {
                return scores;
            }
        }
    }
}
