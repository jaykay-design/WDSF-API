namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("groups", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("groups", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.groups", IsCollection = true)]
    [JsonArray("groups")]
    public class ListOfEventGroup : List<EventGroup>
    {
    }
}
