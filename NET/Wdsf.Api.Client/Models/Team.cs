namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    [XmlType("team", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("team")]
    public class Team : TeamBase
    {
    }
}