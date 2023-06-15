namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    [XmlType("dance", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("dance")]
    public class Dance
    {
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlAttribute("isGroupDance")]
        [JsonProperty("isGroupDance")]
        public bool IsGroupDance { get; set; }

        [XmlArray("scores")]
        [XmlArrayItem(MarkScore.SerializerTypeName, typeof(MarkScore))]
        [XmlArrayItem(FinalScore.SerializerTypeName, typeof(FinalScore))]
        [XmlArrayItem(OnScaleScore.SerializerTypeName, typeof(OnScaleScore))]
        [XmlArrayItem(OnScaleIdoScore.SerializerTypeName, typeof(OnScaleIdoScore))]
        [XmlArrayItem(OnScale2Score.SerializerTypeName, typeof(OnScale2Score))]
        [XmlArrayItem(OnScale3Score.SerializerTypeName, typeof(OnScale3Score))]
        [XmlArrayItem(WdsfBreakingScore.SerializerTypeName, typeof(WdsfBreakingScore))]
        [XmlArrayItem(TriviumScore.SerializerTypeName, typeof(TriviumScore))]
        [XmlArrayItem(ThreeFoldScore.SerializerTypeName, typeof(ThreeFoldScore))]
        [XmlArrayItem(BreakingSeedScore.SerializerTypeName, typeof(BreakingSeedScore))]
        [XmlArrayItem(BreakingSeedByScoreScore.SerializerTypeName, typeof(BreakingSeedByScoreScore))]
        [JsonProperty("scores", ItemConverterType = typeof(Converter.JsonScoreConverter))]
        public List<Score> Scores { get; set; }
        public bool ShouldSerializeScores()
        {
            return Scores != null && Scores.Count > 0;
        }

    }
}
