namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("ranking", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("ranking", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.ranking.team", IsCollection = true)]
    [JsonArray("ranking")]
    public class ListOfRankingTeam : List<RankingTeam>
    {
        [XmlElement("validDate")]
        public DateTime RankingDate { get; set; }
    }
}
