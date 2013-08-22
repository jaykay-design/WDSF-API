using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WDSF_Checkin.Model;

namespace WDSF_Checkin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model.DataContext context;
        private ApiAdapter.IApiAdapter apiAdapter;

        public MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            InitializeComponent();

            this.apiAdapter = ApiAdapter.ApiAdapterFactory.GetInstance();

            if (!this.apiAdapter.Initialize())
            {
                Application.Current.Shutdown();
            }

            context.LoadParticipants(AppSettings.Default.ParticipantExportPath);

            var thread = new System.Threading.Thread(SocketReceiveThread);
            thread.IsBackground = true;
            thread.Start();

            Next_Click(null, null);
            // Let's show some statistics
            Statistics.Content = String.Format("Downloaded: {0}, {1} couples in the local database", context.DownloadDate.ToShortDateString(), context.WDSF_Couples.Count());

        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if(e.ExceptionObject is Exception)
            {
                Exception ex = (Exception) e.ExceptionObject;
                MessageBox.Show("Something went wrong: " + ex.Message + "\r\n" + ex.StackTrace, "Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
        }

        private void MIN_Man_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox_LostFocus(sender, e);

            int min;
            if (!int.TryParse(MIN_Man.Text, out min))
            {
                MIN_Man.Text = "";
                return;
            }

            var man = this.apiAdapter.GetPerson(min);
            if (man == null)
            {
                Text_Man.Content = "Not Found !!!";
                Text_Man.Foreground = Brushes.Red;
            }
            else
            {
                Text_Man.Content = man.Name;
                Text_Man.Foreground = Brushes.Green;
            }

			MIN_Woman.Focus();
        }

        private void MIN_Woman_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBox_LostFocus(sender, e);

            int min;
            if (!int.TryParse(MIN_Woman.Text, out min))
            {
                MIN_Woman.Text = "";
                return;
            }

            var woman = this.apiAdapter.GetPerson(min);

            if (woman == null)
            {
                Text_Woman.Content = "Not Found !!!";
            }
            else
            {
                Text_Woman.Content = woman.Name;
                Text_Woman.Foreground = Brushes.Green;
            }

            var couple = context.WDSF_Couples.FirstOrDefault(c => c.Man.Id == MIN_Man.Text && c.Woman.Id == MIN_Woman.Text);
            if(couple == null) // Try again the other way
                couple = context.WDSF_Couples.FirstOrDefault(c => c.Woman.Id == MIN_Man.Text && c.Man.Id == MIN_Woman.Text);

            if (couple == null)
            {
                Text_Couple.Content = "Not a valid WDSF Couple";
                Text_Couple.Foreground = Brushes.Red;
            }
            else
            {
                Text_Couple.Content = "Couple is valid (Age-Group " + couple.AgeGroup + ") / " + couple.Country;
                Text_Couple.Foreground = Brushes.Green;
                context.CurrentCouple = couple;
            }

            // We try to find this couple in our Competition List
            // we have our equal method for that
            var participant = context.Couples.FirstOrDefault(
                    p => p.Equals(couple)
                );
            if (participant != null)
                this.CompetitionList.ItemsSource = participant.Participations;
            else
                this.CompetitionList.ItemsSource = null;

            Next.Focus();
        }

        private void Next_Click(object sender, System.Windows.RoutedEventArgs e)
        {
        	MIN_Man.Text = "";
			MIN_Woman.Text = "";
            Text_Man.Content = "";
            Text_Woman.Content = "";
            Text_Couple.Content = "";
            CompetitionList.ItemsSource = null;
            MIN_Man.Focus();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox cb = (CheckBox)sender;
                if (cb.DataContext is Participant)
                {
                    var participant = (Model.Participant)cb.DataContext;
                    participant.CheckedIn = cb.IsChecked.HasValue ? cb.IsChecked.Value : false;
                }
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                var txt = (TextBox)sender;
                txt.Background = Brushes.LightGreen;
            }         
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox)
            {
                var txt = (TextBox)sender;
                txt.Background = Brushes.White;
            }
        }

        private void prntButton_Click(object sender, RoutedEventArgs e)
        {
            // We print all selected competitions of this couple ...
            if (CompetitionList.ItemsSource != null && CompetitionList.ItemsSource is List<Participant>)
            {
                var list = (List<Participant>)CompetitionList.ItemsSource;
                var checkin = list.Where(p => p.CheckedIn == true);
                foreach (var p in checkin)
                {
                    var doc = new NumberPrinter(p.Number, p.Couple.NiceName, p.Competition, "PrintDefinition.txt");
                    // print to default printer.
                    doc.PrintDocument(null, false);
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.P:
                    prntButton_Click(null, null);
                    break;
            }
        }

        private void SocketReceiveThread()
        {
            var udpclient = new UdpClient(12345, AddressFamily.InterNetwork) { EnableBroadcast = true };
            var remoteIPEndPoint = new IPEndPoint(0, 0);

            while(true)
            {
                try
                {
                    
                    var buffer = udpclient.Receive(ref remoteIPEndPoint);
                    // Wir brauchen nun einen String
                    var data = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
                    Console.WriteLine("Packet von: {0}: {1}", remoteIPEndPoint, data);
                    data += "\t";
                    // call dispatcher with the data
                    Dispatcher.Invoke(
                    DispatcherPriority.Normal,
                    (Action)delegate()
                                {
                                    if (MIN_Woman.IsFocused)
                                    {
                                        MIN_Woman.Text = data;
                                        Next.Focus();
                                    }

                                    if (MIN_Man.IsFocused)
                                    {
                                        MIN_Man.Text = data;
                                        MIN_Woman.Focus();
                                    }

                                }
                    );
                }catch(Exception ex)
                {
                    
                }

            }
        }
    }
}
