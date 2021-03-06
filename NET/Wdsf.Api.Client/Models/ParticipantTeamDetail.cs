﻿namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("participant", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot(Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("participant")]
    [MediaType("application/vnd.worlddancesport.participant.team")]
    public class ParticipantTeamDetail : ParticipantBaseDetail
    {
        [XmlElement("teamId")]
        [JsonProperty("teamId")]
        public int TeamId { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Team { get; set; }

        [XmlElement("country")]
        [JsonProperty("country")]
        public string Country { get; set; }
    }
}
