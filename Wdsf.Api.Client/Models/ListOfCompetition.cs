namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("competitions",Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("competitions",Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.competitions", IsCollection = true)]
    public class ListOfCompetition : List<Competition>
    {
    }
}
