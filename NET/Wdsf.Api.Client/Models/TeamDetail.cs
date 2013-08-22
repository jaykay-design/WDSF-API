namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("team", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("team", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.team")]
    public class TeamDetail :TeamBase
    {
        [XmlElement("status")]
        public string Status { get; set; }
    }
}
