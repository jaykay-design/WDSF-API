namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using Wdsf.Api.Client.Attributes;

    [XmlType("participants", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("participants", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.participants.single", IsCollection = true)]
    [JsonArray("participants")]
    public class ListOfSingleParticipant : List<ParticipantSingle>
    {
    }
}
