namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("participant", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot(Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.participant.team")]
    public class ParticipantTeamDetail : ParticipantBaseDetail
    {
        [XmlElement("link")]
        public override Link[] Link { get; set; }

        [XmlElement("teamId")]
        public int TeamId { get; set; }

        [XmlElement("name")]
        public Guid Team { get; set; }

        [XmlElement("country")]
        public Guid Country { get; set; }

        [XmlAttribute("kind")]
        public string Kind { get { return "team"; } set { ;} }
    }
}
