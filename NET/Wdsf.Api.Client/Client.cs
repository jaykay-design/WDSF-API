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


        /// <summary>
        /// Gets a list of persons.
        /// </summary>
        /// <param name="filter">A dictionaly containing filter parameters. See API documentation for details.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>A list of persons</returns>
        public IList<Person> GetPersons(IDictionary<string, string> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            string query = string.Join("&", filter.Select(e => string.Format("{0}={1}", e.Key, e.Value)).ToArray());

            return GetResourceList<ListOfPerson, Person>(string.Format("person?{0}", query));
        }

        /// <summary>
        /// Gets a person
        /// </summary>
        /// <param name="min">The member ID number</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The person model.</returns>
        public PersonDetail GetPerson(int min)
        {
            return GetResource<PersonDetail>(string.Format("person/{0}", min));
        }


        /// <summary>
        /// Gets a list of competitions.
        /// </summary>
        /// <param name="filter">A dictionaly containing filter parameters. See API documentation for details.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>A list of competitions.</returns>
        public IList<Competition> GetCompetitions(IDictionary<string, string> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            string query = string.Join("&", filter.Select(e => string.Format("{0}={1}", e.Key, e.Value)).ToArray());

            return GetResourceList<ListOfCompetition, Competition>(string.Format("competition?{0}", query));
        }

        /// <summary>
        /// Gets a competition
        /// </summary>
        /// <param name="id">The competition ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The competition</returns>
        public CompetitionDetail GetCompetition(int id)
        {
            return GetResource<CompetitionDetail>(string.Format("competition/{0}", id));
        }

        /// <summary>
        /// Updates a competition
        /// </summary>
        /// <param name="competition">The competition model</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        public bool UpdateCompetition(CompetitionDetail competition)
        {
            if (competition == null)
                throw new ArgumentNullException("competition");

            // not used for updating
            competition.Links = null;

            return UpdateResource<CompetitionDetail>(competition, string.Format("competition/{0}", competition.Id));
        }


        /// <summary>
        /// Gets a participant (couple)
        /// </summary>
        /// <param name="id">The participant ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The participant (couple)</returns>
        public ParticipantCoupleDetail GetCoupleParticipant(int id)
        {
            return GetResource<ParticipantCoupleDetail>(string.Format("participant/{0}", id));
        }

        /// <summary>
        /// Get all participiants (couples) for specified competition
        /// </summary>
        /// <param name="competitionId">The competition ID</param>
        /// <returns>The list of participants (couples)</returns>
        public IList<ParticipantCouple> GetCoupleParticipants(int competitionId)
        {
            return GetResourceList<ListOfCoupleParticpant, ParticipantCouple>(
                string.Format("participant?competitionId={0}", competitionId)
            );
        }

        /// <summary>
        /// Updates a participant (couple).
        /// </summary>
        /// <param name="participant">The participant model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        public bool UpdateCoupleParticipant(ParticipantCoupleDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException("participant");

            ClearLinks(participant);

            return UpdateResource<ParticipantCoupleDetail>(participant, string.Format("participant/{0}", participant.Id));
        }

        /// <summary>
        /// Saves a new participant (couple).
        /// </summary>
        /// <param name="participant">The participant model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The Uri to the newly created resource</returns>
        public Uri SaveCoupleParticipant(ParticipantCoupleDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException("participant");

            ClearLinks(participant);

            return SaveResource<ParticipantCoupleDetail>(participant, "participant");
        }

        /// <summary>
        /// Delete a participant (couple)
        /// </summary>
        /// <param name="id">The participant ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>true on success.</returns>
        public bool DeleteCoupleParticipant(int id)
        {
            return DeleteResource(string.Format("participant/{0}", id));
        }


        /// <summary>
        /// Gets a participant (team)
        /// </summary>
        /// <param name="id">The participant ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The participant (team)</returns>
        public ParticipantTeamDetail GetTeamParticipant(int id)
        {
            return GetResource<ParticipantTeamDetail>(string.Format("participant/{0}", id));
        }

        /// <summary>
        /// Updates a participant (team).
        /// </summary>
        /// <param name="participant">The participant model.</param>
        /// <exception cref="UnknownMediaTypeException">The received resource type was not recognized.</exception>
        /// <exception cref="UnexpectedMediaTypeException">The received resource type was not expected.</exception>
        public bool UpdateTeamParticipant(ParticipantTeamDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException("participant");

            ClearLinks(participant);

            return UpdateResource<ParticipantTeamDetail>(participant, string.Format("participant/{0}", participant.Id));
        }

        /// <summary>
        /// Saves a new participant (team).
        /// </summary>
        /// <param name="participant">The participant model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The Uri to the newly created resource</returns>
        public Uri SaveTeamParticipant(ParticipantTeamDetail participant)
        {
            if (participant == null)
                throw new ArgumentNullException("participant");

            ClearLinks(participant);

            return SaveResource<ParticipantTeamDetail>(participant, "participant");
        }

        /// <summary>
        /// Delete a participant (team)
        /// </summary>
        /// <param name="id">The participant ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>true on success.</returns>
        public bool DeleteTeamParticipant(int id)
        {
            return DeleteResource(string.Format("participant/{0}", id));
        }


        /// <summary>
        /// Gets an official
        /// </summary>
        /// <param name="id">The official ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The official</returns>
        public OfficialDetail GetOfficial(int id)
        {
            return GetResource<OfficialDetail>(string.Format("official/{0}", id));
        }

        /// <summary>
        /// Gets officials of specified competition
        /// </summary>
        /// <param name="competitionId">The competition ID</param>
        /// <returns>The list of officials</returns>
        public IList<Official> GetOfficials(int competitionId)
        {
            return GetResourceList<ListOfOfficial, Official>(
                string.Format("official?competitionId={0}", competitionId)
            );
        }

        /// <summary>
        /// Updates an official
        /// </summary>
        /// <param name="official">The official model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        public bool UpdateOfficial(OfficialDetail official)
        {
            if (official == null)
                throw new ArgumentNullException("official");

            return UpdateResource<OfficialDetail>(official, string.Format("official/{0}", official.Id));
        }

        /// <summary>
        /// Saves a new official.
        /// </summary>
        /// <param name="official">The official model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The Uri to the newly created official</returns>
        public Uri SaveOfficial(OfficialDetail official)
        {
            if (official == null)
                throw new ArgumentNullException("official");

            return SaveResource<OfficialDetail>(official, "official");
        }

        /// <summary>
        /// Delete am official
        /// </summary>
        /// <param name="id">The official ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>true on success.</returns>
        public bool DeleteOfficial(int id)
        {
            return DeleteResource(string.Format("offcial/{0}", id));
        }


        /// <summary>
        /// Gets a list of all active couples. This list contains all neccesary information for off-line usage.
        /// </summary>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>A list of couples</returns>
        public IList<CoupleExport> GetCouples()
        {
            return GetResourceList<ListOfCoupleExport, CoupleExport>("couple/export");
        }

        /// <summary>
        /// Gets a list of couples.
        /// </summary>
        /// <param name="filter">A dictionaly containing filter parameters. See API documentation for details.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>A list of couples</returns>
        public IList<Couple> GetCouples(IDictionary<string, string> filter)
        {
            if (filter == null)
                throw new ArgumentNullException("filter");

            string query = string.Join("&", filter.Select(e => string.Format("{0}={1}", e.Key, e.Value)).ToArray());

            return GetResourceList<ListOfCouple, Couple>(string.Format("couple?{0}", query));
        }

        /// <summary>
        /// Gets a couple
        /// </summary>
        /// <param name="id">The couple ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The couple</returns>
        public CoupleDetail GetCouple(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Couple ID must be specified.", "id");

            return GetResource<CoupleDetail>(string.Format("couple/{0}", id));
        }

        /// <summary>
        /// Updates a couple
        /// </summary>
        /// <param name="couple">The couple model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        public bool UpdateCouple(CoupleDetail couple)
        {
            if (couple == null)
                throw new ArgumentNullException("couple");

            return UpdateResource<CoupleDetail>(couple, string.Format("couple/{0}", couple.Id));
        }

        /// <summary>
        /// Saves a new couple.
        /// </summary>
        /// <param name="couple">The official model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The Uri to the newly created couple</returns>
        public Uri SaveCouple(CoupleDetail couple)
        {
            if (couple == null)
                throw new ArgumentNullException("couple");

            return SaveResource<CoupleDetail>(couple, "couple");
        }


        /// <summary>
        /// Gets a world ranking list.
        /// </summary>
        /// <param name="discipline">The discipline.</param>
        /// <param name="ageGroup">The age group.</param>
        /// <param name="division">The division.</param>
		/// <param name="date">Date to get ranking for. Can be absent.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <exception cref="ArgumentException">Any of the required arguments is missing.</exception>
        /// <returns>The world ranking list</returns>
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

            return GetResourceList<ListOfRanking, Ranking>(
				string.Format("ranking?agegroup={0}&discipline={1}&division={2}&date={3:yyyy\\/MM\\/dd}", ageGroup, discipline, division)
            );
        }

        /// <summary>
        /// Gets all allowed age groups.
        /// </summary>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The age groups</returns>
        public IList<AgeClass> GetAges()
        {
            return GetResourceList<ListOfAgeClass, AgeClass>("age");
        }

        /// <summary>
        /// Gets all allowed countries.
        /// </summary>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The countries</returns>
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

            try
            {
                return GetAdapter().Get<T>(resourceUri);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
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

            try
            {
                return GetAdapter().Get(resourceUri);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
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

            try
            {
                return GetAdapter().Get(new Uri(this.apiUriBase + resourceUri));
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
        }

        private T GetResource<T>(string resourceUri) where T : class
        {
            try
            {
                return GetAdapter().Get<T>(new Uri(this.apiUriBase + resourceUri));
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
        }
        private IList<TItem> GetResourceList<TContainer, TItem>(string resourceUri)
            where TContainer : class, IEnumerable<TItem>
            where TItem : class
        {

            try
            {
                TContainer items = GetAdapter().Get<TContainer>(new Uri(this.apiUriBase + resourceUri));
                return new List<TItem>(items);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
        }
        private bool UpdateResource<T>(T competition, string resourceUri) where T : class
        {
            StatusMessage message;
            try
            {
                message = GetAdapter().Put<T>(new Uri(this.apiUriBase + resourceUri), competition);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }

            this.LastApiMessage = message.Message;
            return message.Code == (int)HttpStatusCode.OK;
        }
        private Uri SaveResource<T>(T participant, string resourceUri) where T : class
        {
            StatusMessage message;
            try
            {
                message = GetAdapter().Post<T>(new Uri(this.apiUriBase + resourceUri), participant);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
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
            try
            {
                message = GetAdapter().Delete(new Uri(this.apiUriBase + resourceUri));
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
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

                RestAdapter adapter = this.adapters.FirstOrDefault(a => a.IsBusy == false);
                if (adapter == null)
                {
                    adapter = new RestAdapter(this.username, this.password);
                    this.adapters.Add(adapter);
                }

                return adapter;
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
