using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace WDSF_Checkin.Model
{
    public class InternalCouple
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public bool Processed {get; set;}
    }
    /// <summary>
    /// Holds all our data lists of couples and participants
    /// </summary>
    public class DataContext : System.ComponentModel.INotifyPropertyChanged
    {
        public List<Person> Persons { get; set; }
        // Couples are all our registered couples
        public List<Couple> Couples { get; set; }
        // This list contains all wdsf registered couples
        public List<Couple> WDSF_Couples { get; set; }

        public DateTime DownloadDate { get; set; }

        private Couple _currentCouple;
        public Couple CurrentCouple
        {
            get { return _currentCouple; }
            set 
            { 
                _currentCouple = value;
                RaiseEvent("CurrentCouple");
            }
        }

        public DataContext()
        {
            Persons = new List<Person>();
            WDSF_Couples = new List<Couple>();
            Couples = new List<Couple>();
        }

        public void LoadParticipants(string file)
        {
            System.IO.StreamReader sr = null;

            try
            {
                sr = new System.IO.StreamReader(file, System.Text.Encoding.UTF8);
            }
            catch (Exception)
            {        
                return;
            }
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                try
                {
                    var data = line.Split(';');
                    if (data.Length < 12)
                        data = line.Split(','); // Try to split comma
                    // Do we have this couple?
                    var c = Couples.FirstOrDefault(co => co.Man.FirstName == data[4]
                                                         && co.Man.LastName == data[5]
                                                         && co.Woman.FirstName == data[6]
                                                         && co.Woman.LastName == data[7]
                        );
                    if (c == null)
                    {
                        c = new Couple();
                        c.Man = new Person() { FirstName = data[4], LastName = data[5], Country = data[8] };
                        c.Woman = new Person() { FirstName = data[6], LastName = data[7], Country = data[8] };
                        Couples.Add(c);
                    }
                    // Add the competition to your competition list
                    c.AddParticipation(data[0], data[11]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("mal-formed participant file in line " + line);
                }
            }

            sr.Close();
        }
        /// <summary>
        /// Loads the WDSF couples from a local file
        /// </summary>
        /// <param name="filename">Filename of file to load</param>
        public void LoadWDSFCouples(string filename)
        {
            var doc = new System.Xml.XmlDocument();
            doc.Load(filename);
            DateTime date;
            if (!DateTime.TryParse(doc["Couples"].Attributes["date"].Value, out date))
                DownloadDate = new DateTime(2000, 1, 1, 0, 0, 0, 0);
            else
                DownloadDate = date;

            var list = doc.GetElementsByTagName("Couple");
            foreach (var node in list)
            {
                var c = new Couple();
                c.FromXml((System.Xml.XmlNode) node);
                this.WDSF_Couples.Add(c);
                this.Persons.Add(c.Man);
                this.Persons.Add(c.Woman);
            }
        }
        /// <summary>
        /// Save the WDSF couples to a local XML file
        /// </summary>
        /// <param name="filename">XML file to load</param>
        public void SaveWDSFCouples(string filename)
        {
            var outS = new System.IO.StreamWriter(filename);

            outS.WriteLine(String.Format("<Couples date=\"{0}\">", DateTime.Now.ToString()));
            foreach (var c in WDSF_Couples)
            {
                outS.WriteLine(c.GetXml());
            }
            outS.WriteLine("</Couples>");
            outS.Close();
        }

        private void RaiseEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Id { get; set; }
        public string Country { get; set; }      
    }

    public class Couple
    {
        public string Id { get; set; }
        public Person Man { get; set; }
        public Person Woman { get; set; }
        public string Country { get; set; }
        public string AgeGroup { get; set; }
        public string Division { get; set; }
        public List<Participant> Participations { get; set; }

        public string NiceName
        {
            get
            {
                return String.Format("{0} {1} - {2} {3}", Man.FirstName, Man.LastName, Woman.FirstName, Woman.LastName);
            }
        }

        public Couple()
        {
            Participations = new List<Participant>();
        }

        public void AddParticipation(string competition, string number)
        {
            if(Participations.Count(c => c.Competition == competition) == 0)
            {
                Participations.Add(new Participant(){Competition = competition, Number = number, Couple = this});
            }
        }
        /// <summary>
        /// Returns an XML string representing this couple
        /// </summary>
        /// <returns></returns>
        public string GetXml()
        {
            var xmlMan = String.Format("<FirstName>{0}</FirstName><LastName>{1}</LastName><Country>{2}</Country>", this.Man.FirstName, this.Man.LastName, this.Man.Country);
            var xmlWoman = String.Format("<FirstName>{0}</FirstName><LastName>{1}</LastName><Country>{2}</Country>", this.Woman.FirstName, this.Woman.LastName, this.Woman.Country);
            var xml = String.Format("<Couple id=\"{0}\"><Country>{5}</Country><Man id=\"{1}\">{2}</Man><Woman id=\"{3}\">{4}</Woman><AgeGroup>{6}</AgeGroup><Division>{7}</Division></Couple>", 
                                    this.Id, this.Man.Id, xmlMan, this.Woman.Id, xmlWoman, this.Country, this.AgeGroup, this.Division);
            return xml;
        }
        /// <summary>
        /// Sets the properties of this instance from an XML Node
        /// </summary>
        /// <param name="node">The xml node containing the couples data</param>
        public void FromXml(System.Xml.XmlNode node)
        {
              
            var man = new Person() 
            {
                Id = node["Man"].Attributes["id"].Value,
                FirstName = node["Man"]["FirstName"].InnerText,
                LastName = node["Man"]["LastName"].InnerText,
                Country = node["Man"]["Country"].InnerText
            };

            var woman = new Person()
            {
                Id = node["Woman"].Attributes["id"].Value,
                FirstName = node["Woman"]["FirstName"].InnerText,
                LastName = node["Woman"]["LastName"].InnerText,
                Country = node["Woman"]["Country"].InnerText,
            };

            this.Woman = woman;
            this.Man = man;
            this.Country = node["Country"].InnerText;
            this.AgeGroup = node["AgeGroup"].InnerText;
            this.Division = node["Division"].InnerText;

        }
        /// <summary>
        /// Implementation of equal to compare two couples
        /// </summary>
        /// <param name="obj">the couple to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Couple comp = (Couple)obj;
            // we compare the names, this is the only way to make sure
            // it's the same. Won't work with spelling mistakes
            return comp.Man.FirstName == this.Man.FirstName 
                && comp.Man.LastName == this.Man.LastName
                && comp.Woman.FirstName == this.Woman.FirstName 
                && comp.Woman.LastName == this.Woman.LastName;
          
        }

        
        public override int GetHashCode()
        {
            string name = this.Man.FirstName + this.Man.LastName + this.Woman.FirstName + this.Woman.LastName +
                          this.Man.Country + this.Woman.Country;

            return name.GetHashCode();
        }
    }
    /// <summary>
    /// Holds information about a couple dancing a competition. E.g. the number in a competition
    /// </summary>
    public class Participant
    {
        public string Number { get; set; }
        public string Competition { get; set; }
        public bool CheckedIn { get; set; }
        public Couple Couple { get; set; }
    }
}
