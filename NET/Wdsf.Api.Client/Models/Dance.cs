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
        [XmlArrayItem("mark", typeof(MarkScore))]
        [XmlArrayItem("final", typeof(FinalScore))]
        [XmlArrayItem("onScale", typeof(OnScaleScore))]
        [XmlArrayItem("onScaleIdo", typeof(OnScaleIdoScore))]
        [XmlArrayItem("onScale2", typeof(OnScale2Score))]
        [XmlArrayItem("onScale3", typeof(OnScale3Score))]
        [JsonProperty("scores", ItemConverterType = typeof(Converter.JsonScoreConverter))]
        public List<Score> Scores { get; set; }
        public bool ShouldSerializeScores()
        {
            return Scores != null && Scores.Count > 0;
        }

    }
}
