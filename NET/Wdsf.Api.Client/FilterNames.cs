namespace Wdsf.Api.Client
{
    using System;

    /// <summary>
    /// The query parameter names of the list endpoints. Pass them as keys of the filter dictionary.
    /// </summary>
    public sealed class FilterNames
    {
        /// <summary>
        /// Paging of the list endpoints that support it (competitions, persons, couples, teams).
        /// </summary>
        public sealed class Paging
        {
            /// <summary>
            /// The page to return, starting at 1.
            /// </summary>
            public const string Page = "page";
            /// <summary>
            /// The number of entries per page (at least 1).
            /// </summary>
            public const string PageSize = "pageSize";
        }

        public sealed class Competition
        {
            /// <summary>
            /// List all competitions since a date (yyyy-MM-dd).
            /// </summary>
            public const string Since = "from";
            /// <summary>
            /// List all competitions until a date (yyyy-MM-dd).
            /// </summary>
            public const string Until = "to";
            /// <summary>
            /// List the competitions changed since a date and time (ISO 8601).
            /// </summary>
            public const string ModifiedSince = "modifiedSince";
            /// <summary>
            /// "true" lists only competitions counting for the world ranking.
            /// </summary>
            public const string WorldRanking = "worldRanking";
            /// <summary>
            /// List all competitions having a certain status. Allowed values are PreRegistration, Registering,
            /// RegistrationClosed, InProgress, Closed, Canceled, Processing, Postponed and PostponedTBC.
            /// Repeat the parameter for several statuses.
            /// </summary>
            public const string Status = "status";
            /// <summary>
            /// List competition that take/took place in a certain city. This starts matching the begining of a city's name.
            /// </summary>
            public const string Location = "location";
            /// <summary>
            /// List only competitions of a certain division. Allowed values are General and Professional.
            /// </summary>
            public const string Division = "division";
            /// <summary>
            /// List only competitions of a certain age group (its name, e.g. Adult).
            /// </summary>
            public const string AgeGroup = "ageGroup";
            /// <summary>
            /// List only the competitions of an event group (its id).
            /// </summary>
            public const string GroupId = "groupId";
        }
        public sealed class Person
        {
            /// <summary>
            /// Searches a person whos name starts with this value.
            /// </summary>
            public const string Name = "name";
            /// <summary>
            /// This filter works in combination with the name filter. If it is set to "true" the name is searched according to SOUNDEX rules.
            /// </summary>
            public const string Phonetic = "phonetic";
            /// <summary>
            /// Search a person having a certian member ID number. The leading 1 can be ommited.
            /// </summary>
            public const string MIN = "min";
            /// <summary>
            /// Works as a combination of "Name" and "MIN" filter.
            /// </summary>
            public const string NameOrMin = "nameOrMin";
            /// <summary>
            /// Search for persons of a certain age group. Valid values are Adult, Senior I, Senior II, Youth, ...
            /// </summary>
            public const string AgeGroup = "ageGroup";
            /// <summary>
            /// Search for persons holding a licence of a division (General, Professional, ...). Matches exactly.
            /// </summary>
            public const string Division = "division";
            /// <summary>
            /// Search for persons holding a licence of a type, e.g. Athlete, Adjudicator, Chairperson or ITC.
            /// </summary>
            public const string Type = "type";
            /// <summary>
            /// The licence status to match (default Active). Pass Provisional to list provisional ITC licences.
            /// </summary>
            public const string Status = "status";
            /// <summary>
            /// Search for persons of a country (its WDSF short name).
            /// </summary>
            public const string Country = "country";
        }
        public sealed class Couple
        {
            /// <summary>
            /// Searches couples where one member's name starts with this value.
            /// </summary>
            public const string Name = "name";
            /// <summary>
            /// This filter works in combination with the name filter. If it is set to "true" the name is searched according to SOUNDEX rules.
            /// </summary>
            public const string Phonetic = "phonetic";
            /// <summary>
            /// Search couples where one member has a certain member ID number. The leading 1 can be ommited.
            /// </summary>
            public const string MIN = "min";
            /// <summary>
            /// Works as a combination of "Name" and "MIN" filter.
            /// </summary>
            public const string NameOrMin = "nameOrMin";
            /// <summary>
            /// Search for couples of certain age group. Valid values are Adult, Senior I, Senior II, Youth, ...
            /// </summary>
            public const string AgeGroup = "ageGroup";
            /// <summary>
            /// Search for couples being part of a divivion. Allowed values are General and Professional.
            /// </summary>
            public const string Division = "division";
            /// <summary>
            /// Search for couples of a status (e.g. Active, Retired).
            /// </summary>
            public const string Status = "status";
            /// <summary>
            /// Search for couples of a country (its WDSF short name).
            /// </summary>
            public const string Country = "country";
        }
        public sealed class Team
        {
            /// <summary>
            /// Searches teams whose name starts with this value.
            /// </summary>
            public const string Name = "name";
            /// <summary>
            /// Search for teams of a division.
            /// </summary>
            public const string Division = "division";
        }

        public sealed class Group
        {
            /// <summary>
            /// List the groups at a location (exact match).
            /// </summary>
            public const string Location = "location";
            /// <summary>
            /// List the groups ending on or after a date (yyyy-MM-dd).
            /// </summary>
            public const string FromDate = "fromDate";
            /// <summary>
            /// List the groups starting on or before a date (yyyy-MM-dd).
            /// </summary>
            public const string ToDate = "toDate";
        }

        public sealed class Ranking
        {
            /// <summary>
            /// WRL of a certain date (yyyy-MM-dd).
            /// </summary>
            public const string Date = "date";
            /// <summary>
            /// WRL of a certain date (yyyy-MM-dd).
            /// </summary>
            [Obsolete("Use Date.")]
            public const string Name = Date;

            /// <summary>
            /// WRL of a certain discipline. Allowd values are Latin, Standard and Ten Dance
            /// </summary>
            public const string Discipline = "discipline";
            /// <summary>
            /// WRL of a certain age group. Valid values are Adult, Senior I, Senior II, Youth, ...
            /// </summary>
            public const string AgeGroup = "ageGroup";
            /// <summary>
            /// WRL of a certain division. Allowed values are General and Professional.
            /// </summary>
            public const string Division = "division";
            /// <summary>
            /// The dance form, for disciplines that have several.
            /// </summary>
            public const string Form = "form";
            /// <summary>
            /// The gender of a solo ranking (Male or Female).
            /// </summary>
            public const string Gender = "gender";
            /// <summary>
            /// The maximum number of rows (0 or absent: all).
            /// </summary>
            public const string Limit = "limit";
        }
    }
}
