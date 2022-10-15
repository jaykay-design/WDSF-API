namespace Wdsf.Api.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Security;
    using Wdsf.Api.Client.Exceptions;
    using Wdsf.Api.Client.Interfaces;
    using Wdsf.Api.Client.Models;

    /// <summary>
    /// <para>Provides access to the WDSF API throught stongly typed models.</para>
    /// <para>This class is threadsafe and multiple request can be made to the same instance.</para>
    /// </summary>
    public class Client : IDisposable, IClient
    {
        private readonly string username;
        private readonly string password;
        private readonly string onBehalfOf;
        private readonly string apiUriBase;

        private List<RestAdapter> adapters;
        private readonly object adapterLock = new object();

        ///<inheritdoc/>
        public ContentTypes ContentType { get; set; }

        ///<inheritdoc/>
        public string LastApiMessage { get; private set; }

        ///<inheritdoc/>
        public Client(string username, SecureString password, WdsfEndpoint endPoint, string onBehalfOf = null) :
            this(username, ExtractSecureString(password), endPoint == WdsfEndpoint.Services ? "https://services.worlddancesport.org/API/1/" : "https://sandbox.worlddancesport.org/API/1/", onBehalfOf)
        {
        }

        ///<inheritdoc/>
        public Client(string username, string password, WdsfEndpoint endPoint, string onBehalfOf = null) :
            this(username, password, endPoint == WdsfEndpoint.Services ? "https://services.worlddancesport.org/API/1/" : "https://sandbox.worlddancesport.org/API/1/", onBehalfOf)
        {
        }

        ///<inheritdoc/>
        public Client(string username, string password, string baseUrl, string onBehalfOf = null)
        {
            apiUriBase = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
            this.username = username ?? throw new ArgumentNullException(nameof(username));
            this.password = password ?? throw new ArgumentNullException(nameof(password));
            this.onBehalfOf = onBehalfOf;

            this.adapters = new List<RestAdapter>() { };

#if DEBUG
            // Ignore TLS errors when in debug mode (self signed certs)
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
#endif
        }

        private static string ExtractSecureString(SecureString val)
        {
            if (val == null)
                return null;

            return val.ToString();
        }


        ///<inheritdoc/>
        public IList<Person> GetPersons(IDictionary<string, string> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            string query = string.Join("&", filter.Select(e => e.Key + "=" + e.Value).ToArray());

            return GetResourceList<ListOfPerson, Person>("person?" + query);
        }
        ///<inheritdoc/>
        public PersonDetail GetPerson(int min)
        {
            return GetResource<PersonDetail>("person/" + min);
        }
        ///<inheritdoc/>
        public bool UpdatePerson(PersonDetail person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            return UpdateResource<PersonDetail>(person, "person/" + person.Min);
        }


        ///<inheritdoc/>
        public IList<Competition> GetCompetitions(IDictionary<string, string> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            string query = string.Join("&", filter.Select(e => e.Key + "=" + e.Value).ToArray());

            return GetResourceList<ListOfCompetition, Competition>("competition?" + query);
        }
        ///<inheritdoc/>
        public CompetitionDetail GetCompetition(int id)
        {
            return GetResource<CompetitionDetail>("competition/" + id);
        }
        ///<inheritdoc/>
        public bool UpdateCompetition(CompetitionDetail competition)
        {
            if (competition == null)
                throw new ArgumentNullException(nameof(competition));

            // not used for updating
            competition.Links = null;

            return UpdateResource(competition, "competition/" + competition.Id);
        }

        ///<inheritdoc/>
        public ParticipantCoupleDetail GetCoupleParticipant(int id)
        {
            return GetResource<ParticipantCoupleDetail>("participant/" + id);
        }
        ///<inheritdoc/>
        public IList<ParticipantCouple> GetCoupleParticipants(int competitionId)
        {
            return GetResourceList<ListOfCoupleParticipant, ParticipantCouple>(
                "participant?competitionId=" + competitionId
            );
        }
        ///<inheritdoc/>
        public bool UpdateCoupleParticipant(ParticipantCoupleDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException(nameof(participant));

            ClearLinks(participant);

            return UpdateResource(participant, "participant/" + participant.Id);
        }
        ///<inheritdoc/>
        public Uri SaveCoupleParticipant(ParticipantCoupleDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException(nameof(participant));

            ClearLinks(participant);

            return SaveResource(participant, "participant");
        }
        ///<inheritdoc/>
        public bool DeleteCoupleParticipant(int id)
        {
            return DeleteResource("participant/" + id);
        }


        ///<inheritdoc/>
        public ParticipantTeamDetail GetTeamParticipant(int id)
        {
            return GetResource<ParticipantTeamDetail>("participant/" + id);
        }
        ///<inheritdoc/>
        public IList<ParticipantTeam> GetTeamParticipants(int competitionId)
        {
            return GetResourceList<ListOfTeamParticipant, ParticipantTeam>(
                "participant?competitionId=" + competitionId
            );
        }
        ///<inheritdoc/>
        public bool UpdateTeamParticipant(ParticipantTeamDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException(nameof(participant));

            ClearLinks(participant);

            return UpdateResource(participant, "participant/" + participant.Id);
        }
        ///<inheritdoc/>
        public Uri SaveTeamParticipant(ParticipantTeamDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException(nameof(participant));

            ClearLinks(participant);

            return SaveResource(participant, "participant");
        }
        ///<inheritdoc/>
        public bool DeleteTeamParticipant(int id)
        {
            return DeleteResource("participant/" + id);
        }


        ///<inheritdoc/>
        public ParticipantSingleDetail GetSingleParticipant(int id)
        {
            return GetResource<ParticipantSingleDetail>("participant/" + id);
        }
        ///<inheritdoc/>
        public IList<ParticipantSingle> GetSingleParticipants(int competitionId)
        {
            return GetResourceList<ListOfSingleParticipant, ParticipantSingle>(
                "participant?competitionId=" + competitionId
            );
        }
        ///<inheritdoc/>
        public bool UpdateSingleParticipant(ParticipantSingleDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException(nameof(participant));

            ClearLinks(participant);

            return UpdateResource(participant, "participant/" + participant.Id);
        }
        ///<inheritdoc/>
        public Uri SaveSingleParticipant(ParticipantSingleDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException(nameof(participant));

            ClearLinks(participant);

            return SaveResource(participant, "participant");
        }
        ///<inheritdoc/>
        public bool DeleteSingleParticipant(int id)
        {
            return DeleteResource("participant/" + id);
        }


        ///<inheritdoc/>
        public bool UploadResults(IEnumerable<Result> results, int competitionId)
        {
            if (results == null)
                throw new ArgumentNullException(nameof(results));

            if (results.Count() == 0)
                return true;

            var model = new ListOfResults();
            model.AddRange(results);

            return UpdateResource(model, "result/" + competitionId);
        }

        ///<inheritdoc/>
        public OfficialDetail GetOfficial(int id)
        {
            return GetResource<OfficialDetail>("official/" + id);
        }
        ///<inheritdoc/>
        public IList<Official> GetOfficials(int competitionId)
        {
            return GetResourceList<ListOfOfficial, Official>(
                "official?competitionId=" + competitionId
            );
        }
        ///<inheritdoc/>
        public bool UpdateOfficial(OfficialDetail official)
        {
            if (official == null)
                throw new ArgumentNullException(nameof(official));

            return UpdateResource(official, "official/" + official.Id);
        }
        ///<inheritdoc/>
        public Uri SaveOfficial(OfficialDetail official)
        {
            if (official == null)
                throw new ArgumentNullException(nameof(official));

            return SaveResource(official, "official");
        }
        ///<inheritdoc/>
        public bool DeleteOfficial(int id)
        {
            return DeleteResource("official/" +  id);
        }


        ///<inheritdoc/>
        public IList<CoupleExport> GetCouples()
        {
            return GetResourceList<ListOfCoupleExport, CoupleExport>("couple/export");
        }
        ///<inheritdoc/>
        public IList<Couple> GetCouples(IDictionary<string, string> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            string query = string.Join("&", filter.Select(e => e.Key + "=" + e.Value).ToArray());

            return GetResourceList<ListOfCouple, Couple>("couple?" + query);
        }
        ///<inheritdoc/>
        public CoupleDetail GetCouple(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Couple ID must be specified.", nameof(id));

            return GetResource<CoupleDetail>("couple/" + id);
        }
        ///<inheritdoc/>
        public bool UpdateCouple(CoupleDetail couple)
        {
            if (couple == null)
                throw new ArgumentNullException(nameof(couple));

            return UpdateResource(couple, "couple/" + couple.Id);
        }
        ///<inheritdoc/>
        public Uri SaveCouple(CoupleDetail couple)
        {
            if (couple == null)
                throw new ArgumentNullException(nameof(couple));

            return SaveResource(couple, "couple");
        }

        ///<inheritdoc/>
        public IList<Team> GetTeams(IDictionary<string, string> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            string query = string.Join("&", filter.Select(e => e.Key + "=" + e.Value).ToArray());

            return GetResourceList<ListOfTeam, Team>("team?" + query);
        }
        ///<inheritdoc/>
        public TeamDetail GetTeam(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("team ID must be specified.", nameof(id));

            return GetResource<TeamDetail>("team/" + id);
        }
        ///<inheritdoc/>
        public bool UpdateTeam(TeamDetail team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team));

            return UpdateResource(team, "team/" + team.Id);
        }
        ///<inheritdoc/>
        public Uri SaveTeam(TeamDetail team)
        {
            if (team == null)
                throw new ArgumentNullException(nameof(team));

            return SaveResource(team, "team");
        }

        ///<inheritdoc/>
        public IList<Ranking> GetWorldRanking(string discipline, string ageGroup, string division)
        {
            return GetWorldRanking(discipline, ageGroup, division, DateTime.MinValue);
        }
        ///<inheritdoc/>
        public IList<Ranking> GetWorldRanking(string discipline, string ageGroup, string division, DateTime date)
        {
            if (string.IsNullOrEmpty(discipline))
                throw new ArgumentException("Discipline must be set.", nameof(discipline));

            if (string.IsNullOrEmpty(ageGroup))
                throw new ArgumentException("Age group must be set.", nameof(ageGroup));

            if (string.IsNullOrEmpty(division))
                throw new ArgumentException("Division must be set.", nameof(division));

            string resourceUri = $"ranking?agegroup={ageGroup}&discipline={discipline}&division={division}";
            if (date != DateTime.MinValue)
                resourceUri += "&date=" + date.ToString("yyyy/MM/dd");

            return GetResourceList<ListOfRanking, Ranking>(resourceUri);
        }

        ///<inheritdoc/>
        public IList<AgeClass> GetAges()
        {
            return GetResourceList<ListOfAgeClass, AgeClass>("age");
        }

        ///<inheritdoc/>
        public IList<Country> GetCountries()
        {
            return GetResourceList<ListOfCountry, Country>("country");
        }


        /// <summary>
        /// Gets a resource.
        /// </summary>
        /// <typeparam name="T">The expected resource type</typeparam>
        /// <param name="resourceUri">The resourceUri</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The resource</returns>
        public T Get<T>(Uri resourceUri) where T : class
        {
            if (null == resourceUri)
                throw new ArgumentNullException(nameof(resourceUri));

            var adapter = GetAdapter();
            try
            {
                return adapter.Get<T>(resourceUri);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
            finally
            {
                ReleaseAdapter(adapter);
            }
        }

        /// <summary>
        /// Gets a resource where the expected type is not know.
        /// </summary>
        /// <param name="resourceUri">The resourceUri</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The resource</returns>
        public object Get(Uri resourceUri)
        {
            if (null == resourceUri)
                throw new ArgumentNullException(nameof(resourceUri));

            var adapter = GetAdapter();
            try
            {
                return adapter.Get(resourceUri);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
            finally
            {
                ReleaseAdapter(adapter);
            }
        }

        private T GetResource<T>(string resourceUri) where T : class
        {
            var adapter = GetAdapter();
            try
            {
                return adapter.Get<T>(new Uri(this.apiUriBase + resourceUri));
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
            finally
            {
                ReleaseAdapter(adapter);
            }

        }
        private IList<TItem> GetResourceList<TContainer, TItem>(string resourceUri)
            where TContainer : class, IEnumerable<TItem>
            where TItem : class
        {

            var adapter = GetAdapter();
            try
            {
                var items = adapter.Get<TContainer>(new Uri(this.apiUriBase + resourceUri));
                return new List<TItem>(items);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
            finally
            {
                ReleaseAdapter(adapter);
            }

        }
        private bool UpdateResource<T>(T model, string resourceUri) where T : class
        {
            this.LastApiMessage = string.Empty;

            StatusMessage message;
            var adapter = GetAdapter();
            try
            {
                message = adapter.Put(new Uri(this.apiUriBase + resourceUri), model);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
            finally
            {
                ReleaseAdapter(adapter);
            }


            this.LastApiMessage = message.Message;
            return message.Code == (int)HttpStatusCode.OK;
        }
        private Uri SaveResource<T>(T participant, string resourceUri) where T : class
        {
            this.LastApiMessage = string.Empty;

            StatusMessage message;
            var adapter = GetAdapter();
            try
            {
                message = adapter.Post<T>(new Uri(this.apiUriBase + resourceUri), participant);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
            finally
            {
                ReleaseAdapter(adapter);
            }


            if (message.Code != (int)HttpStatusCode.Created)
            {
                throw new ApiException(new RestException(message));
            }

            this.LastApiMessage = message.Message;
            return new Uri(message.Link.HRef);
        }
        private bool DeleteResource(string resourceUri)
        {
            this.LastApiMessage = string.Empty;

            var adapter = GetAdapter();
            try
            {
                var message = adapter.Delete(new Uri(this.apiUriBase + resourceUri));
                this.LastApiMessage = message.Message;
                return message.Code == (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
            finally
            {
                ReleaseAdapter(adapter);
            }

        }

        /// <summary>
        /// Removes the Link property of all Score types to reduce the model's overall size.
        /// </summary>
        /// <param name="participant"></param>
        private void ClearLinks(ParticipantBaseDetail participant)
        {
            participant.Links = null;

            if (participant.Rounds == null)
            {
                return;
            }

            foreach (var round in participant.Rounds)
            {
                foreach (var dance in round.Dances)
                {
                    foreach (var score in dance.Scores)
                    {
                        score.Link = null;
                    }
                }
            }
        }

        private RestAdapter GetAdapter()
        {
            lock (adapterLock)
            {
                if (null == this.adapters)
                    throw new ObjectDisposedException(GetType().FullName);

                var adapter = this.adapters.FirstOrDefault(a => a.IsBusy == false && a.IsAssigned == false);
                if (adapter == null)
                {
                    adapter = new RestAdapter(this.username, this.password, this.onBehalfOf)
                    {
                        ContentType = this.ContentType
                    };
                    this.adapters.Add(adapter);
                }

                adapter.IsAssigned = true;
                return adapter;
            }
        }
        private void ReleaseAdapter(RestAdapter adapter)
        {
            lock (adapterLock)
            {
                adapter.IsAssigned = false;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (this.adapterLock)
                {
                    if (this.adapters != null)
                    {
                        foreach (var adapter in this.adapters)
                        {
                            adapter.Dispose();
                        }

                        this.adapters = null;
                    }
                }
            }
        }
    }
}
