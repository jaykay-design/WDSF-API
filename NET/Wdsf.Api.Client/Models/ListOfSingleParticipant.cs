﻿namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("participants", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("participants", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.participants.single", IsCollection = true)]
    public class ListOfSingleParticipant : List<ParticipantSingle>
    {
    }
}