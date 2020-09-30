using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace Kino
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            pokazDane();
            Timer();
            media.Play();
        }

        List<Repertuar> rep = new List<Repertuar>();
        ObservableCollection<Sala> sala = new ObservableCollection<Sala>();
        ObservableCollection<Seans> seans = new ObservableCollection<Seans>();
        Random rand = new Random();
        ObservableCollection<Widz> widz = new ObservableCollection<Widz>();
        private void pokazDane()
        {
            rep.Add(new Repertuar() { Tytul = "Gladiator", CzasTrwania = "2 godz. 35min.", Wiek = 16, Gatunek = "Dramat historyczny" });
            rep.Add(new Repertuar() { Tytul = "Nietykalni", CzasTrwania = "1 godz. 52min.", Wiek = 16, Gatunek = "Dramat" });
            repertuarLV.ItemsSource = rep;

            sala.Add(new Sala() { NrSali = 1, IleMiejsc = 40 });
            sala.Add(new Sala() { NrSali = 2, IleMiejsc = 30 });
            sala1Miejsce.Content = "Ilośc miejsca w sali 1: 40";
            sala2Miejsce.Content = "Ilośc miejsca w sali 2: 30";

            seans.Add(new Seans() { NrSeansu = 1, Sala = sala[0], Repertuar = rep[0], Godzina = "15:00" });
            seans.Add(new Seans() { NrSeansu = 2, Sala = sala[1], Repertuar = rep[1], Godzina = "17:00" });
            seansLB.ItemsSource = seans;
            seansLV.ItemsSource = seans;

            widz.Add(new Widz() { NrWidza = rand.Next(0, 9999), Imie = "Adam", Nazwisko = "Nowak", Seans = seans[0] });
            widz.Add(new Widz() { NrWidza = rand.Next(0, 9999), Imie = "Jan", Nazwisko = "Kowalski", Seans = seans[1] });
            widzLV.ItemsSource = widz;
        }
        private void repertuarLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Repertuar rep = (Repertuar)repertuarLV.SelectedItem;
            poka.Content = "Film pt.: " + rep.Tytul + ", czas trwania: " + rep.CzasTrwania + ", od " + rep.Wiek + " lat, gatunek: " + rep.Gatunek;
        }

        private void widzLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Widz widz = (Widz)widzLV.SelectedItem;
            poka.Content = widz.Imie + " " + widz.Nazwisko + " \n"
                + "Rezerwacja na: " + widz.Seans.Repertuar.Tytul + ", o godzinie: " + widz.Seans.Godzina + ", sala nr: " + widz.Seans.Sala.NrSali;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Seans seans = (Seans)seansLB.SelectedItem;
            if (imieLBL.Text == string.Empty || nazwiskoLBL.Text == string.Empty || seansLB.SelectedIndex == -1)
            {
                MessageBox.Show("Musisz uzupełnić dane lub/oraz wybrać film.");
            }
            else
            {
                widz.Add(new Widz() { NrWidza = rand.Next(0, 9999), Imie = imieLBL.Text, Nazwisko = nazwiskoLBL.Text, Seans = seans });
                seans.Sala.IleMiejsc--;
                imieLBL.Text = "";
                nazwiskoLBL.Text = "";
            }
            sala1Miejsce.Content = "Ilośc miejsca w sali 1: " + sala[0].IleMiejsc;
            sala2Miejsce.Content = "Ilośc miejsca w sali 2: " + sala[1].IleMiejsc;
        }

        private void playBTN_Click(object sender, RoutedEventArgs e)
        {
            media.Play();
        }

        private void stopBTN_Click(object sender, RoutedEventArgs e)
        {
            media.Stop();
        }

        private void pauseBTN_Click(object sender, RoutedEventArgs e)
        {
            media.Pause();
        }
        private void Timer()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        void timer_Tick(object sender, EventArgs e)
        {
            if (media.Source != null)
            {
                if (media.NaturalDuration.HasTimeSpan)
                    statusLBL.Content = String.Format("{0} / {1}", media.Position.ToString(@"mm\:ss"), media.NaturalDuration.TimeSpan.ToString(@"mm\:ss"));
            }
            else
                statusLBL.Content = "No file selected...";
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            media.Stop();
        }
        private void media_MediaOpened(object sender, RoutedEventArgs e)
        {
            media.Pause();
        }
        private void seansLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Seans seans = (Seans)seansLB.SelectedItem;
            if ( seans.Repertuar.Tytul == "Gladiator")
            {
                media.Source = new Uri("http://hubblesource.stsci.edu/sources/video/clips/details/images/hst_1.mpg");
            }
            if (seans.Repertuar.Tytul == "Nietykalni")
            {
                media.Source = new Uri("http://hubblesource.stsci.edu/sources/video/clips/details/images/grb_1.mpg");
            }
        }
    }
}