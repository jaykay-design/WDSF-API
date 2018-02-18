namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Wdsf.Api.Client.Attributes;

    [XmlType("ages", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("ages", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.ages", IsCollection = true)]
    [JsonArray("ages")]
    public class ListOfAgeClass : List<AgeClass>
    {
    }
}
