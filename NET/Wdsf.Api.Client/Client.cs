namespace Wdsf.Api.Client
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
        private readonly string apiKey;
        private readonly string onBehalfOf;
        private readonly string apiUriBase;

        private List<RestAdapter> adapters;
        private readonly object adapterLock = new object();

        ///<inheritdoc/>
        public ContentTypes ContentType { get; set; }

        ///<inheritdoc/>
        public string LastApiMessage { get; private set; }

        /// <summary>
        /// The base URL of the live WDSF API (v2).
        /// </summary>
        public const string ServicesBaseUrl = "https://api.wdsf.org/api/2/";

        /// <summary>
        /// The base URL of the WDSF API sandbox (v2), for testing.
        /// </summary>
        public const string SandboxBaseUrl = "https://api-staging.wdsf.org/api/2/";

        /// <summary>
        /// The request header that carries the API key.
        /// </summary>
        public const string ApiKeyHeaderName = "X-WDSF-API-Key";

        /// <summary>
        /// Creates a client for the live API or the sandbox.
        /// </summary>
        /// <param name="apiKey">Your API key (a token created in My WDSF, My API Tokens).</param>
        /// <param name="endPoint">The live API or the sandbox.</param>
        /// <param name="onBehalfOf">The MIN of a scrutineer to act for (sent as X-OnBehalfOf). Needs a token that may impersonate; the person must hold an active scrutiny licence.</param>
        public Client(SecureString apiKey, WdsfEndpoint endPoint, string onBehalfOf = null) :
            this(ExtractSecureString(apiKey), new Uri(GetBaseUrl(endPoint)), onBehalfOf)
        {
        }

        /// <summary>
        /// Creates a client for the live API or the sandbox.
        /// </summary>
        /// <param name="apiKey">Your API key (a token created in My WDSF, My API Tokens).</param>
        /// <param name="endPoint">The live API or the sandbox.</param>
        /// <param name="onBehalfOf">The MIN of a scrutineer to act for (sent as X-OnBehalfOf). Needs a token that may impersonate; the person must hold an active scrutiny licence.</param>
        public Client(string apiKey, WdsfEndpoint endPoint, string onBehalfOf = null) :
            this(apiKey, new Uri(GetBaseUrl(endPoint)), onBehalfOf)
        {
        }

        private static string GetBaseUrl(WdsfEndpoint endPoint)
        {
            return endPoint == WdsfEndpoint.Services ? ServicesBaseUrl : SandboxBaseUrl;
        }

        /// <summary>
        /// Creates a client for an API at another address (e.g. a local test server).
        /// </summary>
        /// <param name="apiKey">Your API key (a token created in My WDSF, My API Tokens).</param>
        /// <param name="baseUrl">The API's base URL including the version, e.g. https://api.wdsf.org/api/2/.</param>
        /// <param name="onBehalfOf">The MIN of a scrutineer to act for (sent as X-OnBehalfOf). Needs a token that may impersonate; the person must hold an active scrutiny licence.</param>
        public Client(string apiKey, Uri baseUrl, string onBehalfOf = null)
        {
            if (baseUrl == null)
                throw new ArgumentNullException(nameof(baseUrl));

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("The API key must be set.", nameof(apiKey));

            var url = baseUrl.ToString();
            apiUriBase = url.EndsWith("/") ? url : url + "/";
            this.apiKey = apiKey.Trim();
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

            return new NetworkCredential(string.Empty, val).Password;
        }

        private static string BuildQuery(IDictionary<string, string> filter)
        {
            return string.Join("&", filter.Select(e => Uri.EscapeDataString(e.Key) + "=" + Uri.EscapeDataString(e.Value ?? string.Empty)).ToArray());
        }


        ///<inheritdoc/>
        public IList<Person> GetPersons(IDictionary<string, string> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return GetResourceList<ListOfPerson, Person>("person?" + BuildQuery(filter));
        }
        ///<inheritdoc/>
        public IList<PersonExport> GetPersonExport(IDictionary<string, string> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return GetResourceList<ListOfPersonExport, PersonExport>("person/export?" + BuildQuery(filter));
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

            return GetResourceList<ListOfCompetition, Competition>("competition?" + BuildQuery(filter));
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

            return GetResourceList<ListOfCouple, Couple>("couple?" + BuildQuery(filter));
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

            return GetResourceList<ListOfTeam, Team>("team?" + BuildQuery(filter));
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
            return GetResourceList<ListOfRanking, Ranking>(BuildRankingUri(discipline, ageGroup, division, date, null, null));
        }
        ///<inheritdoc/>
        public IList<RankingSingle> GetSingleWorldRanking(string discipline, string ageGroup, string division, string gender = null, DateTime? date = null)
        {
            return GetResourceList<ListOfRankingSingle, RankingSingle>(BuildRankingUri(discipline, ageGroup, division, date ?? DateTime.MinValue, gender, null));
        }
        ///<inheritdoc/>
        public IList<RankingTeam> GetTeamWorldRanking(string discipline, string ageGroup, string division, DateTime? date = null)
        {
            return GetResourceList<ListOfRankingTeam, RankingTeam>(BuildRankingUri(discipline, ageGroup, division, date ?? DateTime.MinValue, null, null));
        }

        private static string BuildRankingUri(string discipline, string ageGroup, string division, DateTime date, string gender, string form)
        {
            if (string.IsNullOrEmpty(discipline))
                throw new ArgumentException("Discipline must be set.", nameof(discipline));

            if (string.IsNullOrEmpty(ageGroup))
                throw new ArgumentException("Age group must be set.", nameof(ageGroup));

            if (string.IsNullOrEmpty(division))
                throw new ArgumentException("Division must be set.", nameof(division));

            var filter = new Dictionary<string, string>
            {
                { FilterNames.Ranking.AgeGroup, ageGroup },
                { FilterNames.Ranking.Discipline, discipline },
                { FilterNames.Ranking.Division, division },
            };

            if (date != DateTime.MinValue)
                filter.Add(FilterNames.Ranking.Date, date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(gender))
                filter.Add(FilterNames.Ranking.Gender, gender);

            if (!string.IsNullOrEmpty(form))
                filter.Add(FilterNames.Ranking.Form, form);

            return "ranking?" + BuildQuery(filter);
        }

        ///<inheritdoc/>
        public IList<AgeVerification> CheckAgeForCompetition(IEnumerable<int> mins, IEnumerable<int> competitionIds)
        {
            if (mins == null)
                throw new ArgumentNullException(nameof(mins));

            if (competitionIds == null)
                throw new ArgumentNullException(nameof(competitionIds));

            var minList = mins.ToList();
            if (minList.Count < 1 || minList.Count > 2)
                throw new ArgumentException("One MIN (solo) or two MIN (couple) must be given.", nameof(mins));

            var idList = competitionIds.ToList();
            if (idList.Count == 0)
                throw new ArgumentException("At least one competition id must be given.", nameof(competitionIds));

            return GetResourceList<ListOfAgeVerification, AgeVerification>(
                "age/CheckForCompetition/" + string.Join(",", minList) + "/" + string.Join(",", idList));
        }

        ///<inheritdoc/>
        public IList<EventGroup> GetEventGroups(IDictionary<string, string> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return GetResourceList<ListOfEventGroup, EventGroup>("group?" + BuildQuery(filter));
        }
        ///<inheritdoc/>
        public EventGroupDetail GetEventGroup(int id)
        {
            return GetResource<EventGroupDetail>("group/" + id);
        }

        ///<inheritdoc/>
        public IList<ScheduleEntry> GetSchedule(int groupId)
        {
            return GetResourceList<ListOfScheduleEntry, ScheduleEntry>("schedule?groupId=" + groupId);
        }
        ///<inheritdoc/>
        public ScheduleEntry GetScheduleEntry(Guid id)
        {
            return GetResource<ScheduleEntry>("schedule/" + id);
        }
        ///<inheritdoc/>
        public Uri SaveScheduleEntry(ScheduleEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            return SaveResource(entry, "schedule");
        }
        ///<inheritdoc/>
        public bool UpdateScheduleEntry(ScheduleEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            if (entry.Id == Guid.Empty)
                throw new ArgumentException("The schedule entry's Id must be set.", nameof(entry));

            return UpdateResource(entry, "schedule/" + entry.Id);
        }
        ///<inheritdoc/>
        public bool DeleteScheduleEntry(Guid id)
        {
            return DeleteResource("schedule/" + id);
        }

        ///<inheritdoc/>
        public ApiFile GetParticipantMusic(int participantId, DateTimeOffset? ifModifiedSince = null)
        {
            return GetFile("participant/" + participantId + "/music", false, ifModifiedSince);
        }
        ///<inheritdoc/>
        public ApiFile GetParticipantMusicInfo(int participantId)
        {
            return GetFile("participant/" + participantId + "/music", true, null);
        }

        private ApiFile GetFile(string resourceUri, bool headersOnly, DateTimeOffset? ifModifiedSince)
        {
            var adapter = GetAdapter();
            try
            {
                return adapter.GetFile(new Uri(this.apiUriBase + resourceUri), headersOnly, ifModifiedSince);
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
                    adapter = new RestAdapter(this.apiKey, this.onBehalfOf)
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
