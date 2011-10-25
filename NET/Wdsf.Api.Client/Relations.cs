/*  Copyright (C) 2011-2012 JayKay-Design S.C.
    Author: John Caprez jay@jaykay-design.com

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU LEsser General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/lgpl.html>.
 */

namespace Wdsf.Api.Client
{
    public static class ResourceRelation
    {
        /// <summary>
        /// The resource itself
        /// </summary>
        public static readonly string Self = "self";
        /// <summary>
        /// A competition
        /// </summary>
        public static readonly string Competition = "http://services.worlddancesport.org/rel/competition";
        /// <summary>
        /// The participants of a competition.
        /// </summary>
        public static readonly string CompetitionParticipants = "http://services.worlddancesport.org/rel/competition/participants";
        /// <summary>
        /// The officials of a competition
        /// </summary>
        public static readonly string CompetitionOfficials = "http://services.worlddancesport.org/rel/competition/officials";
        /// <summary>
        /// A participant
        /// </summary>
        public static readonly string Particiapnt = "http://services.worlddancesport.org/rel/participant";
        /// <summary>
        /// The competition a participant has taken part at
        /// </summary>
        public static readonly string ParticipantCompetition = "http://services.worlddancesport.org/rel/participant/competition";
        /// <summary>
        /// The couple related to this participant
        /// </summary>
        public static readonly string ParticipantCouple = "http://services.worlddancesport.org/rel/participant/couple";
        /// <summary>
        /// The team related to this participant
        /// </summary>
        public static readonly string ParticipantTeam = "http://services.worlddancesport.org/rel/participant/team";
        /// <summary>
        /// A couple
        /// </summary>
        public static readonly string Couple = "http://services.worlddancesport.org/rel/couple";
        /// <summary>
        /// The man of a couple
        /// </summary>
        public static readonly string Man = "http://services.worlddancesport.org/rel/couple/man";
        /// <summary>
        /// The woman of a couple
        /// </summary>
        public static readonly string Woman = "http://services.worlddancesport.org/rel/couple/woman";
        /// <summary>
        /// A team
        /// </summary>
        public static readonly string Team = "http://services.worlddancesport.org/rel/team";
        /// <summary>
        /// A person
        /// </summary>
        public static readonly string Person = "http://services.worlddancesport.org/rel/person";
        /// <summary>
        /// The current partner of a person.
        /// </summary>
        public static readonly string PersonPartner = "http://services.worlddancesport.org/rel/person/partner";
        /// <summary>
        /// An official
        /// </summary>
        public static readonly string Official = "http://services.worlddancesport.org/rel/official";
        /// <summary>
        /// The Person acting as an official
        /// </summary>
        public static readonly string OfficialPerson = "http://services.worlddancesport.org/rel/official/person";
        /// <summary>
        /// The competition an official acts at
        /// </summary>
        public static readonly string OfficialCompetition = "http://services.worlddancesport.org/rel/official/competition";

    }
}
