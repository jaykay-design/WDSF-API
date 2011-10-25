namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("couples", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("couples", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.couples", IsCollection = true)]
    public class ListOfCouple : List<Couple>
    {
    }
}
