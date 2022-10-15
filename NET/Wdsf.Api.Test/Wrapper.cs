using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wdsf.Api.Test
{

    internal class WrapperBase
    {
        public Exception Exception { get; protected set; }
        public string SourceMessage { get; protected set; }
    }

    internal class AgeWrapper
    {
        public Wdsf.Api.Client.Models.AgeClass age;
        public AgeWrapper(Wdsf.Api.Client.Models.AgeClass age)
        {
            this.age = age;
        }

        public override string ToString()
        {
            return this.age.Name;
        }
    }
    internal class PersonWrapper
    {
        public Wdsf.Api.Client.Models.Person person;
        public PersonWrapper(Wdsf.Api.Client.Models.Person person)
        {
            this.person = person;
        }

        public override string ToString()
        {
            return string.Format("{0} {1,-7} {2,-50} {3}", this.person.Min, this.person.Sex, person.Name, this.person.Country);
        }
    }
    internal class CountryWrapper
    {
        public Wdsf.Api.Client.Models.Country country;
        public CountryWrapper(Wdsf.Api.Client.Models.Country country)
        {
            this.country = country;
        }

        public override string ToString()
        {
            return this.country.Name;
        }
    }
    internal class ParticipantWrapper:WrapperBase
    {
        public Wdsf.Api.Client.Models.ParticipantCoupleDetail coupleParticipant;
        public Wdsf.Api.Client.Models.ParticipantTeamDetail teamParticipant;

        public ParticipantWrapper(Wdsf.Api.Client.Models.ParticipantTeamDetail teamParticipant)
        {
            this.teamParticipant = teamParticipant;

        }
        public ParticipantWrapper(Wdsf.Api.Client.Models.ParticipantCoupleDetail coupleParticipant)
        {
            this.coupleParticipant = coupleParticipant;
        }
        public ParticipantWrapper(Exception ex, string sourceMessage)
        {
            this.Exception = ex;
            this.SourceMessage = sourceMessage;
        }

        public override string ToString()
        {
            if (this.Exception != null)
            {
                return this.SourceMessage + " ####  " + this.Exception.Message;
            }
            return this.coupleParticipant != null
                ? string.Format("{0,-3} {1,-3} - {2,-60} {3}", this.coupleParticipant.Rank, this.coupleParticipant.StartNumber, this.coupleParticipant.Name, this.coupleParticipant.Country)
                : this.teamParticipant != null
                ? string.Format("{0,-3} {1,-3} - {2,-60} {3}", this.teamParticipant.Rank, this.teamParticipant.StartNumber, this.teamParticipant.Team, this.teamParticipant.Country)
                : string.Empty;
        }
    }
    internal class OfficialWrapper
    {
        public Wdsf.Api.Client.Models.OfficialDetail official;
        public OfficialWrapper(Wdsf.Api.Client.Models.OfficialDetail official)
        {
            this.official = official;
        }

        public override string ToString()
        {
            return this.official == null ? string.Empty : string.Format("{0,-3} {1,-10} - {2,-30} {3}", this.official.AdjudicatorChar, this.official.Task, this.official.Person, this.official.Nationality);
        }
    }
    internal class CompetitionWrapper
    {
        public Wdsf.Api.Client.Models.Competition competition;
        public CompetitionWrapper(Wdsf.Api.Client.Models.Competition competition)
        {
            this.competition = competition;
        }

        public override string ToString()
        {
            return competition.Name;
        }
    }
    internal class CoupleWrapper
    {
        public Wdsf.Api.Client.Models.Couple couple;
        public CoupleWrapper(Wdsf.Api.Client.Models.Couple couple)
        {
            this.couple = couple;
        }

        public override string ToString()
        {
            return string.Format("{0,-50} {1}", couple.Name, couple.Country);
        }
    }
}
