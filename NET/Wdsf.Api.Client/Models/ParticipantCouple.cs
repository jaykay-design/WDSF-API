namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("participant", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("participant", Namespace = "http://services.worlddancesport.org/api")]
    public class ParticipantCouple : ParticipantBase
    {
        [XmlElement("link")]
        public override Link[] Link { get; set; }

        [XmlElement("name")]
        public string Couple { get; set; }

        [XmlElement("country")]
        public string Country { get; set; }

        [XmlElement("number")]
        public string StartNumber { get; set; }
    }
}
