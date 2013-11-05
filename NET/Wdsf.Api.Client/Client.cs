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
            if (username == null)
                throw new ArgumentNullException("username");
            if (password == null)
                throw new ArgumentNullException("password");
            if (baseUrl == null)
                throw new ArgumentNullException("baseUrl");

            apiUriBase = baseUrl;

            this.username = username;
            this.password = password;

            this.adapters = new List<RestAdapter>() { new RestAdapter(username, password) };

#if DEBUG
            System.Net.ServicePointManager.ServerCertificateValidationCallback = CertificatePolicy.ValidateSSLCertificate;
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
                throw new ArgumentNullException("filter");

            string query = string.Join("&", filter.Select(e => string.Format("{0}={1}", e.Key, e.Value)).ToArray());

            return GetResourceList<ListOfPerson, Person>(string.Format("person?{0}", query));
        }
        public PersonDetail GetPerson(int min)
        {
            return GetResource<PersonDetail>(string.Format("person/{0}", min));
        }
        public bool UpdatePerson(PersonDetail person)
        {
            if (person == null)
                throw new ArgumentNullException("person");

            return UpdateResource<PersonDetail>(person, string.Format("person/{0}", person.Min));
        }


        public IList<Competition> GetCompetitions(IDictionary<string, string> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            string query = string.Join("&", filter.Select(e => string.Format("{0}={1}", e.Key, e.Value)).ToArray());

            return GetResourceList<ListOfCompetition, Competition>(string.Format("competition?{0}", query));
        }
        public CompetitionDetail GetCompetition(int id)
        {
            return GetResource<CompetitionDetail>(string.Format("competition/{0}", id));
        }
        public bool UpdateCompetition(CompetitionDetail competition)
        {
            if (competition == null)
                throw new ArgumentNullException("competition");

            // not used for updating
            competition.Links = null;

            return UpdateResource<CompetitionDetail>(competition, string.Format("competition/{0}", competition.Id));
        }

        public ParticipantCoupleDetail GetCoupleParticipant(int id)
        {
            return GetResource<ParticipantCoupleDetail>(string.Format("participant/{0}", id));
        }
        public IList<ParticipantCouple> GetCoupleParticipants(int competitionId)
        {
            return GetResourceList<ListOfCoupleParticipant, ParticipantCouple>(
                string.Format("participant?competitionId={0}", competitionId)
            );
        }
        public bool UpdateCoupleParticipant(ParticipantCoupleDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException("participant");

            ClearLinks(participant);

            return UpdateResource<ParticipantCoupleDetail>(participant, string.Format("participant/{0}", participant.Id));
        }
        public Uri SaveCoupleParticipant(ParticipantCoupleDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException("participant");

            ClearLinks(participant);

            return SaveResource<ParticipantCoupleDetail>(participant, "participant");
        }
        public bool DeleteCoupleParticipant(int id)
        {
            return DeleteResource(string.Format("participant/{0}", id));
        }


        public ParticipantTeamDetail GetTeamParticipant(int id)
        {
            return GetResource<ParticipantTeamDetail>(string.Format("participant/{0}", id));
        }
        public IList<ParticipantTeam> GetTeamParticipants(int competitionId)
        {
            return GetResourceList<ListOfTeamParticipant, ParticipantTeam>(
                string.Format("participant?competitionId={0}", competitionId)
            );
        }
        public bool UpdateTeamParticipant(ParticipantTeamDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException("participant");

            ClearLinks(participant);

            return UpdateResource<ParticipantTeamDetail>(participant, string.Format("participant/{0}", participant.Id));
        }
        public Uri SaveTeamParticipant(ParticipantTeamDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException("participant");

            ClearLinks(participant);

            return SaveResource<ParticipantTeamDetail>(participant, "participant");
        }
        public bool DeleteTeamParticipant(int id)
        {
            return DeleteResource(string.Format("participant/{0}", id));
        }


        public ParticipantSingleDetail GetSingleParticipant(int id)
        {
            return GetResource<ParticipantSingleDetail>(string.Format("participant/{0}", id));
        }
        public IList<ParticipantSingle> GetSingleParticipants(int competitionId)
        {
            return GetResourceList<ListOfSingleParticipant, ParticipantSingle>(
                string.Format("participant?competitionId={0}", competitionId)
            );
        }
        public bool UpdateSingleParticipant(ParticipantSingleDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException("participant");

            ClearLinks(participant);

            return UpdateResource<ParticipantSingleDetail>(participant, string.Format("participant/{0}", participant.Id));
        }
        public Uri SaveSingleParticipant(ParticipantSingleDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException("participant");

            ClearLinks(participant);

            return SaveResource<ParticipantSingleDetail>(participant, "participant");
        }
        public bool DeleteSingleParticipant(int id)
        {
            return DeleteResource(string.Format("participant/{0}", id));
        }


        public OfficialDetail GetOfficial(int id)
        {
            return GetResource<OfficialDetail>(string.Format("official/{0}", id));
        }
        public IList<Official> GetOfficials(int competitionId)
        {
            return GetResourceList<ListOfOfficial, Official>(
                string.Format("official?competitionId={0}", competitionId)
            );
        }
        public bool UpdateOfficial(OfficialDetail official)
        {
            if (official == null)
                throw new ArgumentNullException("official");

            return UpdateResource<OfficialDetail>(official, string.Format("official/{0}", official.Id));
        }
        public Uri SaveOfficial(OfficialDetail official)
        {
            if (official == null)
                throw new ArgumentNullException("official");

            return SaveResource<OfficialDetail>(official, "official");
        }
        public bool DeleteOfficial(int id)
        {
            return DeleteResource(string.Format("official/{0}", id));
        }


        public IList<CoupleExport> GetCouples()
        {
            return GetResourceList<ListOfCoupleExport, CoupleExport>("couple/export");
        }
        public IList<Couple> GetCouples(IDictionary<string, string> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            string query = string.Join("&", filter.Select(e => string.Format("{0}={1}", e.Key, e.Value)).ToArray());

            return GetResourceList<ListOfCouple, Couple>(string.Format("couple?{0}", query));
        }
        public CoupleDetail GetCouple(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Couple ID must be specified.", "id");

            return GetResource<CoupleDetail>(string.Format("couple/{0}", id));
        }
        public bool UpdateCouple(CoupleDetail couple)
        {
            if (couple == null)
                throw new ArgumentNullException("couple");

            return UpdateResource<CoupleDetail>(couple, string.Format("couple/{0}", couple.Id));
        }
        public Uri SaveCouple(CoupleDetail couple)
        {
            if (couple == null)
                throw new ArgumentNullException("couple");

            return SaveResource<CoupleDetail>(couple, "couple");
        }


        public IList<Ranking> GetWorldRanking(string discipline, string ageGroup, string division)
        {
            return GetWorldRanking(discipline, ageGroup, division, DateTime.MinValue);
        }
		public IList<Ranking> GetWorldRanking(string discipline, string ageGroup, string division, DateTime date)
        {
            if (string.IsNullOrEmpty(discipline))
            {
                throw new ArgumentException("Discipline must be set.", "discipline");
            }

            if (string.IsNullOrEmpty(ageGroup))
            {
                throw new ArgumentException("Age group must be set.", "ageGroup");
            }

            if (string.IsNullOrEmpty(division))
            {
                throw new ArgumentException("Division must be set.", "division");
            }

            string resourceUri = 
                date != DateTime.MinValue ?
                string.Format("ranking?agegroup={0}&discipline={1}&division={2}&date={3:yyyy/MM/dd}", ageGroup, discipline, division, date) :
                string.Format("ranking?agegroup={0}&discipline={1}&division={2}", ageGroup, discipline, division);

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
        public T Get<T>(Uri resourceUri) where T:class
        {
            if (null == resourceUri)
                throw new ArgumentNullException("resourceUri");

            RestAdapter adapter = GetAdapter();
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
                throw new ArgumentNullException("resourceUri");

            RestAdapter adapter = GetAdapter();
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
                throw new ArgumentNullException("resourceUri");

            return GetResource<T>(resourceUri);
        }

        private object Get(string resourceUri)
        {
            if (null == resourceUri)
                throw new ArgumentNullException("resourceUri");

            RestAdapter adapter = GetAdapter();
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
            RestAdapter adapter = GetAdapter();
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

            RestAdapter adapter = GetAdapter();
            try
            {
                TContainer items = adapter.Get<TContainer>(new Uri(this.apiUriBase + resourceUri));
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
            StatusMessage message;
            RestAdapter adapter = GetAdapter();
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
            StatusMessage message;
            RestAdapter adapter = GetAdapter();
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
            StatusMessage message;
            RestAdapter adapter = GetAdapter();
            try
            {
                message = adapter.Delete(new Uri(this.apiUriBase + resourceUri));
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

        /// <summary>
        /// Removes the Link property of all Score types to reduce the model's overall size.
        /// </summary>
        /// <param name="participant"></param>
        private void ClearLinks(ParticipantBaseDetail participant)
        {
            participant.Link = null;

            if (participant.Rounds == null)
            {
                return;
            }

            foreach (Round round in participant.Rounds)
            {
                foreach (Dance dance in round.Dances)
                {
                    foreach (Score score in dance.Scores)
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

                RestAdapter adapter = this.adapters.FirstOrDefault(a => a.IsBusy == false && a.IsAssigned == false);
                if (adapter == null)
                {
                    adapter = new RestAdapter(this.username, this.password);
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
                        foreach (RestAdapter adapter in this.adapters)
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
