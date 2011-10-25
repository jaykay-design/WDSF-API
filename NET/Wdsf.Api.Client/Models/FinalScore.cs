namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Xml.Serialization;

    [XmlType("final", Namespace = "http://services.worlddancesport.org/api")]
    public class FinalScore : Score
    {
        public FinalScore():
            base()
        {

        }
        public FinalScore(int officialId, int rank):
            base(officialId)
        {
            this.Rank = rank;
        }

        [XmlAttribute("rank")]
        public int Rank { get; set; }
    }
}
