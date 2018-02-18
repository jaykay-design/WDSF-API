namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Wdsf.Api.Client.Attributes;

    [XmlType("officials",Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("officials", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.officials", IsCollection = true)]
    [JsonArray("officials")]
    public class ListOfOfficial : List<Official>
    {
    }
}
