using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wdsf.Api.Client;
using Wdsf.Api.Client.Exceptions;
using System.Windows;
using System.Threading;

namespace WDSF_Checkin.Model
{
    /// <summary>
    /// Event handler for our progress bar
    /// </summary>
    public class ProgressEventArgs: EventArgs
    {
        public int Progress {get; set; }
        public int MaxObjects { get; set; }
        public int CurrentObject { get; set; }
        public string State { get; set; }
    }


    public delegate void ProgressChangedEvent(object sender, ProgressEventArgs e);

    /// <summary>
    /// Implements the WDSF Api to download all WDSF Couples
    /// </summary>
    class WDSFAPI_Client
    {
        DataContext _context;
        private Client _apiClient;

        public event ProgressChangedEvent ProgressChanged;

        public WDSFAPI_Client(DataContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Downloads all couples via the WDSF api
        /// </summary>
        /// <param name="user">Username provided by WDSF</param>
        /// <param name="password">Password provided by WDSF</param>
        /// <param name="liveSystem">true, if using the live system</param>
        public void DownloadCouples(string user, string password, bool liveSystem)
        {
            if (ProgressChanged != null)
                ProgressChanged(this, new ProgressEventArgs() { Progress = 0, State = "Downloading List of Couples" });

            _apiClient = new Client(user, password, liveSystem ? WdsfEndpoint.Services : WdsfEndpoint.Sandbox);

            int currentProgress = 0;
            Timer t = new Timer(
                (object state) =>
                {
                    this.ProgressChanged(this, new ProgressEventArgs() { Progress = currentProgress++ % 100 });
                }, 
                null,
                1, 
                1000);

            var couples = _apiClient.GetCouples();

            t.Dispose();

            _context.WDSF_Couples.Clear();
            foreach (var couple in couples)
            {
                _context.WDSF_Couples.Add(new Couple()
                {
                    Man = new Person(){ FirstName = couple.ManName, LastName = couple.ManSurname, Id = couple.ManMin.ToString(), Country = couple.ManNationality},
                    Woman = new Person() { FirstName = couple.WomanName, LastName = couple.WomanSurname, Id = couple.WomanMin.ToString(), Country = couple.WomanNationality },
                    Id = couple.Id,
                    AgeGroup = couple.AgeGroup,
                    Country = couple.Country
                });
            }
        }

        internal void Abort()
        {
            this._apiClient.Dispose();
        }
    }
}
