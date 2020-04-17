namespace Wdsf.Api.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Wdsf.Api.Client;
    using Wdsf.Api.Client.Interfaces;
    using API_Models = Wdsf.Api.Client.Models;

    public partial class Form1 : Form
    {
        IClient apiClient;
        string[] args;
        internal Model model;

        internal BindingListInvokable<string> errors;

        public Form1(string[] args)
        {
            this.args = args;
            
            InitializeComponent();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string username = args.FirstOrDefault(a => a.StartsWith("username"));
            if (username == null)
            {
                MessageBox.Show("No API user name given");
                Application.Exit();
                return;
            }
            else
            {
                username = username.Replace("username=", string.Empty);
            }

            string password = args.FirstOrDefault(a => a.StartsWith("password"));
            if (password == null)
            {
                MessageBox.Show("No API password given");
                Application.Exit();
                return;
            }
            else
            {
                password = password.Replace("password=", string.Empty);
            }

            this.errors = new BindingListInvokable<string>(this);

            this.model = new Model(this);
            this.comboBoxCoupleCountry.DataSource = this.model.Countries;
            this.checkedListBoxCoupleAgeGroup.DataSource = this.model.Ages;
            this.checkedListBoxAthleteAgeGroup.DataSource = this.model.Ages;
            this.listBoxCompetitions.DataSource = this.model.Competitions;
            this.listBoxCouples.DataSource = this.model.Couples;
            this.listBoxAthletes.DataSource = this.model.Athletes;
            this.listBoxParticipants.DataSource = this.model.Participants;
            this.listBoxOfficials.DataSource = this.model.CompetitionOfficials;
            this.listBoxErrors.DataSource = this.errors;

            string apiUrl = args.FirstOrDefault(a => a.StartsWith("apiurl"));

            if (apiUrl == null)
            {
                this.Text = string.Format(this.Text, "LIVE");
                this.apiClient = new Api.Client.Client(username, password, Wdsf.Api.Client.WdsfEndpoint.Services);
            }
            else
            {
                apiUrl = apiUrl.Replace("apiurl=", string.Empty);
                this.Text = string.Format(this.Text, apiUrl);
                this.apiClient = new Api.Client.Client(username, password, apiUrl);
            }

            ((Api.Client.Client)this.apiClient).ContentType = ContentTypes.Xml;

            this.apiClient.GetAges().ToList().ForEach(c => this.model.Ages.Add(new AgeWrapper(c)));
            this.apiClient.GetCountries().ToList().ForEach(c => this.model.Countries.Add(new CountryWrapper(c)));
            this.apiClient.GetPersons(new Dictionary<string, string>() { { "type", "Adjudicator" } }).ToList().ForEach(c => this.model.Adjudicator.Add(new PersonWrapper(c)));
            this.apiClient.GetPersons(new Dictionary<string, string>() { { "type", "Chairman" } }).ToList().ForEach(c => this.model.Chairman.Add(new PersonWrapper(c)));

            foreach (string arg in args)
            {
                if (arg.StartsWith("groupId="))
                {
                    this.model.EventGroupId = arg.Replace("groupId=", string.Empty);
                    this.textBoxGroupId.Text = this.model.EventGroupId.ToString();
                    LoadCompetitions();
                }
                if (arg.StartsWith("athletes="))
                {
                    string[] p = arg.Replace("athletes=", string.Empty).Split('|');
                    this.checkedListBoxAthleteAgeGroup.SetItemChecked(this.checkedListBoxAthleteAgeGroup.Items.IndexOf(this.model.Ages.First(a => a.age.Name == p[0])), true);
                    this.checkedListBoxAthleteStatus.SetItemChecked(this.checkedListBoxAthleteStatus.Items.IndexOf(p[1]), true);
                    this.checkedListBoxAthleteDivision.SetItemChecked(this.checkedListBoxAthleteDivision.Items.IndexOf(p[2]), true);
                    LoadAthletes();
                }
                if (arg.StartsWith("couples="))
                {
                    string[] p = arg.Replace("couples=", string.Empty).Split('|');
                    this.checkedListBoxCoupleAgeGroup.SetItemChecked(this.checkedListBoxCoupleAgeGroup.Items.IndexOf(this.model.Ages.First(a => a.age.Name == p[0])), true);
                    this.checkedListBoxCoupleStatus.SetItemChecked(this.checkedListBoxCoupleStatus.Items.IndexOf(p[1]), true);
                    this.checkedListBoxCoupleDivision.SetItemChecked(this.checkedListBoxCoupleDivision.Items.IndexOf(p[2]), true);
                    LoadCouples();
                }
            }
        }

        private void buttonLoadCouples_Click(object sender, EventArgs e)
        {
            LoadCouples();
        }
        private void LoadCouples()
        {
            new Task(() =>
            {
                lock (this.model.Couples)
                {
                    this.model.Couples.Clear();
                    this.model.Couples.RaiseListChangedEvents = false;
                    int min;
                    bool isMin = this.textBoxCoupleName.Text.Length == 8 && int.TryParse(this.textBoxCoupleName.Text, out min);

                    SetProgressBar(this.progressBarCouples, 0, 100);
                    using (var timer = new System.Threading.Timer(state => { SetProgressBar(this.progressBarCouples); }, null, 0, 100))
                    {

                        this.apiClient.GetCouples(new Dictionary<string, string>()
                        {
                            { isMin ? "min": "name", this.textBoxCoupleName.Text},
                            { "ageGroup", string.Join(",", checkedListBoxCoupleAgeGroup.CheckedItems.OfType<AgeWrapper>().Select(a=> a.age.Name))},
                            { "status", string.Join(",", checkedListBoxCoupleStatus.CheckedItems.OfType<string>())},
                            { "division", string.Join(",", checkedListBoxCoupleDivision.CheckedItems.OfType<string>())}
                        })
                        .ToList()
                        .ForEach(c => this.model.Couples.Add(new CoupleWrapper(c)));
                    }
                    SetProgressBar(this.progressBarCouples, -1, -1);

                    this.model.Couples.RaiseListChangedEvents = true;
                    this.model.Couples.ResetBindings();

                    MessageBox.Show("All couples loaded.");
                }
            }).Start();
        }     
        private void buttonAddCouple_Click(object sender, EventArgs e)
        {
            API_Models.CoupleDetail couple = new API_Models.CoupleDetail()
            {
                Country = this.comboBoxCoupleCountry.SelectedItem.ToString(),
                Division = (string)this.comboBoxCoupleDivision.SelectedItem,
                ManMin = int.Parse(this.textBoxManMin.Text),
                WomanMin = int.Parse(this.textBoxWomanMin.Text),
                Status = (string)this.comboBoxCoupleStatus.SelectedItem,
                RetiredOn  = this.dateTimePickerCoupleRetireOn.Value.ToString("yyyy-MM-dd")
            };
            try
            {
                Uri coupleUri = this.apiClient.SaveCouple(couple);
                MessageBox.Show(string.Format("Created new couple {0}", coupleUri.ToString()));
            }
            catch (Api.Client.Exceptions.ApiException ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }
        }

        private void buttonLoadCompetitions_Click(object sender, EventArgs e)
        {
            this.model.EventGroupId = textBoxGroupId.Text;
            LoadCompetitions();
        }
        private void LoadCompetitions()
        {
            tabControl1.SelectTab(tabPageCompetitions);
            this.model.Competitions.Clear();

            new Task(() =>
            {
                lock (this.model.Competitions)
                {
                    this.apiClient.GetCompetitions(
                        new Dictionary<string, string>() 
                    { 
                        { "groupId", this.model.EventGroupId }
                    })
                    .ToList()
                    .ForEach(c => this.model.Competitions.Add(new CompetitionWrapper(c)));
                }
            }).Start();
        }

        private void buttonLoadParticipants_Click(object sender, EventArgs e)
        {
            this.model.Participants.Clear();

            var competition = listBoxCompetitions.SelectedItem as CompetitionWrapper;

            DateTime start = DateTime.Now;
            var allParticipants = this.apiClient.GetCoupleParticipants(competition.competition.Id);
            List<Task> tasks = new List<Task>();

            if (allParticipants.Count == 0)
            {
                MessageBox.Show("No participants.");
                return;
            }
            foreach (var p in allParticipants)
            {
                tasks.Add(new Task(new Action<object>( state =>
                {
                    try
                    {
                        var pa = new ParticipantWrapper(this.apiClient.GetCoupleParticipant((int)state));
                        lock (this.model.Participants)
                        {
                            this.model.Participants.Add(pa);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }), p.Id));
            }

            Task.Factory.ContinueWhenAll(tasks.ToArray(), new Action<Task[]>(t =>
            {
                MessageBox.Show("All participants loaded.");
            }));

            foreach (var t in tasks)
            {
                t.Start();
            }
        }
        private void buttonDeleteAllParticipants_Click(object sender, EventArgs e)
        {
            var competition = listBoxCompetitions.SelectedItem as CompetitionWrapper;

            List<Task> tasks = new List<Task>();
            foreach (var p in this.listBoxParticipants.Items.OfType<ParticipantWrapper>())
            {
                tasks.Add(new Task(new Action<object>(state =>
                {
                    ParticipantWrapper pa = state as ParticipantWrapper;
                    if (pa.coupleParticipant != null)
                    {
                        this.apiClient.DeleteCoupleParticipant(pa.coupleParticipant.Id);
                    }
                    lock (this.model.Participants)
                    {
                        this.model.Participants.Remove(pa);
                    }
                }), p));
            }

            if (tasks.Count == 0)
            {
                return;
            }

            foreach (var t in tasks)
            {
                t.Start();
            }

            Task.Factory.ContinueWhenAll(tasks.ToArray(), new Action<Task[]>(t =>
            {
                MessageBox.Show("All participants deleted.");
            }));
        }
        private void buttonFillWithParticipants_Click(object sender, EventArgs e)
        {
            List<Task> tasks = new List<Task>();
            Random random = new Random();
            int competitionId = (this.listBoxCompetitions.SelectedItem as CompetitionWrapper).competition.Id;
            int total = int.Parse(this.textBoxParticipantCount.Text);
            int start = int.Parse(this.textBoxFirstStartNumber.Text);
            int max = listBoxCouples.Items.Count - 1;
            List<int> steps = new List<int> { 192, 96, 48, 24, 12, 6, 0 };
            int maxRounds = 7 - steps.IndexOf(steps.First(s => s < total));

            if (this.model.CompetitionOfficials.Count == 0)
            {
                MessageBox.Show("No officils loaded.");
                return;
            }

            string scoreType = this.checkBoxIncludeScores.Checked ? this.comboBoxScoreType.SelectedItem.ToString() : string.Empty;

            SetProgressBar(this.progressBarParticipants, 0, total);
            for (int i = start; i < start + total; i++)
            {
                tasks.Add(new Task(new Action<object>((state) =>
                {
                    int startNumber = (int)state;
                    int rank = startNumber - start + 1;
                    CoupleWrapper couple = listBoxCouples.Items[random.Next(0, max)] as CoupleWrapper;
                    int rounds = steps.IndexOf(steps.First(s => s < rank)) + 1;

                    var model = new Api.Client.Models.ParticipantCoupleDetail()
                    {
                        CoupleId = couple.couple.Id,
                        CompetitionId = competitionId,
                        Status = "Present",
                        StartNumber = startNumber,
                        Rank = rank.ToString()
                    };
                    ModelFiller.Fill(model, scoreType, rounds, maxRounds, this.model.CompetitionOfficials);

                    try
                    {
                        Uri participantUrl = apiClient.SaveCoupleParticipant(model);
                        var participant = apiClient.Get<API_Models.ParticipantCoupleDetail>(participantUrl);
                        lock (this.model.Participants)
                        {
                            this.model.Participants.Add(new ParticipantWrapper(participant));
                        }
                    }
                    catch (Exception ex)
                    {
                        lock (this.model.Participants)
                        {
                            this.model.Participants.Add(new ParticipantWrapper(ex.InnerException, couple.ToString()));
                        }
                    }

                    SetProgressBar(this.progressBarParticipants);
                }), i));
            }

            foreach (var t in tasks)
            {
                t.Start();
            }

            Task.Factory.ContinueWhenAll(tasks.ToArray(), new Action<Task[]>(t =>
            {
                SetProgressBar(this.progressBarParticipants, -1, -1);
                MessageBox.Show("All participants created.");
            }));
        }
        private void buttonAddParticipant_Click(object sender, EventArgs e)
        {

        }

        private void buttonLoadAthletes_Click(object sender, EventArgs e)
        {
            LoadAthletes();
        }
        private void LoadAthletes()
        {
            new Task(() =>
            {
                lock (this.model.Athletes)
                {
                    this.model.Athletes.Clear();
                    this.model.Athletes.RaiseListChangedEvents = false;
                    int min;
                    bool isMin = this.textBoxPersonName.Text.Length == 8 && int.TryParse(this.textBoxPersonName.Text, out min);


                    SetProgressBar(this.progressBarAthletes, 0, 100);
                    using (var timer = new System.Threading.Timer(state => { SetProgressBar(this.progressBarAthletes); }, null, 0, 100))
                    {
                        this.apiClient.GetPersons(new Dictionary<string, string>()
                        {
                            { isMin ? "min" : "name", this.textBoxPersonName.Text},
                            { "type", "Athlete" },
                            { "ageGroup", string.Join(",", checkedListBoxAthleteAgeGroup.CheckedItems.OfType<AgeWrapper>().Select(a=> a.age.Name))},
                            { "status", string.Join(",", checkedListBoxAthleteStatus.CheckedItems.OfType<string>())},
                            { "division", string.Join(",", checkedListBoxAthleteDivision.CheckedItems.OfType<string>())},
                        })
                        .ToList()
                        .ForEach(a => this.model.Athletes.Add(new PersonWrapper(a)));
                    }
                    SetProgressBar(this.progressBarAthletes, -1, -1);

                    this.model.Athletes.RaiseListChangedEvents = true;
                    this.model.Athletes.ResetBindings();

                    MessageBox.Show("All athletes loaded.");
                }
            }).Start();
        }
 
        private void buttonLoadOfficials_Click(object sender, EventArgs e)
        {
            LoadCompetitionOfficials();
        }
        private void LoadCompetitionOfficials()
        {
            this.model.CompetitionOfficials.Clear();

            var competition = listBoxCompetitions.SelectedItem as CompetitionWrapper;

            var allOfficials = this.apiClient.GetOfficials(competition.competition.Id);

            if (allOfficials.Count == 0)
            {
                MessageBox.Show("No officials.");
                return;
            }
            List<Task> tasks = new List<Task>();
            foreach (var p in allOfficials)
            {
                tasks.Add(new Task(new Action<object>(state =>
                {
                    var of = new OfficialWrapper(this.apiClient.GetOfficial((int)state));
                    lock (this.model.CompetitionOfficials)
                    {
                        this.model.CompetitionOfficials.Add(of);
                    }
                }), p.Id));
            }

            Task.Factory.ContinueWhenAll(tasks.ToArray(), new Action<Task[]>(t =>
            {
                MessageBox.Show("All officials loaded.");
            }));
            foreach (var t in tasks)
            {
                t.Start();
            }
        }
        private void buttonDeleteAllOfficials_Click(object sender, EventArgs e)
        {
            var competition = listBoxCompetitions.SelectedItem as CompetitionWrapper;

            List<Task> tasks = new List<Task>();
            foreach (var p in this.listBoxOfficials.Items.OfType<OfficialWrapper>())
            {
                tasks.Add(new Task(new Action<object>(state =>
                {
                    OfficialWrapper pa = state as OfficialWrapper;
                    this.apiClient.DeleteOfficial(pa.official.Id);
                    lock (this.model.CompetitionOfficials)
                    {
                        this.model.CompetitionOfficials.Remove(pa);
                    }
                }), p));
            }

            if (tasks.Count == 0)
            {
                return;
            }

            Task.Factory.ContinueWhenAll(tasks.ToArray(), new Action<Task[]>(t =>
            {
                MessageBox.Show("All officials deleted.");
            }));

            foreach (var t in tasks)
            {
                t.Start();
            }
        }
        private void buttonFillWithOfficials_Click(object sender, EventArgs e)
        {
            var competition = listBoxCompetitions.SelectedItem as CompetitionWrapper;

            List<Task> tasks = new List<Task>();
            Random random = new Random();
            tasks.Add(new Task(new Action(() =>
            {
                var chairman = this.model.Chairman[random.Next(0, this.model.Chairman.Count - 1)];
                try
                {
                    Uri chairmanUri = this.apiClient.SaveOfficial(new Api.Client.Models.OfficialDetail()
                    {
                        AdjudicatorChar = "CH",
                        Task = "Chairman",
                        CompetitionId = competition.competition.Id,
                        Min = chairman.person.Min
                    });
                    var ch = new OfficialWrapper(this.apiClient.Get<API_Models.OfficialDetail>(chairmanUri));
                    lock (this.model.CompetitionOfficials)
                    {
                        this.model.CompetitionOfficials.Add(ch);
                    }
                }
                catch (Api.Client.Exceptions.ApiException ex)
                {
                    this.errors.Add(ex.InnerException.Message);
                }

            })));

            List<int> indexes = new List<int>();
            for (int index = 0; index < 100; index++)
            {
                indexes.Add(random.Next(0, this.model.Adjudicator.Count - 1));
            }
            var adjChar = 'A';
            foreach(int i in indexes.Distinct().Take(int.Parse(textBoxOfficialsCount.Text)))
            {
                var adjChLocal = adjChar;
                adjChar++;

                tasks.Add(new Task(new Action<object>((state) =>
                {
                    var adjudicator = this.model.Adjudicator[(int)state];
                    try
                    {
                        Uri adjudicatorUri = this.apiClient.SaveOfficial(new Api.Client.Models.OfficialDetail()
                        {
                            AdjudicatorChar = adjChLocal.ToString(),
                            Task = "Adjudicator",
                            CompetitionId = competition.competition.Id,
                            Min = adjudicator.person.Min
                        });
                        var adj = new OfficialWrapper(this.apiClient.Get<API_Models.OfficialDetail>(adjudicatorUri));
                        lock (this.model.CompetitionOfficials)
                        {
                            this.model.CompetitionOfficials.Add(adj);
                        }
                    }
                    catch (Api.Client.Exceptions.ApiException ex)
                    {
                        this.errors.Add(ex.InnerException.Message);
                    }

                }), i));

            }

            Task.Factory.ContinueWhenAll(tasks.ToArray(), new Action<Task[]>(t =>
            {
                MessageBox.Show("All officials loaded.");
            }));
            foreach (var t in tasks)
            {
                t.Start();
            }
        }

        private void textBoxManMin_DragEnter(object sender, DragEventArgs e)
        {
            PersonWrapper a = (PersonWrapper)e.Data.GetData(typeof(PersonWrapper));
            if (a != null && a.person.Sex == Gender.Male)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void textBoxManMin_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PersonWrapper)))
            {
                PersonWrapper a = (PersonWrapper)e.Data.GetData(typeof(PersonWrapper));
                if(a.person.Sex == Gender.Male)
                {
                    textBoxManMin.Text = ((PersonWrapper)e.Data.GetData(typeof(PersonWrapper))).person.Min.ToString();
                }
            }
        }
        private void textBoxWomanMin_DragEnter(object sender, DragEventArgs e)
        {
            PersonWrapper a = (PersonWrapper)e.Data.GetData(typeof(PersonWrapper));
            if (a != null && a.person.Sex == Gender.Female)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void textBoxWomanMin_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PersonWrapper)))
            {
                PersonWrapper a = (PersonWrapper)e.Data.GetData(typeof(PersonWrapper));
                if (a.person.Sex == Gender.Female)
                {
                    textBoxWomanMin.Text = ((PersonWrapper)e.Data.GetData(typeof(PersonWrapper))).person.Min.ToString();
                }
            }
        }
        private void textBoxCoupleId_DragEnter(object sender, DragEventArgs e)
        {
            CoupleWrapper a = (CoupleWrapper)e.Data.GetData(typeof(CoupleWrapper));
            if (a != null)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void textBoxCoupleId_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(CoupleWrapper)))
            {
                CoupleWrapper a = (CoupleWrapper)e.Data.GetData(typeof(CoupleWrapper));
                textBoxCoupleId.Text = ((CoupleWrapper)e.Data.GetData(typeof(CoupleWrapper))).couple.Id;
            }
        }
        private void listBoxDragable_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks == 2)
            {
                return;
            }
            ListBox source = sender as ListBox;
            int indexOfItem = source.IndexFromPoint(e.X, e.Y);
            if (indexOfItem >= 0 && indexOfItem < source.Items.Count)
            {
                source.DoDragDrop(source.Items[indexOfItem], DragDropEffects.Copy);
            }
        }

        private void listBoxParticipants_DoubleClick(object sender, EventArgs e)
        {
            ParticipantWrapper p = this.listBoxParticipants.SelectedItem as ParticipantWrapper;

            var participant = this.apiClient.GetCoupleParticipant(p.coupleParticipant.Id);
            this.model.Participant = participant;

            var country = this.model.Countries.First(co => co.country.Name == participant.Country);

            this.textBoxCoupleId.Text = participant.CoupleId;
            this.textBoxStartNumber.Text = participant.StartNumber.ToString();
            this.comboBoxParticipantStatus.SelectedIndex = this.comboBoxParticipantStatus.Items.IndexOf(participant.Status);
        }

        private void listBoxCouples_DoubleClick(object sender, EventArgs e)
        {
            CoupleWrapper c = this.listBoxCouples.SelectedItem as CoupleWrapper;

            var couple = this.apiClient.GetCouple(c.couple.Id);
            this.model.Couple = couple;

            var country = this.model.Countries.First(co => co.country.Name == couple.Country);

            this.textBoxManMin.Text = couple.ManMin.ToString();
            this.textBoxWomanMin.Text = couple.WomanMin.ToString();
            this.comboBoxCoupleCountry.SelectedIndex = this.comboBoxCoupleCountry.Items.IndexOf(country);
            this.comboBoxCoupleDivision.SelectedIndex = this.comboBoxCoupleDivision.Items.IndexOf(couple.Division);
            this.comboBoxCoupleStatus.SelectedIndex = this.comboBoxCoupleStatus.Items.IndexOf(couple.Status);
            this.labelCoupleWrlBlocked.Text = couple.WrlBlockedUntil;
            this.labelCoupleChBlocked.Text = couple.CupOrChampionshipBlockedUntil;
            if (string.IsNullOrEmpty(couple.RetiredOn))
            {
                this.dateTimePickerCoupleRetireOn.Value = new DateTime(1800, 1, 1);
            }
            else
            {
                this.dateTimePickerCoupleRetireOn.Value = DateTime.Parse(couple.RetiredOn);
            }
        }
        private void listBoxAthletes_DoubleClick(object sender, EventArgs e)
        {
            PersonWrapper p = this.listBoxAthletes.SelectedItem as PersonWrapper;

            API_Models.PersonDetail person = this.apiClient.GetPerson(p.person.Min);
            this.model.Person = person;

            if (model.Person.Licenses == null)
            {
                MessageBox.Show("No licenses.");
                return;
            }

            API_Models.License license = person.Licenses.FirstOrDefault(l => l.Type == "Athlete");
            if (license == null)
            {
                return;
            }

            this.textBoxAthleteMin.Text = person.Min.ToString();
            this.comboBoxAthleteDivision.SelectedIndex = this.comboBoxCoupleDivision.Items.IndexOf(license.Division);
            this.comboBoxAthleteStatus.SelectedIndex = this.comboBoxAthleteStatus.Items.IndexOf(license.Status);
            this.labelAthleteWrlBlocked.Text = license.WrlBlockedUntil;
            this.labelAthleteChBlocked.Text = license.CupOrChampionshipBlockedUntil;
            if (string.IsNullOrEmpty(license.ExpiresOn))
            {
                this.dateTimePickerAthleteRetireOn.Value = new DateTime(1800, 1, 1);
            }
            else
            {
                this.dateTimePickerAthleteRetireOn.Value = DateTime.Parse(license.ExpiresOn);
            }
        }

        private void SetProgressBar(ProgressBar progressBar, int value = -1, int max = 0)
        {
            if (this.InvokeRequired && ! this.IsDisposed)
            {
                this.Invoke(new Action<ProgressBar, int, int>(this.SetProgressBar), progressBar, value, max);
                return;
            }

            if (max > 0)
            {
                progressBar.Maximum = max;
                progressBar.Visible = true;
            }
            else if (max == -1)
            {
                progressBar.Visible = false;
            }

            if (value != -1)
            {
                progressBar.Value = value;
            }
            else
            {
                progressBar.Value = (progressBar.Value + 1) % progressBar.Maximum;
            }
        }

        private void buttonUpdatePerson_Click(object sender, EventArgs e)
        {
            API_Models.PersonDetail person = this.model.Person;
            if (person == null)
            {
                return;
            }
            API_Models.License license = person.Licenses.First(l => l.Type == "Athlete");
            if (license == null)
            {
                return;
            }

            license.Division = (string)this.comboBoxAthleteDivision.SelectedItem;
            license.Status = (string)this.comboBoxAthleteStatus.SelectedItem;
            if(this.dateTimePickerAthleteRetireOn.Value.Year != 1800)
            {
                license.ExpiresOn = this.dateTimePickerAthleteRetireOn.Value.ToString("yyyy-MM-dd");
            }

            try
            {
                if (!this.apiClient.UpdatePerson(person))
                {
                    MessageBox.Show(this.apiClient.LastApiMessage);
                }
                else
                {
                    MessageBox.Show("License updated");
                }
            }
            catch (Api.Client.Exceptions.ApiException ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }

            listBoxAthletes_DoubleClick(null, null);
        }

        private void buttonUpdateCouple_Click(object sender, EventArgs e)
        {
            API_Models.CoupleDetail couple = this.model.Couple;
            if (couple == null)
            {
                return;
            }

            couple.Division = (string)this.comboBoxCoupleDivision.SelectedItem;
            couple.Status = (string)this.comboBoxCoupleStatus.SelectedItem;

            if (this.dateTimePickerCoupleRetireOn.Value.Year != 1800)
            {
                couple.RetiredOn = this.dateTimePickerCoupleRetireOn.Value.ToString("yyyy-MM-dd");
            }

            try
            {
                if (!this.apiClient.UpdateCouple(couple))
                {
                    MessageBox.Show(this.apiClient.LastApiMessage);
                }
                else
                {
                    MessageBox.Show("Couple updated");
                }
            }
            catch (Api.Client.Exceptions.ApiException ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }

            listBoxCouples_DoubleClick(null, null);
        }

        private void buttonCreateRandomCouples_Click(object sender, EventArgs e)
        {
            if (this.model.Athletes.Count == 0)
            {
                return;
            }

            var men = this.model.Athletes.Where(a=> a.person.Sex == "Male").ToList();
            var women = this.model.Athletes.Where(a=> a.person.Sex == "Female");
            int total = int.Parse(this.textBoxRandomCoupleCount.Text);
            Random rnd = new Random();
            List<string> result = new List<string>();
            for (int index = 0; index < total; index++)
            {
                API_Models.PersonDetail man = this.apiClient.GetPerson(men[rnd.Next(0, men.Count() - 1)].person.Min);
                var others = women.Where(a => a.person.Country == man.Country);
                API_Models.PersonDetail woman = this.apiClient.GetPerson(others.Skip(rnd.Next(0, others.Count() - 1)).First().person.Min);

                API_Models.CoupleDetail couple = new API_Models.CoupleDetail()
                    {
                        ManMin = man.Min,
                        WomanMin = woman.Min,
                        Country = man.Country,
                        Division = man.Licenses.First().Division,
                        Status = "Active"
                    };

                try
                {
                    Uri coupleUri = this.apiClient.SaveCouple(couple);
                    result.Add(coupleUri.ToString());
                }
                catch (Exception ex)
                {
                    result.Add(ex.InnerException !=null ? ex.InnerException.Message : ex.Message);
                }
            }

            MessageBox.Show(string.Join("\n",result));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(Model));
            using (System.IO.FileStream o = new System.IO.FileStream("C:\temp.xml", System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                s.Serialize(o, this.model);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(Model));
            using (System.IO.FileStream o = new System.IO.FileStream("C:\temp.xml", System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                this.model = s.Deserialize(o) as Model;
            }
        }

        private void buttonUpdateParticipant_Click(object sender, EventArgs e)
        {
            model.Participant.Status = (string)this.comboBoxCoupleStatus.SelectedValue;

            if (this.model.CompetitionOfficials.Count == 0)
            {
                MessageBox.Show("No officils loaded.");
                return;
            }

            string scoreType = this.checkBoxIncludeScores.Checked ? this.comboBoxScoreType.SelectedItem.ToString() : string.Empty;

            if (this.checkBoxIncludeScoresSingle.Checked)
            {
                ModelFiller.Fill(model.Participant, scoreType, 4, 4, this.model.CompetitionOfficials);
            }

            try
            {
                bool success = apiClient.UpdateCoupleParticipant(model.Participant);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException.Message);
            }

        }
    }
}
