using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wdsf.Api.Client
{
    public static class FilterNames
    {
        public static class Competition
        {
            /// <summary>
            /// List all competitions since a date. The date is formated according to ISO YYYY/MM/DD.
            /// </summary>
            public static string Since = "from";
            /// <summary>
            /// List all competitions until a date. The date is formated according to ISO YYYY/MM/DD.
            /// </summary>
            public static string Until = "to";
            /// <summary>
            /// List all competitions having a certain status. Allowed values are PreRegistration, Registering, RegistrationClosed, Processing, Closed and Canceled.
            /// </summary>
            public static string Status = "status";
            /// <summary>
            /// List competition that take/took place in a certain city. This starts matching the begining of a city's name.
            /// </summary>
            public static string Location = "location";
            /// <summary>
            /// List only competitions af a certain division. Allowed values are General and Professional.
            /// </summary>
            public static string Division = "divivion";
        }
        public static class Person
        {
            /// <summary>
            /// Searches a person whos name starts with this value.
            /// </summary>
            public static string Name = "name";
            /// <summary>
            /// This filter works in combination with the name filter. If it is set to "true" the name is searched according to SOUNDEX rules.
            /// </summary>
            public static string Phonetic = "phonetic";
            /// <summary>
            /// Search a person having a certian member ID number. The leading 1 can be ommited.
            /// </summary>
            public static string MIN = "min";
            /// <summary>
            /// Works as a combination of "Name" and "MIN" filter.
            /// </summary>
            public static string NameOrMin = "nameOrMin";
            /// <summary>
            /// Search for persons of a certain age group. Valid values are Adult, Senior I, Senior II, Youth, ...
            /// </summary>
            public static string AgeGroup = "ageGroup";
            /// <summary>
            /// Search for persons being part of a divivion. Allowed values are General and Professional.
            /// </summary>
            public static string Division = "division";
            /// <summary>
            /// Search for persons of a certain type. Allowed values are:Athlete, Adjudicator or Chairman
            /// </summary>
            public static string Type = "type";
        }
        public static class Couple
        {
            /// <summary>
            /// Searches couples where one member's name starts with this value.
            /// </summary>
            public static string Name = "name";
            /// <summary>
            /// This filter works in combination with the name filter. If it is set to "true" the name is searched according to SOUNDEX rules.
            /// </summary>
            public static string Phonetic = "phonetic";
            /// <summary>
            /// Search couples where one member has a certain member ID number. The leading 1 can be ommited.
            /// </summary>
            public static string MIN = "min";
            /// <summary>
            /// Works as a combination of "Name" and "MIN" filter.
            /// </summary>
            public static string NameOrMin = "nameOrMin";
            /// <summary>
            /// Search for couples of certain age group. Valid values are Adult, Senior I, Senior II, Youth, ...
            /// </summary>
            public static string AgeGroup = "ageGroup";
            /// <summary>
            /// Search for couples being part of a divivion. Allowed values are General and Professional.
            /// </summary>
            public static string Division = "division";
         }

        public static class Ranking
        {
            /// <summary>
            /// WRL of a certain date. The value is an ISO date, YYYY/MM/DD
            /// </summary>
            public static string Name = "date";

            /// <summary>
            /// WRL of a certain discipline. Allowd values are Latin, Standard and Ten Dance
            /// </summary>
            public static string Discipline = "discipline";
            /// <summary>
            /// WRL of a certain age group. Valid values are Adult, Senior I, Senior II, Youth, ...
            /// </summary>
            public static string AgeGroup = "ageGroup";
            /// <summary>
            /// WRL of a certain division. Allowed values are General and Professional.
            /// </summary>
            public static string Division = "division";
        }
    }
}
