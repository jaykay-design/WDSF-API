namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("officials",Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("officials", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.officials", IsCollection = true)]
    public class ListOfOfficial : List<Official>
    {
    }
}
