namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("participant", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("participant", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.participant.single")]
    public class ParticipantSingleDetail : ParticipantBaseDetail
    {
        [XmlElement("link")]
        public override Link[] Link { get; set; }

        [XmlElement("personId")]
        public string PersonId { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("country")]
        public string Country { get; set; }
    }
}
