namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class ParticipantBase
    {
        [XmlElement("link")]
        public virtual Link[] Link { get; set; }

        [XmlIgnore()]
        public int CompetitionId { get; set; }

        [XmlElement("id")]
        public int Id { get; set; }
    }
}
