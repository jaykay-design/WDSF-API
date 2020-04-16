namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("couples", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("couples", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.couples.export", IsCollection = true)]
    [JsonArray("couples")]
    public class ListOfCoupleExport : List<CoupleExport>
    {
    }
}
