namespace Wdsf.Api.Client.Models
{
    using System.Xml.Serialization;

    public class CoupleBase
    {
        [XmlElement("link")]
        public virtual Link[] Links { get; set; }

        [XmlElement("id")]
        public virtual string Id { get; set; }
    }
}
