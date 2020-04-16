namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("teams", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("teams", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.teams", IsCollection = true)]
    [JsonArray("teams")]
    public class ListOfTeam : List<Team>
    {
    }
}
