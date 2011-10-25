namespace Wdsf.Api.Client.Models
{
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    public class CoupleBase
    {
        [XmlElement("link")]
        public virtual Link[] Links { get; set; }

        [XmlElement("id")]
        public virtual string Id { get; set; }
    }
}
