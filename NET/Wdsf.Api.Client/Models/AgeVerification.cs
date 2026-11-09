namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Xml.Serialization;

    /// <summary>
    /// Whether an athlete (one MIN) or a couple (two MIN) may take part in a competition.
    /// </summary>
    [XmlType("age_verification", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("age_verification")]
    public class AgeVerification
    {
        [XmlElement("competitionId")]
        [JsonProperty("competitionId")]
        public int CompetitionId { get; set; }

        [XmlElement("permitted")]
        [JsonProperty("permitted")]
        public bool IsPermitted { get; set; }

        /// <summary>
        /// When not permitted, the reason; otherwise empty.
        /// </summary>
        [XmlElement("reason")]
        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}
