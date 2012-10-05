namespace Wdsf.Api.Client.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Wdsf.Api.Client.Models;

    public interface IClient
    {
        IList<Competition> GetCompetitions(IDictionary<string, string> filter);
        CompetitionDetail GetCompetition(int id);
        bool UpdateCompetition(CompetitionDetail competition);

        Uri SaveCoupleParticipant(ParticipantCoupleDetail participant);
        ParticipantCoupleDetail GetCoupleParticipant(int id);
        bool UpdateCoupleParticipant(ParticipantCoupleDetail participant);
        bool DeleteCoupleParticipant(int id);

        Uri SaveTeamParticipant(ParticipantTeamDetail participant);
        ParticipantTeamDetail GetTeamParticipant(int id);
        bool UpdateTeamParticipant(ParticipantTeamDetail participant);
        bool DeleteTeamParticipant(int id);

        Uri SaveOfficial(OfficialDetail official);
        OfficialDetail GetOfficial(int id);
        bool UpdateOfficial(OfficialDetail official);
        bool DeleteOfficial(int id);

        IList<CoupleExport> GetCouples();
        IList<Couple> GetCouples(IDictionary<string, string> filter);
        Uri SaveCouple(CoupleDetail couple);
        CoupleDetail GetCouple(string id);
        bool UpdateCouple(CoupleDetail couple);

        PersonDetail GetPerson(int min);
        IList<Person> GetPersons(IDictionary<string, string> filter);
        IList<Ranking> GetWorldRanking(string discipline, string ageGroup, string division);

        IList<Country> GetCountries();
        IList<AgeClass> GetAges();

        T Get<T>(Uri resourceUri) where T : class;
        object Get(Uri resourceUri);

        void Dispose();
    }
}
