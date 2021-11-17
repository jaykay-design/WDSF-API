namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Attributes;
    using Newtonsoft.Json;

    [XmlType("results", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("results", Namespace = "http://services.worlddancesport.org/api")]
    [JsonArray("results")]
    [MediaType("application/vnd.worlddancesport.results")]
    public class ListOfResults : List<Result>
    {
    }
}
