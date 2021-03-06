﻿namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Xml.Serialization;

    [XmlType("competition", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("competition", Namespace = "http://services.worlddancesport.org/api")]
    [JsonObject("competition")]
    public class Competition : EntityWithLinks
    {
        [XmlElement("id")]
        [JsonProperty("id")]
        public int Id { get; set; }

        [XmlElement("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [XmlElement("lastmodifiedDate")]
        [JsonProperty("lastmodifiedDate")]
        public DateTime LastModifiedDate { get; set; }

    }
}