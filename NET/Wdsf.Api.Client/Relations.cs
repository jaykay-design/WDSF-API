namespace Wdsf.Api.Client
{
    public static class ResourceRelation
    {
        /// <summary>
        /// The resource itself
        /// </summary>
        public const string Self = "self";
        /// <summary>
        /// A competition
        /// </summary>
        public const string Competition = "http://services.worlddancesport.org/rel/competition";
        /// <summary>
        /// The participants of a competition.
        /// </summary>
        public const string CompetitionParticipants = "http://services.worlddancesport.org/rel/competition/participants";
        /// <summary>
        /// The officials of a competition
        /// </summary>
        public const string CompetitionOfficials = "http://services.worlddancesport.org/rel/competition/officials";
        /// <summary>
        /// A participant
        /// </summary>
        public const string Participant = "http://services.worlddancesport.org/rel/participant";
        /// <summary>
        /// The competition a participant has taken part at
        /// </summary>
        public const string ParticipantCompetition = "http://services.worlddancesport.org/rel/participant/competition";
        /// <summary>
        /// The couple related to this participant
        /// </summary>
        public const string ParticipantCouple = "http://services.worlddancesport.org/rel/participant/couple";
        /// <summary>
        /// The team related to this participant
        /// </summary>
        public const string ParticipantTeam = "http://services.worlddancesport.org/rel/participant/team";
        /// <summary>
        /// A couple
        /// </summary>
        public const string Couple = "http://services.worlddancesport.org/rel/couple";
        /// <summary>
        /// The man of a couple
        /// </summary>
        public const string Man = "http://services.worlddancesport.org/rel/couple/man";
        /// <summary>
        /// The woman of a couple
        /// </summary>
        public const string Woman = "http://services.worlddancesport.org/rel/couple/woman";
        /// <summary>
        /// A team
        /// </summary>
        public const string Team = "http://services.worlddancesport.org/rel/team";
        /// <summary>
        /// A person
        /// </summary>
        public const string Person = "http://services.worlddancesport.org/rel/person";
        /// <summary>
        /// The current partner of a person.
        /// </summary>
        public const string PersonPartner = "http://services.worlddancesport.org/rel/person/partner";
        /// <summary>
        /// An official
        /// </summary>
        public const string Official = "http://services.worlddancesport.org/rel/official";
        /// <summary>
        /// The Person acting as an official
        /// </summary>
        public const string OfficialPerson = "http://services.worlddancesport.org/rel/official/person";
        /// <summary>
        /// The competition an official acts at
        /// </summary>
        public const string OfficialCompetition = "http://services.worlddancesport.org/rel/official/competition";

    }
}
