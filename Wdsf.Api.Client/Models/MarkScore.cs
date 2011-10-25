namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;

    [XmlType("mark", Namespace = "http://services.worlddancesport.org/api")]
    public class MarkScore : Score
    {
        public MarkScore()
            :base()
        {

        }
        public MarkScore(int officialId, bool isSet)
            :base(officialId)
        {
            this.IsSet = isSet;
        }

        [XmlAttribute("set")]
        public bool IsSet { get; set; }

        [XmlIgnore]
        public bool IsSetSpecified { get { return this.IsSet; } set { ;} }
    }
}
