namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("participants", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("participants", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.participants.couple", IsCollection = true)]
    [JsonArray("participants")]
    public class ListOfCoupleParticipant : List<ParticipantCouple>
    {
    }
}
