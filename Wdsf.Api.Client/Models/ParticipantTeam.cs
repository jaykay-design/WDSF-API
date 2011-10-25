namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("participant", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("participant", Namespace = "http://services.worlddancesport.org/api")]
    public class ParticipantTeam : ParticipantBase
    {
        [XmlElement("link")]
        public override Link[] Link { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("country")]
        public string Country { get; set; }

        [XmlAttribute]
        public string Kind { get { return "Team"; } set { ;} }
    }
}
