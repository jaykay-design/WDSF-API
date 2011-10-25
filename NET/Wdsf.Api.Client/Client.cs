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
    using Wdsf.Api.Client.Exceptions;
    using Wdsf.Api.Client.Interfaces;
    using Wdsf.Api.Client.Models;
    
    /// <summary>
    /// <para>Provides access to the WDSF API throught stongly typed models.</para>
    /// <para>This class is threadsafe and multiple request can be made to the same instance.</para>
    /// </summary>
    public class Client : IDisposable, IClient
    {
        private string username;
        private string password;
        private string apiUriBase;

        private List<RestAdapter> adapters;
        private object adapterLock = new object();

        public Client(string username, string password, WdsfEndpoint endPoint)
        {
            apiUriBase = endPoint == WdsfEndpoint.Services ? "https://services.worlddancesport.org/API/1/" : "https://sandbox.worlddancesport.org/API/1/";

            this.username = username;
            this.password = password;

            this.adapters = new List<RestAdapter>() { new RestAdapter(username, password) };
        }

        /// <summary>
        /// Gets a list of persons.
        /// </summary>
        /// <param name="filter">A dictionaly containing filter parameters. See API documentation for details.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>A list of persons</returns>
        public IList<Person> GetPersons(IDictionary<string,string> filter)
        {
            string query = string.Join("&", filter.Select(e => string.Format("{0}={1}", e.Key, e.Value)).ToArray());
            Uri resourceUri = new Uri(string.Format("{0}person?{1}", this.apiUriBase, query));

            return GetResourceList<ListOfPerson, Person>(resourceUri);
        }

        /// <summary>
        /// Gets a person
        /// </summary>
        /// <param name="min">The member ID number</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The person model.</returns>
        public PersonDetail GetPerson(int min)
        {
            Uri resourceUri = new Uri(string.Format("{0}person/{1}", this.apiUriBase, min));

            return GetResource<PersonDetail>(resourceUri);
        }


        /// <summary>
        /// Gets a list of competitions.
        /// </summary>
        /// <param name="filter">A dictionaly containing filter parameters. See API documentation for details.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>A list of competitions.</returns>
        public IList<Competition> GetCompetitions(IDictionary<string, string> filter)
        {
            string query = string.Join("&", filter.Select(e => string.Format("{0}={1}", e.Key, e.Value)).ToArray());
            Uri resourceUri = new Uri(string.Format("{0}competition?{1}", this.apiUriBase, query));

            return GetResourceList<ListOfCompetition, Competition>(resourceUri);
        }

        /// <summary>
        /// Gets a competition
        /// </summary>
        /// <param name="id">The competition ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The competition</returns>
        public CompetitionDetail GetCompetition(int id)
        {
            Uri resourceUri = new Uri(string.Format("{0}competition/{1}", this.apiUriBase, id));

            return GetResource<CompetitionDetail>(resourceUri);
        }

        /// <summary>
        /// Updates a competition
        /// </summary>
        /// <param name="competition">The competition model</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        public bool UpdateCompetition(CompetitionDetail competition)
        {
            Uri resourceUri = new Uri(string.Format("{0}competition/{1}", this.apiUriBase, competition.Id));

            // not used for updating
            competition.Links = null;

            return UpdateResource<CompetitionDetail>(competition, resourceUri);
        }


        /// <summary>
        /// Gets a participant (couple)
        /// </summary>
        /// <param name="id">The participant ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The participant (couple)</returns>
        public ParticipantCoupleDetail GetCoupleParticipant(int id)
        {
            Uri resourceUri = new Uri(string.Format("{0}participant/{1}", this.apiUriBase, id));

            return GetResource<ParticipantCoupleDetail>(resourceUri);
        }

        /// <summary>
        /// Updates a participant (couple).
        /// </summary>
        /// <param name="participant">The participant model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        public bool UpdateCoupleParticipant(ParticipantCoupleDetail participant)
        {
            Uri resourceUri = new Uri(string.Format("{0}participant/{1}", this.apiUriBase, participant.Id));

            ClearLinks(participant);

            return UpdateResource<ParticipantCoupleDetail>(participant, resourceUri);
        }

        /// <summary>
        /// Saves a new participant (couple).
        /// </summary>
        /// <param name="participant">The participant model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The Uri to the newly created resource</returns>
        public Uri SaveCoupleParticipant(ParticipantCoupleDetail participant)
        {
            Uri resourceUri = new Uri(string.Format("{0}participant", this.apiUriBase));

            ClearLinks(participant);

            return SaveResource<ParticipantCoupleDetail>(participant, resourceUri);
        }

        /// <summary>
        /// Delete a participant (couple)
        /// </summary>
        /// <param name="id">The participant ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>true on success.</returns>
        public bool DeleteCoupleParticipant(int id)
        {
            Uri resourceUri = new Uri(string.Format("{0}participant/{1}", this.apiUriBase, id));

            return DeleteResource(resourceUri);
        }


        /// <summary>
        /// Gets a participant (team)
        /// </summary>
        /// <param name="id">The participant ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The participant (team)</returns>
        public ParticipantTeamDetail GetTeamParticipant(int id)
        {
            Uri resourceUri = new Uri(string.Format("{0}participant/{1}", this.apiUriBase, id));

            return GetResource<ParticipantTeamDetail>(resourceUri);
        }

        /// <summary>
        /// Updates a participant (team).
        /// </summary>
        /// <param name="participant">The participant model.</param>
        /// <exception cref="UnknownMediaTypeException">The received resource type was not recognized.</exception>
        /// <exception cref="UnexpectedMediaTypeException">The received resource type was not expected.</exception>
        public bool UpdateTeamParticipant(ParticipantTeamDetail participant)
        {
            Uri resourceUri = new Uri(string.Format("{0}participant/{1}", this.apiUriBase, participant.Id));

            ClearLinks(participant);

            return UpdateResource<ParticipantTeamDetail>(participant, resourceUri);
        }

        /// <summary>
        /// Saves a new participant (team).
        /// </summary>
        /// <param name="participant">The participant model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The Uri to the newly created resource</returns>
        public Uri SaveTeamParticipant(ParticipantTeamDetail participant)
        {
            Uri resourceUri = new Uri(string.Format("{0}participant", this.apiUriBase));

            ClearLinks(participant);

            return SaveResource<ParticipantTeamDetail>(participant, resourceUri);
        }

        /// <summary>
        /// Delete a participant (team)
        /// </summary>
        /// <param name="id">The participant ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>true on success.</returns>
        public bool DeleteTeamParticipant(int id)
        {
            Uri resourceUri = new Uri(string.Format("{0}participant/{1}", this.apiUriBase, id));

            return DeleteResource(resourceUri);
        }


        /// <summary>
        /// Gets an official
        /// </summary>
        /// <param name="id">The official ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The official</returns>
        public OfficialDetail GetOfficial(int id)
        {
            Uri resourceUri = new Uri(string.Format("{0}official/{1}", this.apiUriBase, id));

            return GetResource<OfficialDetail>(resourceUri);
        }

        /// <summary>
        /// Updates an official
        /// </summary>
        /// <param name="official">The official model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        public bool UpdateOfficial(OfficialDetail official)
        {
            Uri resourceUri = new Uri(string.Format("{0}official/{1}", this.apiUriBase, official.Id));

            return UpdateResource<OfficialDetail>(official, resourceUri);
        }

        /// <summary>
        /// Saves a new official.
        /// </summary>
        /// <param name="official">The official model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The Uri to the newly created official</returns>
        public Uri SaveOfficial(OfficialDetail official)
        {
            Uri resourceUri = new Uri(string.Format("{0}official", this.apiUriBase));

            return SaveResource<OfficialDetail>(official, resourceUri);
        }

        /// <summary>
        /// Delete am official
        /// </summary>
        /// <param name="id">The official ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>true on success.</returns>
        public bool DeleteOfficial(int id)
        {
            Uri resourceUri = new Uri(string.Format("{0}offcial/{1}", this.apiUriBase, id));

            return DeleteResource(resourceUri);
        }


        /// <summary>
        /// Gets a list of couples.
        /// </summary>
        /// <param name="filter">A dictionaly containing filter parameters. See API documentation for details.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>A list of couples</returns>
        public IList<Couple> GetCouples(IDictionary<string, string> filter)
        {
            string query = string.Join("&", filter.Select(e => string.Format("{0}={1}", e.Key, e.Value)).ToArray());
            Uri resourceUri = new Uri(string.Format("{0}couple?{1}", this.apiUriBase, query));

            return GetResourceList<ListOfCouple, Couple>(resourceUri);
        }

        /// <summary>
        /// Gets a couple
        /// </summary>
        /// <param name="id">The couple ID</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The couple</returns>
        public CoupleDetail GetCouple(string id)
        {
            Uri resourceUri = new Uri(string.Format("{0}couple/{1}", this.apiUriBase, id));

            return GetResource<CoupleDetail>(resourceUri);
        }

        /// <summary>
        /// Updates a couple
        /// </summary>
        /// <param name="couple">The couple model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        public bool UpdateCouple(CoupleDetail couple)
        {
            Uri resourceUri = new Uri(string.Format("{0}couple/{1}", this.apiUriBase, couple.Id));

            return UpdateResource<CoupleDetail>(couple, resourceUri);
        }

        /// <summary>
        /// Saves a new couple.
        /// </summary>
        /// <param name="couple">The official model.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The Uri to the newly created couple</returns>
        public Uri SaveCouple(CoupleDetail couple)
        {
            Uri resourceUri = new Uri(string.Format("{0}couple", this.apiUriBase));

            return SaveResource<CoupleDetail>(couple, resourceUri);
        }


        /// <summary>
        /// Gets a world ranking list.
        /// </summary>
        /// <param name="discipline">The discipline.</param>
        /// <param name="ageGroup">The age group.</param>
        /// <param name="division">The division.</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <exception cref="ArgumentException">Any of the arguments is missing.</exception>
        /// <returns>The world ranking list</returns>
        public IList<Ranking> GetWorldRanking(string discipline, string ageGroup, string division)
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

            Uri resourceUri = new Uri(string.Format("{0}ranking?agegroup={1}&discipline={2}&division={3}", this.apiUriBase, ageGroup, discipline, division));

            return GetResourceList<ListOfRanking, Ranking>(resourceUri);
        }

        /// <summary>
        /// Gets all allowed age groups.
        /// </summary>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The age groups</returns>
        public IList<AgeClass> GetAges()
        {
            Uri resourceUri = new Uri(string.Format("{0}age", this.apiUriBase));

            return GetResourceList<ListOfAgeClass,AgeClass>(resourceUri);
        }

        /// <summary>
        /// Gets all allowed countries.
        /// </summary>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The countries</returns>
        public IList<Country> GetCountries()
        {
            Uri resourceUri = new Uri(string.Format("{0}country", this.apiUriBase));

            return GetResourceList<ListOfCountry, Country>(resourceUri);
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
            return GetResource<T>(resourceUri);
        }

        /// <summary>
        /// Gets a resource where the expected type is not know.
        /// </summary>
        /// <param name="resourceUri">The resourceUri</param>
        /// <exception cref="ApiException">The request failed. See inner exception for details.</exception>
        /// <returns>The resource</returns>
        public object Get(Uri resourceUri)
        {
            try
            {
                return GetAdapter().Get(resourceUri);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
        }

        private T GetResource<T>(Uri resourceUri) where T : class
        {
            try
            {
                return GetAdapter().Get<T>(resourceUri);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
        }
        private IList<TItem> GetResourceList<TContainer, TItem>(Uri resourceUri)
            where TContainer : class, IEnumerable<TItem>
            where TItem : class
        {

            try
            {
                TContainer items = GetAdapter().Get<TContainer>(resourceUri);
                return new List<TItem>(items);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }
        }
        private bool UpdateResource<T>(T competition, Uri resourceUri) where T : class
        {
            StatusMessage message;
            try
            {
                message = GetAdapter().Put<T>(resourceUri, competition);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }

            return message.Code == (int)HttpStatusCode.OK;
        }
        private Uri SaveResource<T>(T participant, Uri resourceUri) where T : class
        {
            StatusMessage message;
            try
            {
                message = GetAdapter().Post<T>(resourceUri, participant);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }

            if (message.Code != (int)HttpStatusCode.Created)
            {
                throw new ApiException(new RestException(message));
            }

            return new Uri(message.Link.HRef);
        }
        private bool DeleteResource(Uri resourceUri)
        {
            StatusMessage message;
            try
            {
                message = GetAdapter().Delete(resourceUri);
            }
            catch (Exception ex)
            {
                throw new ApiException(ex);
            }

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

        #endregion
    }
}
