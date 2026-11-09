namespace Wdsf.Api.Client.Models
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Newtonsoft.Json;

    [XmlType("round", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("round")]
    public class Round
    {
        private List<Dance> dances = new List<Dance>();

        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Maximum allowed deviation of scrores in AJS 3.0
        /// </summary>
        [XmlAttribute("maxDeviation")]
        [JsonProperty("maxDeviation")]
        [JsonConverter(typeof(Converter.NumericStringConverter))]
        public string MaxDeviation { get; set; }
        public bool ShouldSerializeMaxDeviation()
        {
            return !string.IsNullOrEmpty(MaxDeviation);
        }

        /// <summary>
        /// The number of participants recalled from this round (Skating rounds).
        /// </summary>
        [XmlAttribute("recalls")]
        [JsonProperty("recalls")]
        [JsonConverter(typeof(Converter.NumericStringConverter))]
        public string Recalls { get; set; }
        public bool ShouldSerializeRecalls()
        {
            return !string.IsNullOrEmpty(Recalls);
        }

        /// <summary>
        /// <para>Do not use this array to process dances.</para>
        /// <para>It is used only as a workaround for .NET's XmlSerializer limitations on deserializing lists.</para>
        /// </summary>
        [XmlArray("dances")]
        [JsonProperty("dances")]
        public Dance[] DancesForSerialization
        {
            get
            {
                return dances.Count == 0 ? null : dances.ToArray();
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                dances = new List<Dance>(value);
            }
        }

        [XmlIgnore, JsonIgnore]
        public IList<Dance> Dances
        {
            get
            {
                return dances;
            }
        }
    }
}
