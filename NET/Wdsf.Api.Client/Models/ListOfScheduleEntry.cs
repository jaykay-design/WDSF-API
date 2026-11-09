namespace Wdsf.Api.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Wdsf.Api.Client.Attributes;

    [XmlType("schedules", Namespace = "http://services.worlddancesport.org/api")]
    [XmlRoot("schedules", Namespace = "http://services.worlddancesport.org/api")]
    [MediaType("application/vnd.worlddancesport.schedules", IsCollection = true)]
    [JsonArray("schedules")]
    public class ListOfScheduleEntry : List<ScheduleEntry>
    {
    }
}
