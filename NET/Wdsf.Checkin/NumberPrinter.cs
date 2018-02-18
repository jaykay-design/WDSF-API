using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
using System.Windows.Xps;
using System.IO;

namespace WDSF_Checkin
{
    /// <summary>
    /// Class for printing back numbers.
    /// Part's of this code have been reused from other projects
    /// </summary>
    public class NumberPrinter
    {
        protected FixedDocument doc;

        private string _number;
        private string _competition;
        private string _couple;
        private string _pageDef;

        public FixedDocument Document
        {
            get { return doc; }
        }
        /// <summary>
        /// Page number if used 
        /// </summary>
        public bool PageNumbers
        { get; set; }
        /// <summary>
        /// Creates an empty document, the size is DIN A4 (european size)
        /// </summary>
        public NumberPrinter(string number, string couple, string competition, string pageDefFile)
        {
            doc = new FixedDocument();
            doc.DocumentPaginator.PageSize = new Size(96 * 21 / 2.54, 96 * 29.7 / 2.54);
            _number = number;
            _competition = competition;
            _couple = couple;
            _pageDef = pageDefFile;

            createPage();
        }

        /// <summary>
        /// Creates our page to print
        /// </summary>
        /// <returns></returns>
        public FixedPage createPage()
        {
            PageContent page = new PageContent();
            FixedPage fixedPage = new FixedPage();
            ((IAddChild)page).AddChild(fixedPage);
            doc.Pages.Add(page);

            fixedPage.Background = Brushes.White;
            // Read the page definitions
            StreamReader def = new StreamReader(_pageDef);
            while (!def.EndOfStream)
            {
                var line = def.ReadLine();
                if (!line.StartsWith("#"))
                {
                    var data = line.Split(';');
                    var text = data[4].Replace("<Number>", _number).Replace("<Couple>", _couple).Replace("<Competition>", _competition);
                    AddText(fixedPage, text, Double.Parse(data[0]), Double.Parse(data[1]), 
                        Int32.Parse(data[3]), Double.Parse(data[2]), 10.0, 
                        TextAlignment.Center, FontWeights.Normal, 0);
                }
            }
            def.Close();
            return fixedPage;
        }
        /// <summary>
        /// Text einer Seite hinzufügen
        /// </summary>
        /// <param name="Page">Die Seire</param>
        /// <param name="Text">Text</param>
        /// <param name="Top">in Cm</param>
        /// <param name="Left">in Cm</param>
        /// <param name="FontSize">in Punkten</param>
        /// <param name="Width"></param>
        /// <param name="alignment"></param>
        protected void AddText(FixedPage Page, string Text, double Top, double Left, int FontSize, double Width, TextAlignment alignment)
        {
            AddText(Page, Text, Top, Left, FontSize, Width, 3, alignment, FontWeights.Normal, 0);
        }
        /// <summary>
        /// Fügt einen Text hinzu mit der Möglichkeit eines Rahmnens
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Text"></param>
        /// <param name="Top"></param>
        /// <param name="Left"></param>
        /// <param name="FontSize"></param>
        /// <param name="Width"></param>
        /// <param name="alignment"></param>
        /// <param name="Weight"></param>
        /// <param name="Border"></param>
        protected void AddText(FixedPage Page, string Text, double Top, double Left, int FontSize, double Width, double Height, TextAlignment alignment, FontWeight Weight, int Border)
        {
            TextBlock text = new TextBlock();
            text.Text = Text;
            text.FontSize = FontSize;
            text.FontFamily = new FontFamily("Arial");
            text.TextAlignment = alignment;
            text.FontWeight = Weight;
            text.Width = Width * 96 / 2.54;
            text.Height = Height * 96 / 2.54;
            var width = text.ActualWidth;
            var boxwidth = Width * 96 / 2.54;
            var diff = (boxwidth - width) / 2;

            if (Border > 0)
            {
                text.Padding = new Thickness(4, 2, 2, 2);
            }

            Border b = new Border();
            b.BorderThickness = new Thickness(Border);
            b.BorderBrush = Brushes.Black;
            b.Child = text;
            FixedPage.SetLeft(b, 96 * Left / 2.54); // left margin
            FixedPage.SetTop(b, 96 / 2.54 * Top); // top margin
            width = text.ActualWidth;
            Page.Children.Add((UIElement)b);

        }
        /// <summary>
        /// Fügt einen Rahmen hinzu
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Top"></param>
        /// <param name="Left"></param>
        /// <param name="Heigh"></param>
        /// <param name="Width"></param>
        protected void AddBox(FixedPage Page, double Top, double Left, double Heigh, double Width)
        {
            System.Windows.Controls.Border b = new Border();
            b.BorderThickness = new Thickness(1);
            b.BorderBrush = Brushes.Black;
            b.Width = Width * 96 / 2.54;
            b.Height = Heigh * 96 / 2.54;
            FixedPage.SetLeft(b, 96 * Left / 2.54); // left margin
            FixedPage.SetTop(b, 96 / 2.54 * Top); // top margin
            Page.Children.Add((UIElement)b);
        }
        /// <summary>
        /// Fügt eine horizontale Linie hinzu
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Top"></param>
        /// <param name="Left"></param>
        /// <param name="Width"></param>
        protected void AddHorizontalLine(FixedPage Page, double Top, double Left, double Width)
        {
            System.Windows.Controls.Border b = new Border();
            b.BorderThickness = new Thickness(1);
            b.BorderBrush = Brushes.Black;
            b.Width = Width * 96 / 2.54;
            b.Height = 1;
            FixedPage.SetLeft(b, 96 * Left / 2.54); // left margin
            FixedPage.SetTop(b, 96 / 2.54 * Top); // top margin
            Page.Children.Add((UIElement)b);
        }
        /// <summary>
        /// Vertikale Linie
        /// </summary>
        /// <param name="Page"></param>
        /// <param name="Top"></param>
        /// <param name="Left"></param>
        /// <param name="Height"></param>
        protected void AddVerticalLine(FixedPage Page, double Top, double Left, double Height)
        {
            System.Windows.Controls.Border b = new Border();
            b.BorderThickness = new Thickness(1);
            b.BorderBrush = Brushes.Black;
            b.Height = Height * 96 / 2.54;
            b.Width = 1;
            FixedPage.SetLeft(b, 96 * Left / 2.54); // left margin
            FixedPage.SetTop(b, 96 / 2.54 * Top); // top margin
            Page.Children.Add((UIElement)b);
        }

        protected void AddText(FixedPage Page, string Text, double Top, double Left, int FontSize)
        {
            AddText(Page, Text, Top, Left, FontSize, 30, TextAlignment.Left);
        }
 
        /// <summary>
        /// Wird unmittelbar vor dem Druck aufgerufen und fügt beispielsweise Seitennummern hinzu
        /// </summary>
        protected void FinalizeDocument()
        {
            int count = 1;
            
            if (this.PageNumbers)
            {
                foreach (PageContent p in doc.Pages)
                {
                    // AddHorizontalLine(p.Child, 26.5, 1, 19);
                    AddText(p.Child, "Seite " + count + " von " + doc.Pages.Count, 27, 10, 12, 100, TextAlignment.Center);
                    count++;
                }
            }
        }

        /// <summary>
        /// Prints the document to the standard printer
        /// </summary>
        /// <param name="toFile">filename to store a copy of the document as file, set to null if not used</param>
        /// <param name="showPrintDlg">set to true if the standard windows printer dialog should come up</param>
        public void PrintDocument(string toFile, bool showPrintDlg)
        {
            FinalizeDocument();
            if (toFile != null)
            {
                System.IO.File.Delete(toFile);
                XpsDocument xpsd = new XpsDocument(toFile, System.IO.FileAccess.ReadWrite);
                XpsDocumentWriter xw = XpsDocument.CreateXpsDocumentWriter(xpsd);
                xw.Write(doc);
                xpsd.Close();
            }
            else
            {
                PrintDialog prndlg = new PrintDialog();
                if (showPrintDlg)
                    if (prndlg.ShowDialog().Value == false) return;
                prndlg.PrintDocument(doc.DocumentPaginator, "Check-In Number");
            }
        }
    }
}
