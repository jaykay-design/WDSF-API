/*  Copyright (C) 2011-2020 JayKay-Design S.C.
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
        private readonly SecureString password;
        private readonly string apiUriBase;

        private List<RestAdapter> adapters;
        private readonly object adapterLock = new object();

        public ContentTypes ContentType { get; set; }

        /// <summary>
        /// Contains the last message returned from the API
        /// </summary>
        public string LastApiMessage { get; private set; }

        public Client(string username, string password, WdsfEndpoint endPoint) :
            this(username, MakeSecureString(password), endPoint)
        {
        }

        public Client(string username, SecureString password, WdsfEndpoint endPoint) :
            this(username, password, endPoint == WdsfEndpoint.Services ? "https://services.worlddancesport.org/API/1/" : "https://sandbox.worlddancesport.org/API/1/")
        {
        }

        public Client(string username, string password, string baseUrl) :
            this(username, MakeSecureString(password), baseUrl)
        {
        }

        public Client(string username, SecureString password, string baseUrl)
        {
            apiUriBase = baseUrl ?? throw new ArgumentNullException(nameof(baseUrl));
            this.username = username ?? throw new ArgumentNullException(nameof(username));
            this.password = password ?? throw new ArgumentNullException(nameof(password));

            this.adapters = new List<RestAdapter>() { };

#if DEBUG
            // Ignore TLS errors when in debug mode (self signed certs)
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
#endif
        }

        private static SecureString MakeSecureString(string val)
        {
            if (val == null)
                return null;

            SecureString retVal = new SecureString();

            if (val.Length == 0)
                return retVal;

            for (int i = 0; i < val.Length; i++)
                retVal.AppendChar(val[i]);

            retVal.MakeReadOnly();
            return retVal;
        }


        public IList<Person> GetPersons(IDictionary<string, string> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            string query = string.Join("&", filter.Select(e => e.Key + "=" + e.Value).ToArray());

            return GetResourceList<ListOfPerson, Person>("person?" + query);
        }
        public PersonDetail GetPerson(int min)
        {
            return GetResource<PersonDetail>("person/" + min);
        }
        public bool UpdatePerson(PersonDetail person)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            return UpdateResource<PersonDetail>(person, "person/" + person.Min);
        }


        public IList<Competition> GetCompetitions(IDictionary<string, string> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            string query = string.Join("&", filter.Select(e => e.Key + "=" + e.Value).ToArray());

            return GetResourceList<ListOfCompetition, Competition>("competition?" + query);
        }
        public CompetitionDetail GetCompetition(int id)
        {
            return GetResource<CompetitionDetail>("competition/" + id);
        }
        public bool UpdateCompetition(CompetitionDetail competition)
        {
            if (competition == null)
                throw new ArgumentNullException(nameof(competition));

            // not used for updating
            competition.Links = null;

            return UpdateResource(competition, "competition/" + competition.Id);
        }

        public ParticipantCoupleDetail GetCoupleParticipant(int id)
        {
            return GetResource<ParticipantCoupleDetail>("participant/" + id);
        }
        public IList<ParticipantCouple> GetCoupleParticipants(int competitionId)
        {
            return GetResourceList<ListOfCoupleParticipant, ParticipantCouple>(
                "participant?competitionId=" + competitionId
            );
        }
        public bool UpdateCoupleParticipant(ParticipantCoupleDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException(nameof(participant));

            ClearLinks(participant);

            return UpdateResource(participant, "participant/" + participant.Id);
        }
        public Uri SaveCoupleParticipant(ParticipantCoupleDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException(nameof(participant));

            ClearLinks(participant);

            return SaveResource(participant, "participant");
        }
        public bool DeleteCoupleParticipant(int id)
        {
            return DeleteResource("participant/" + id);
        }


        public ParticipantTeamDetail GetTeamParticipant(int id)
        {
            return GetResource<ParticipantTeamDetail>("participant/" + id);
        }
        public IList<ParticipantTeam> GetTeamParticipants(int competitionId)
        {
            return GetResourceList<ListOfTeamParticipant, ParticipantTeam>(
                "participant?competitionId=" + competitionId
            );
        }
        public bool UpdateTeamParticipant(ParticipantTeamDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException(nameof(participant));

            ClearLinks(participant);

            return UpdateResource(participant, "participant/" + participant.Id);
        }
        public Uri SaveTeamParticipant(ParticipantTeamDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException(nameof(participant));

            ClearLinks(participant);

            return SaveResource(participant, "participant");
        }
        public bool DeleteTeamParticipant(int id)
        {
            return DeleteResource("participant/" + id);
        }


        public ParticipantSingleDetail GetSingleParticipant(int id)
        {
            return GetResource<ParticipantSingleDetail>("participant/" + id);
        }
        public IList<ParticipantSingle> GetSingleParticipants(int competitionId)
        {
            return GetResourceList<ListOfSingleParticipant, ParticipantSingle>(
                "participant?competitionId=" + competitionId
            );
        }
        public bool UpdateSingleParticipant(ParticipantSingleDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException(nameof(participant));

            ClearLinks(participant);

            return UpdateResource(participant, "participant/" + participant.Id);
        }
        public Uri SaveSingleParticipant(ParticipantSingleDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException(nameof(participant));

            ClearLinks(participant);

            return SaveResource(participant, "participant");
        }
        public bool DeleteSingleParticipant(int id)
        {
            return DeleteResource("participant/" + id);
        }


        public OfficialDetail GetOfficial(int id)
        {
            return GetResource<OfficialDetail>("official/" + id);
        }
        public IList<Official> GetOfficials(int competitionId)
        {
            return GetResourceList<ListOfOfficial, Official>(
                "official?competitionId=" + competitionId
            );
        }
        public bool UpdateOfficial(OfficialDetail official)
        {
            if (official == null)
                throw new ArgumentNullException(nameof(official));

            return UpdateResource(official, "official/" + official.Id);
        }
        public Uri SaveOfficial(OfficialDetail official)
        {
            if (official == null)
                throw new ArgumentNullException(nameof(official));

            return SaveResource(official, "official");
        }
        public bool DeleteOfficial(int id)
        {
            return DeleteResource("official/" +  id);
        }


        public IList<CoupleExport> GetCouples()
        {
            return GetResourceList<ListOfCoupleExport, CoupleExport>("couple/export");
        }
        public IList<Couple> GetCouples(IDictionary<string, string> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            string query = string.Join("&", filter.Select(e => e.Key + "=" + e.Value).ToArray());

            return GetResourceList<ListOfCouple, Couple>("couple?" + query);
        }
        public CoupleDetail GetCouple(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Couple ID must be specified.", nameof(id));

            return GetResource<CoupleDetail>("couple/" + id);
        }
        public bool UpdateCouple(CoupleDetail couple)
        {
            if (couple == null)
                throw new ArgumentNullException(nameof(couple));

            return UpdateResource(couple, "couple/" + couple.Id);
        }
        public Uri SaveCouple(CoupleDetail couple)
        {
            if (couple == null)
                throw new ArgumentNullException(nameof(couple));

            return SaveResource(couple, "couple");
        }


        public IList<Ranking> GetWorldRanking(string discipline, string ageGroup, string division)
        {
            return GetWorldRanking(discipline, ageGroup, division, DateTime.MinValue);
        }
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

        public IList<AgeClass> GetAges()
        {
            return GetResourceList<ListOfAgeClass, AgeClass>("age");
        }

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

        private T Get<T>(string resourceUri) where T : class
        {
            if (null == resourceUri)
                throw new ArgumentNullException(nameof(resourceUri));

            return GetResource<T>(resourceUri);
        }

        private object Get(string resourceUri)
        {
            if (null == resourceUri)
                throw new ArgumentNullException(nameof(resourceUri));

            var adapter = GetAdapter();
            try
            {
                return adapter.Get(new Uri(this.apiUriBase + resourceUri));
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
        private bool UpdateResource<T>(T competition, string resourceUri) where T : class
        {
            this.LastApiMessage = string.Empty;

            StatusMessage message;
            var adapter = GetAdapter();
            try
            {
                message = adapter.Put<T>(new Uri(this.apiUriBase + resourceUri), competition);
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
                    adapter = new RestAdapter(this.username, this.password)
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
