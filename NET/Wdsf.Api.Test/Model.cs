namespace Wdsf.Api.Test
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    internal class Model
    {
        internal BindingList<ParticipantWrapper> Participants { get; set; }
        internal BindingList<PersonWrapper> Adjudicator { get; set; }
        internal BindingList<PersonWrapper> Chairman { get; set; }
        internal BindingList<PersonWrapper> Athletes { get; set; }
        internal BindingList<CoupleWrapper> Couples { get; set; }

        internal BindingList<AgeWrapper> Ages { get; set; }
        internal BindingList<CountryWrapper> Countries { get; set; }

        internal string EventGroupId { get; set; }
        internal BindingList<CompetitionWrapper> Competitions { get; set; }
        internal BindingList<OfficialWrapper> CompetitionOfficials { get; set; }

        internal Client.Models.CoupleDetail Couple { get; set; }
        internal Client.Models.ParticipantCoupleDetail Participant { get; set; }
        internal Client.Models.PersonDetail Person { get; set; }
 
        public Model(Form form)
        {
            this.Adjudicator = new BindingListInvokable<PersonWrapper>(form);
            this.Ages = new BindingListInvokable<AgeWrapper>(form);
            this.Chairman = new BindingListInvokable<PersonWrapper>(form);
            this.Athletes = new BindingListInvokable<PersonWrapper>(form);
            this.Competitions = new BindingListInvokable<CompetitionWrapper>(form);
            this.CompetitionOfficials = new BindingListInvokable<OfficialWrapper>(form);
            this.Countries = new BindingListInvokable<CountryWrapper>(form);
            this.Couples = new BindingListInvokable<CoupleWrapper>(form);
            this.Participants = new BindingListInvokable<ParticipantWrapper>(form);
        }
   }

    internal class BindingListInvokable<T> : BindingList<T>
    {
        Form form;
        public BindingListInvokable(Form form)
            : base()
        {
            this.form = form;
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            if (this.form.InvokeRequired)
            {
                this.form.Invoke(new Action<ListChangedEventArgs>(OnListChanged), e); 
            }
            base.OnListChanged(e);
        }
    }
}
