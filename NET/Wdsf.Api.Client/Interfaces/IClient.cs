namespace Wdsf.Api.Client.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Wdsf.Api.Client.Exceptions;
    using Wdsf.Api.Client.Models;

    /// <summary>
    /// <para>Provides access to the WDSF API throught stongly typed models.</para>
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// This string contains the message returned from the last API call.
        /// </summary>
        string LastApiMessage { get; }

        /// <summary>
        /// Gets a list of competitions.
        /// </summary>
        /// <param name="filter">A dictionaly containing filter parameters. See API documentation for details.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>A list of competitions.</returns>
        IList<Competition> GetCompetitions(IDictionary<string, string> filter);
        /// <summary>
        /// Gets a competition
        /// </summary>
        /// <param name="id">The competition ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The competition</returns>
        CompetitionDetail GetCompetition(int id);
        /// <summary>
        /// Updates a competition
        /// </summary>
        /// <param name="competition">The competition model</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        bool UpdateCompetition(CompetitionDetail competition);

        /// <summary>
        /// Get the participants of a competition
        /// </summary>
        /// <param name="competitionId">The competition Id</param>
        /// <returns>List of participants</returns>
        IList<ParticipantCouple> GetCoupleParticipants(int competitionId);
        /// <summary>
        /// Saves a new participant (couple).
        /// </summary>
        /// <param name="participant">The participant model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The Uri to the newly created resource</returns>
        Uri SaveCoupleParticipant(ParticipantCoupleDetail participant);
        /// <summary>
        /// Gets a participant (couple)
        /// </summary>
        /// <param name="id">The participant ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The participant (couple)</returns>
        ParticipantCoupleDetail GetCoupleParticipant(int id);
        /// <summary>
        /// Updates a participant (couple).
        /// </summary>
        /// <param name="participant">The participant model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        bool UpdateCoupleParticipant(ParticipantCoupleDetail participant);
        /// <summary>
        /// Delete a participant (couple)
        /// </summary>
        /// <param name="id">The participant ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>true on success.</returns>
        bool DeleteCoupleParticipant(int id);

        /// <summary>
        /// Get the participants of a competition
        /// </summary>
        /// <param name="competitionId">The competition Id</param>
        /// <returns>List of participants</returns>
        IList<ParticipantTeam> GetTeamParticipants(int competitionId);
        /// <summary>
        /// Saves a new participant (team).
        /// </summary>
        /// <param name="participant">The participant model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The Uri to the newly created resource</returns>
        Uri SaveTeamParticipant(ParticipantTeamDetail participant);
        /// <summary>
        /// Gets a participant (team)
        /// </summary>
        /// <param name="id">The participant ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The participant (team)</returns>
        ParticipantTeamDetail GetTeamParticipant(int id);
        /// <summary>
        /// Updates a participant (team).
        /// </summary>
        /// <param name="participant">The participant model.</param>
        /// <exception cref="UnknownMediaTypeException">The received resource type was not recognized.</exception>
        /// <exception cref="UnexpectedMediaTypeException">The received resource type was not expected.</exception>
        bool UpdateTeamParticipant(ParticipantTeamDetail participant);
        /// <summary>
        /// Delete a participant (team)
        /// </summary>
        /// <param name="id">The participant ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>true on success.</returns>
        bool DeleteTeamParticipant(int id);

        /// <summary>
        /// Get the participants of a competition
        /// </summary>
        /// <param name="competitionId">The competition Id</param>
        /// <returns>List of participants</returns>
        IList<ParticipantSingle> GetSingleParticipants(int competitionId);
        /// <summary>
        /// Saves a new participant (single).
        /// </summary>
        /// <param name="participant">The participant model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The Uri to the newly created resource</returns>
        Uri SaveSingleParticipant(ParticipantSingleDetail participant);
        /// <summary>
        /// Gets a participant (single)
        /// </summary>
        /// <param name="id">The participant ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The participant (single)</returns>
        ParticipantSingleDetail GetSingleParticipant(int id);
        /// <summary>
        /// Updates a participant (single).
        /// </summary>
        /// <param name="participant">The participant model.</param>
        /// <exception cref="UnknownMediaTypeException">The received resource type was not recognized.</exception>
        /// <exception cref="UnexpectedMediaTypeException">The received resource type was not expected.</exception>
        bool UpdateSingleParticipant(ParticipantSingleDetail participant);
        /// <summary>
        /// Delete a participant (single)
        /// </summary>
        /// <param name="id">The participant ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>true on success.</returns>
        bool DeleteSingleParticipant(int id);

        /// <summary>
        /// Uploads multipel results at once.
        /// </summary>
        /// <param name="results">The results</param>
        /// <param name="competitionId">What competition these belong to</param>
        /// <returns>true on success</returns>
        bool UploadResults(IEnumerable<Result> results, int competitionId);

        /// <summary>
        /// Gets officials of specified competition
        /// </summary>
        /// <param name="competitionId">The competition ID</param>
        /// <returns>The list of officials</returns>
        IList<Official> GetOfficials(int competitionId);
        /// <summary>
        /// Saves a new official.
        /// </summary>
        /// <param name="official">The official model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The Uri to the newly created official</returns>
        Uri SaveOfficial(OfficialDetail official);
        /// <summary>
        /// Gets an official
        /// </summary>
        /// <param name="id">The official ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The official</returns>
        OfficialDetail GetOfficial(int id);
        /// <summary>
        /// Updates an official
        /// </summary>
        /// <param name="official">The official model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        bool UpdateOfficial(OfficialDetail official);
        /// <summary>
        /// Delete am official
        /// </summary>
        /// <param name="id">The official ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>true on success.</returns>
        bool DeleteOfficial(int id);

        /// <summary>
        /// Gets a list of all active couples. This list contains all neccesary information for off-line usage.
        /// </summary>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>A list of couples</returns>
        IList<CoupleExport> GetCouples();
        /// <summary>
        /// Gets a list of couples.
        /// </summary>
        /// <param name="filter">A dictionaly containing filter parameters. See API documentation for details.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>A list of couples</returns>
        IList<Couple> GetCouples(IDictionary<string, string> filter);
        /// <summary>
        /// Saves a new couple.
        /// </summary>
        /// <param name="couple">The official model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The Uri to the newly created couple</returns>
        Uri SaveCouple(CoupleDetail couple);
        /// <summary>
        /// Gets a couple
        /// </summary>
        /// <param name="id">The couple ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The couple</returns>
        CoupleDetail GetCouple(string id);
        /// <summary>
        /// Updates a couple
        /// </summary>
        /// <param name="couple">The couple model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        bool UpdateCouple(CoupleDetail couple);

        /// <summary>
        /// Gets a list of teams.
        /// </summary>
        /// <param name="filter">A dictionaly containing filter parameters. See API documentation for details.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>A list of teams</returns>
        IList<Team> GetTeams(IDictionary<string, string> filter);
        /// <summary>
        /// Saves a new team.
        /// </summary>
        /// <param name="team">The official model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The Uri to the newly created team</returns>
        Uri SaveTeam(TeamDetail team);
        /// <summary>
        /// Gets a team
        /// </summary>
        /// <param name="id">The team ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The team</returns>
        TeamDetail GetTeam(string id);
        /// <summary>
        /// Updates a team
        /// </summary>
        /// <param name="team">The team model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        bool UpdateTeam(TeamDetail team);

        /// <summary>
        /// Gets a list of persons.
        /// </summary>
        /// <param name="filter">A dictionaly containing filter parameters. See API documentation for details.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>A list of persons</returns>
        IList<Person> GetPersons(IDictionary<string, string> filter);
        /// <summary>
        /// Gets a person
        /// </summary>
        /// <param name="min">The member ID number</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The person model.</returns>
        PersonDetail GetPerson(int min);
        /// <summary>
        /// Gets the person export: one row per person with the type and division of each active licence.
        /// </summary>
        /// <param name="filter">A dictionaly containing filter parameters (see <see cref="FilterNames.Person"/>).</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The exported persons</returns>
        IList<PersonExport> GetPersonExport(IDictionary<string, string> filter);
        /// <summary>
        /// Updates a person
        /// </summary>
        /// <param name="person">The person model.</param>
        /// <returns></returns>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        bool UpdatePerson(PersonDetail person);


        /// <summary>
        /// Gets a world ranking list.
        /// </summary>
        /// <param name="discipline">The discipline.</param>
        /// <param name="ageGroup">The age group.</param>
        /// <param name="division">The division.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <exception cref="ArgumentException">Any of the required arguments is missing.</exception>
        /// <returns>The world ranking list</returns>
        IList<Ranking> GetWorldRanking(string discipline, string ageGroup, string division);
        /// <summary>
        /// Gets a world ranking list by a date. The WRL does not exists for all dates.
        /// </summary>
        /// <param name="discipline">The discipline.</param>
        /// <param name="ageGroup">The age group.</param>
        /// <param name="division">The division.</param>
        /// <param name="date">Date to get ranking for. Can be absent.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <exception cref="ArgumentException">Any of the required arguments is missing.</exception>
        /// <returns>The world ranking list</returns>
        IList<Ranking> GetWorldRanking(string discipline, string ageGroup, string division, DateTime date);
        /// <summary>
        /// Gets a solo (single athlete) world ranking list.
        /// </summary>
        /// <param name="discipline">The discipline.</param>
        /// <param name="ageGroup">The age group.</param>
        /// <param name="division">The division.</param>
        /// <param name="gender">Male or Female, for disciplines ranked by gender. Can be absent.</param>
        /// <param name="date">Date to get ranking for. Can be absent.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <exception cref="ArgumentException">Any of the required arguments is missing.</exception>
        /// <returns>The world ranking list</returns>
        IList<RankingSingle> GetSingleWorldRanking(string discipline, string ageGroup, string division, string gender = null, DateTime? date = null);
        /// <summary>
        /// Gets a team world ranking list.
        /// </summary>
        /// <param name="discipline">The discipline.</param>
        /// <param name="ageGroup">The age group.</param>
        /// <param name="division">The division.</param>
        /// <param name="date">Date to get ranking for. Can be absent.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <exception cref="ArgumentException">Any of the required arguments is missing.</exception>
        /// <returns>The world ranking list</returns>
        IList<RankingTeam> GetTeamWorldRanking(string discipline, string ageGroup, string division, DateTime? date = null);

        /// <summary>
        /// Gets all allowed countries.
        /// </summary>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The countries</returns>
        IList<Country> GetCountries();
        /// <summary>
        /// Gets all allowed age groups.
        /// </summary>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The age groups</returns>
        IList<AgeClass> GetAges();

        /// <summary>
        /// Checks whether an athlete (one MIN) or a couple (two MIN) may take part in competitions:
        /// age group, and for a solo athlete a same-day entry in another event group.
        /// </summary>
        /// <param name="mins">One MIN (solo) or two MIN (couple).</param>
        /// <param name="competitionIds">The competitions to check.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <exception cref="ArgumentException">No or more than two MIN, or no competition.</exception>
        /// <returns>One result per competition</returns>
        IList<AgeVerification> CheckAgeForCompetition(IEnumerable<int> mins, IEnumerable<int> competitionIds);

        /// <summary>
        /// Gets a list of event groups.
        /// </summary>
        /// <param name="filter">A dictionaly containing filter parameters (see <see cref="FilterNames.Group"/>).</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The event groups</returns>
        IList<EventGroup> GetEventGroups(IDictionary<string, string> filter);
        /// <summary>
        /// Gets an event group with its competitions.
        /// </summary>
        /// <param name="id">The group id</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The event group</returns>
        EventGroupDetail GetEventGroup(int id);

        /// <summary>
        /// Gets the time schedule of an event group. Scrutineer accounts only.
        /// </summary>
        /// <param name="groupId">The event group id</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The schedule entries</returns>
        IList<ScheduleEntry> GetSchedule(int groupId);
        /// <summary>
        /// Gets a schedule entry. Scrutineer accounts only.
        /// </summary>
        /// <param name="id">The entry id</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The schedule entry</returns>
        ScheduleEntry GetScheduleEntry(Guid id);
        /// <summary>
        /// Saves a new schedule entry. Scrutineer accounts only.
        /// </summary>
        /// <param name="entry">The entry; Time is required.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The Uri to the new entry</returns>
        Uri SaveScheduleEntry(ScheduleEntry entry);
        /// <summary>
        /// Updates a schedule entry; properties left null keep their value. Scrutineer accounts only.
        /// </summary>
        /// <param name="entry">The entry, with its Id.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>true on success.</returns>
        bool UpdateScheduleEntry(ScheduleEntry entry);
        /// <summary>
        /// Deletes a schedule entry. Scrutineer accounts only.
        /// </summary>
        /// <param name="id">The entry id</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>true on success.</returns>
        bool DeleteScheduleEntry(Guid id);

        /// <summary>
        /// Downloads a participant's approved music: one file as itself (usually audio/mpeg), several
        /// as one zip archive. Scrutineer and service-provider accounts only.
        /// </summary>
        /// <param name="participantId">The participant id</param>
        /// <param name="ifModifiedSince">The LastModified of an earlier download; the music is downloaded only if it changed since.</param>
        /// <exception cref="ApiException">The request failed (e.g. no approved music). See inner exception for details.</exception>
        /// <returns>The music, or null when it did not change since ifModifiedSince</returns>
        ApiFile GetParticipantMusic(int participantId, DateTimeOffset? ifModifiedSince = null);
        /// <summary>
        /// Gets the type, file name and last change of a participant's approved music without downloading it.
        /// </summary>
        /// <param name="participantId">The participant id</param>
        /// <exception cref="ApiException">The request failed (e.g. no approved music). See inner exception for details.</exception>
        /// <returns>The music's headers; Content is null</returns>
        ApiFile GetParticipantMusicInfo(int participantId);

        T Get<T>(Uri resourceUri) where T : class;
        object Get(Uri resourceUri);

        void Dispose();
    }
}
