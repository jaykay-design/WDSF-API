namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType("team", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("team")]
    public class Team : TeamBase
    {
    }
}