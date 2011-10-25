namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("participant", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot(Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.participant.couple")]
    public class ParticipantCoupleDetail : ParticipantBaseDetail
    {
        [XmlElement("link")]
        public override Link[] Link { get; set; }

        [XmlElement("coupleId")]
        public string CoupleId { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("country")]
        public string Country { get; set; }

        [XmlAttribute("kind")]
        public string Kind { get { return "couple"; } set { ;} }
    }
}
