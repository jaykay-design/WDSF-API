namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Wdsf.Api.Client.Attributes;

    [XmlType("ranking", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("ranking", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.ranking", IsCollection = true)]
    [JsonArray("ranking")]
    public class ListOfRanking : List<Ranking>
    {
        [XmlElement("validDate")]
        public DateTime RankingDate { get; set; }
    }
}
