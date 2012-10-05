using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WDSF_Checkin
{
    /// <summary>
    /// Interaction logic for Download.xaml
    /// </summary>
    public partial class Download : Window
    {
        // our data context
        Model.DataContext _context;
        System.Threading.Thread _thread;

        public event EventHandler OnComplete;

        public Download(Model.DataContext context)
        {
            _context = context;
            InitializeComponent();
            Progress.Maximum = 65;
            Progress.Minimum = 0;
        }

        private void run()
        {
            var client = new Model.WDSFAPI_Client(_context);
            client.ProgressChanged += new Model.ProgressChangedEvent(ProgressChanged);

            try
            {
                client.DownloadCouples(AppSettings.Default.API_User,
                                        AppSettings.Default.API_Password,
                                        AppSettings.Default.API_ProductionSystem);
            }
            catch (Wdsf.Api.Client.Exceptions.ApiException ex)
            {
                if (ex.InnerException.GetType() == typeof(System.Threading.ThreadAbortException))
                {
                    client.Abort();
                }
                else
                {
                    MessageBox.Show("Could not download couples, please check the internet connection (Message: " + ex.Message + " - " + (ex.InnerException != null ? ex.InnerException.Message : "") + ")", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return;
            }

            // Now we save this data
            _context.SaveWDSFCouples(AppSettings.Default.CoupleFile);

            // we are done ... exit
            this.Dispatcher.Invoke(new System.Threading.ThreadStart(delegate()
            {
                this.Close();
                MessageBox.Show("Downloaded all couples from WDSF.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.OnComplete(this, new EventArgs());
            }));
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (button1.Content.ToString() == "Start")
            {
                _thread = new System.Threading.Thread(run);
                button1.Content = "Cancel";
                _thread.Start();
            }
            else
            {
                _thread.Abort();
                this.Close();
            }
        }

        void ProgressChanged(object sender, Model.ProgressEventArgs e)
        {
            this.Dispatcher.Invoke(new System.Threading.ThreadStart(delegate()
            {
                Progress.Value = e.Progress;
            }));
        }
    }
}
