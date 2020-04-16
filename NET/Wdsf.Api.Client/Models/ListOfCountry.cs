namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("countries", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("countries", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.countries", IsCollection = true)]
    [JsonArray("countries")]
    public class ListOfCountry : List<Country>
    {
    }
}
