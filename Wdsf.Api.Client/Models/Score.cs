namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;

    public class Score
    {
        public Score()
        {

        }
        public Score(int officialId)
        {
            this.OfficialId = officialId;
        }

        [XmlElement("link")]
        //[XmlIgnore]
        public Link[] Link { get; set; }

        [XmlAttribute("adjudicator")]
        public int OfficialId { get; set; }
    }
}
