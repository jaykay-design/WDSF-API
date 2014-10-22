namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Wdsf.Api.Client.Attributes;

    [XmlType("persons", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("persons", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.persons", IsCollection = true)]
    [JsonArray("persons")]
    public class ListOfPerson : List<Person>
    {
    }
}
