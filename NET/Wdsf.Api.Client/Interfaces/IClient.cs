namespace Wdsf.Api.Client.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Wdsf.Api.Client.Exceptions;
    using Wdsf.Api.Client.Models;

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

        T Get<T>(Uri resourceUri) where T : class;
        object Get(Uri resourceUri);

        void Dispose();
    }
}
