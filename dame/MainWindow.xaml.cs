using System;
using System.Collections;
using System.Collections.Generic;
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

namespace dame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int kvadrat_velikost = 55;
        int N = 8;
        Image image = new Image();
        bool zarek = false;
        List<Image> slike = new List<Image>();
        List<int> zarek_vrstice = new List<int>();
        List<int> zarek_stolpci = new List<int>();
        List<int> zarek_vrstice2 = new List<int>();
        List<int> zarek_stolpci2= new List<int>();
        List<List<Image>> zacasna = new List<List<Image>>();
        List<int> ohlajanje_vrstice = new List<int>();
        List<int> ohlajanje_stolpci = new List<int>();
        List<int> ohlajanje_vrstice2 = new List<int>();
        List<int> ohlajanje_stolpci2 = new List<int>();
        bool vn_iz_zanke = false;
        bool konec_zarkov = false;
        int zarek_povrsti = -1;
        List<List<Image>> mnozica_slik = new List<List<Image>>();
        List<int> ocene = new List<int>();
        int najboljsi_i;
        int najboljsi_j;
        int min_hevristika = 10000;
        int hevristika_od_S;
        List<int> vsi_i = new List<int>();
        List<int> vsi_j = new List<int>();
        Random rand_verj = new Random();
        public MainWindow()
        {
            InitializeComponent();

            init_miza();
            init_slika();
            postavi_kraljice_nakljucno();
            //postavi_kraljice();
        }
        Random r1 = new Random();
        private void postavi_kraljice_nakljucno()
        {
            //Random r1= new Random();
            int i1;
            int i = 0;
            
            foreach (var slika in slike)
            {
                i1 = r1.Next(0, N); //for ints
                Grid.SetRow(slika, i1);
                ohlajanje_vrstice.Add(i1);
                i1 = r1.Next(0, N); //for ints
                zarek_vrstice.Add(i1);
                zarek_stolpci.Add(i);
                ohlajanje_stolpci.Add(i);
                Grid.SetColumn(slika, i);
                if (!zarek) { 
                    LayoutRoot.Children.Add(slika);
                }
                i++;
            }
        }
       
        private int hevristika()
        {
            int sum = 0;
            for (int k = 0; k < N; k++)
            {
                int trenutna_stolpec = Grid.GetColumn(slike[k]);
                int trenutna_vrstica = Grid.GetRow(slike[k]);
                int del_sum = 0;
                //preverim vse kraljice
                for (int i = 0; i < N; i++)
                {
                    // Dobim od naslednje kraljice vrstico
                    int naslednja_vrstica = Grid.GetRow(slike[i]);

                    // Preverim, če je v isti vrstici ali stolpcu
                    if (naslednja_vrstica == trenutna_vrstica || // ista vrstica
                    naslednja_vrstica == trenutna_vrstica - (trenutna_stolpec - i) || // ista diagonala
                    naslednja_vrstica == trenutna_vrstica + (trenutna_stolpec - i)) // ista diagonala
                    {   
                        del_sum++;
                    }
                }
                //MessageBox.Show("SUM: " + (del_sum-1));
                sum += (del_sum - 1);//ker ta steje tudi sebe
            }
            //MessageBox.Show("CELOTNA: " + sum);
            return sum;
    }
        private int hevristika2(List<Image> slike_zbrane)
        {
            int sum = 0;
            for (int k = 0; k < slike_zbrane.Count(); k++)
            {
                int trenutna_stolpec = Grid.GetColumn(slike_zbrane[k]);
                int trenutna_vrstica = Grid.GetRow(slike_zbrane[k]);
                int del_sum = 0;
                //preverim vse kraljice
                for (int i = 0; i < N; i++)
                {
                    // Dobim od naslednje kraljice vrstico
                    int naslednja_vrstica = Grid.GetRow(slike_zbrane[i]);

                    // Preverim, če je v isti vrstici ali stolpcu
                    if (naslednja_vrstica == trenutna_vrstica || // ista vrstica
                    naslednja_vrstica == trenutna_vrstica - (trenutna_stolpec - i) || // ista diagonala
                    naslednja_vrstica == trenutna_vrstica + (trenutna_stolpec - i)) // ista diagonala
                    {
                        del_sum++;
                    }
                }
                //MessageBox.Show("SUM: " + (del_sum-1));
                sum += (del_sum - 1);//ker ta steje tudi sebe
            }
            //MessageBox.Show("CELOTNA: " + sum);
            return sum;
        }
       
        private void zbrisi()
        {
            LayoutRoot.Children.Clear();
        }
        private void init_slika()
        {
            slike.Clear();
            for(int i = 0; i < N; i++)
            {
                Image a = new Image();
                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri("D:/fax/3letnik/UI/dame/sss.png");
                b.EndInit();
                a.Source = b;
                a.Width = 50;
                a.Height = 50;
                slike.Add(a);
            }
        }

        private void init_miza()
        {
            //pripravim vrstice in stolpce
            GridLengthConverter myGridLengthConverter = new GridLengthConverter();
            GridLength side = (GridLength)myGridLengthConverter.ConvertFromString("Auto");
            for (int i = 0; i < N; i++)
            {
                LayoutRoot.ColumnDefinitions.Add(new ColumnDefinition());
                LayoutRoot.ColumnDefinitions[i].Width = side;
                LayoutRoot.RowDefinitions.Add(new RowDefinition());
                LayoutRoot.RowDefinitions[i].Height = side;
            }
            Rectangle[,] kvadrat = new Rectangle[N, N];//naredim 2D array kvadratkov
            for (int row = 0; row < N; row++)//ustrezno barvam in dodajam v grid
                for (int col = 0; col < N; col++)
                {
                    kvadrat[row, col] = new Rectangle();
                    kvadrat[row, col].Height = kvadrat_velikost;
                    kvadrat[row, col].Width = kvadrat_velikost;
                    Grid.SetColumn(kvadrat[row, col], col);
                    Grid.SetRow(kvadrat[row, col], row);
                    if ((row + col) % 2 == 0)
                    {
                        kvadrat[row, col].Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 255, 255));
                    }
                    else
                    {
                        kvadrat[row, col].Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 0, 0));
                    }
                    LayoutRoot.Children.Add(kvadrat[row, col]);
                }   
        }
//ZACETEK ALGORITMOV
        private void button_Click(object sender, RoutedEventArgs e)
        {
            N = Convert.ToInt32(textBox.Text);
            zbrisi();
            init_miza();
            init_slika();

            if (radioButton.IsChecked==true) {
                postavi_kraljice_nakljucno();
                int st_ponovitev = Convert.ToInt32(textBox1.Text);
                Dispatcher.Invoke((Action)(() => { }), DispatcherPriority.Render);
                System.Threading.Thread.Sleep(500);
                vzpenjanje_na_hrib(st_ponovitev);
            }
            else if (radioButton1.IsChecked == true)
            {
                ohlajanje_vrstice.Clear();
                vn_iz_zanke = false;
                postavi_kraljice_nakljucno();
                double zacetna_temp = Convert.ToDouble(textBox2.Text);
                double sprememba_temp = Convert.ToDouble(textBox3.Text);
                simultano_ohlajanje(zacetna_temp,sprememba_temp);
            }
            else if (radioButton2.IsChecked == true)
            {
                konec_zarkov = false;
                zarek = true;
                int st_stanj = Convert.ToInt32(textBox4.Text);
                lokalni_zarek(st_stanj);
                zarek = false;
            }
            else if (radioButton3.IsChecked == true)
            {
                zbrisi();
                init_miza();
                init_slika();
                genetski_algoritem();
            }
        }
        private bool vzpenjanje_na_hrib(int st_ponovitev) {
            hevristika_od_S=hevristika();
            if (hevristika_od_S == 0) {
                MessageBox.Show("USPEŠNO KONČANO");
                return true;
            }
            razvijanje();
            if (min_hevristika >= hevristika_od_S){
                if ((min_hevristika == hevristika_od_S) && (st_ponovitev >= 1))//moznosti se odstevajo
                {
                    st_ponovitev--;
                    Grid.SetRow(slike[najboljsi_i], najboljsi_j);//dam na novo vrstico
                    vzpenjanje_na_hrib(st_ponovitev);
                }
                else {//ni vec moznosti, koncam in ne gre
                    MessageBox.Show("NE GRE");
                    return false;
                }
            }
            else {
                Grid.SetRow(slike[najboljsi_i], najboljsi_j);//dam na novo vrstico
                vzpenjanje_na_hrib(st_ponovitev);
            }
            return false;
        }
        private void razvijanje() {
            //razvijanje
            int original = 0;
            vsi_i.Clear();
            vsi_j.Clear();
            najboljsi_i = 0;
            najboljsi_j = 0;
            min_hevristika = 10000;
            for (int i = 0; i < N; i++)
            {//vsak stolpec - vzamem eno kraljico
                original = Grid.GetRow(slike[i]);
                for (int j = 0; j < N; j++)
                {//uvrstim v vsako vrstico razn tam, kjer je ze
                    if (original != j)
                    {//ce ni na tem mestu
                        Grid.SetRow(slike[i], j);//dam na novo vrstico
                        //hevristika
                        int trenutna_hevristika = hevristika();
                        if (min_hevristika >= trenutna_hevristika)
                        {
                            if (min_hevristika >= trenutna_hevristika)
                            {
                                min_hevristika = trenutna_hevristika;
                                vsi_i.Add(i);
                                vsi_j.Add(j);
                            }
                            else {
                                min_hevristika = trenutna_hevristika;
                                vsi_i.Clear();
                                vsi_j.Clear();
                                vsi_i.Add(i);
                                vsi_j.Add(j);
                            }
                            
                        }
                        Grid.SetRow(slike[i], original);
                    }
                }
            }
            //konec razvijanja
            //izberemo random potezo od enakih
            Random r2 = new Random();
            int i2 = r2.Next(0, vsi_i.Count());
            najboljsi_i = vsi_i[i2];
            najboljsi_j = vsi_j[i2];
        }

        private void update_ohlajanje_kraljice()
        {
            zbrisi();
            init_miza();
            init_slika();
            for (int i = 0; i < slike.Count(); i++)
            {
                Grid.SetRow(slike[i], ohlajanje_vrstice[i]);
                Grid.SetColumn(slike[i], i);
                LayoutRoot.Children.Add(slike[i]);
            }
            zarek_vrstice.Clear();
        }
        private int ohlajanje_hevristika() {
            int sum = 0;
            for (int k = 0; k < N; k++)
            {
                int trenutna_stolpec = k;
                int trenutna_vrstica = ohlajanje_vrstice[k];
                int del_sum = 0;
                //preverim vse kraljice
                for (int i = 0; i < N; i++)
                {
                    // Dobim od naslednje kraljice vrstico
                    int naslednja_vrstica = ohlajanje_vrstice[i];

                    // Preverim, če je v isti vrstici ali stolpcu
                    if (naslednja_vrstica == trenutna_vrstica || // ista vrstica
                    naslednja_vrstica == trenutna_vrstica - (trenutna_stolpec - i) || // ista diagonala
                    naslednja_vrstica == trenutna_vrstica + (trenutna_stolpec - i)) // ista diagonala
                    {
                        del_sum++;
                    }
                }
                sum += (del_sum - 1);//ker ta steje tudi sebe
            }
            return sum;
        }
       
        private void simultano_ohlajanje(double zacetna_temp, double sprememba_temp) {
            while (true) {
                if (vn_iz_zanke) { break; }
                if (zacetna_temp <= 0)
                {
                    vn_iz_zanke = true;
                    MessageBox.Show("T=0");
                }
                int stara_hevristika = ohlajanje_hevristika();
                //nakljucni naslednik
                int kraljica = rand_verj.Next(0, N);//zberem nakljucno kraljico
                bool izbiram = true;
                int vrstica = 0;
                while (izbiram) {//dokler nisem izbral ustrezno vrstico
                    vrstica = rand_verj.Next(0, N);//zberem nakljucno vrstico
                    if (vrstica != ohlajanje_vrstice[kraljica])//da jo ne dam na isto mesto
                    {
                        izbiram = false; // sem zbral novo potezo
                    }
                }
                int stara_vrstica = ohlajanje_vrstice[kraljica]; //zabelezim kje sm mel prej kraljico
                ohlajanje_vrstice[kraljica] = vrstica;
                //nova hevristika
                int nova_hevristika=ohlajanje_hevristika();
                if (nova_hevristika == 0)//ce je trenutna hevristika 0 je konec
                {
                    update_ohlajanje_kraljice();//osvezim zakljucen prikaz
                    MessageBox.Show("KONEC - hevristika je 0");
                    vn_iz_zanke = true;
                }
                int sprememba_hevristike = (nova_hevristika - stara_hevristika);//izracunam spremembo
                if (sprememba_hevristike < 0)
                {
               //imam spremembo ze narejeno, pustim in grem na naslednji krog
                }
                else {
                    //z verjetnostjo
                    double intervalcek=rand_verj.Next(0, 100);
                    intervalcek=intervalcek / 100;//dobim v intervalu od 0-1
                    double exponent = (-sprememba_temp / zacetna_temp);
                    double verjetnost = Math.Pow(Math.E, exponent);
                    //sedaj posodobim S<-X
                    if (intervalcek < verjetnost)
                    {
                        //imam spremembo ze narejeno, pustim in grem na naslednji krog
                    }
                    else {
                        //povrnem na staro pozicijo
                        ohlajanje_vrstice[kraljica] = stara_vrstica;
                    }
                }
                zacetna_temp = zacetna_temp - sprememba_temp; //T<-T-spr.T
            }                                                          //nov krog
        }


        int a = 0;

        /* te dve funkciji ce bi zelel prikazovati sproti*/
        private void postavi_kraljice(List<Image> slike_zbrane)
        {
            zbrisi();
            init_miza();
            init_slika();
            for (int i = 0; i < slike_zbrane.Count(); i++)
            {
                zarek_povrsti++;
                if (zarek_povrsti >= zarek_vrstice.Count()) { break; }
                Grid.SetRow(slike_zbrane[i], zarek_vrstice[zarek_povrsti]);
                Grid.SetColumn(slike_zbrane[i], zarek_stolpci[zarek_povrsti]);
                LayoutRoot.Children.Add(slike_zbrane[i]);
                
            }
            //Dispatcher.Invoke((Action)(() => { }), DispatcherPriority.Render);
            //System.Threading.Thread.Sleep(50);
        }
        private void update_kraljice(List<Image> slike_zbrane)
        {
            zbrisi();
            init_miza();
            init_slika();
            for (int i = 0; i < slike_zbrane.Count(); i++)
            {
                zarek_povrsti++;
                if (zarek_povrsti >= zarek_vrstice.Count()) { break; }
                Grid.SetRow(slike_zbrane[i], zarek_vrstice[zarek_povrsti]);
                Grid.SetColumn(slike_zbrane[i], zarek_stolpci[zarek_povrsti]);
                LayoutRoot.Children.Add(slike_zbrane[i]);
                Dispatcher.Invoke((Action)(() => { }), DispatcherPriority.Render);
                System.Threading.Thread.Sleep(10);
            }

        }
        private void lokalni_zarek(int st_stanj) {
//nakljucna mnozice k zakljucenih stanj
            zarek_stolpci.Clear();
            zarek_vrstice.Clear();
            mnozica_slik.Clear();
            for (int i = 0; i < st_stanj; i++) {
                slike.Clear();
                init_slika();
                postavi_kraljice_nakljucno();
                mnozica_slik.Add(slike);
            }
            //zanka
            int zaloop = 0;
            while (true)
            {
//preverjanje, če ni bilo uspešno
                zaloop++;
                if (zaloop > 100)
                {//nekje zaustavim, če ni bilo uspešno
                    MessageBox.Show("zaloopal");
                    konec_zarkov = true;
                    zarek_povrsti = -1;
                    mnozica_slik.Clear();
                    a = 0;
                    break;
                }
 //izracunamo oceno h vsem stanjem v P
                zarek_povrsti = -1;
                SortedDictionary<int, int> urejeno = new SortedDictionary<int, int>();
                if (a >= st_stanj)
                {
                    for (int i = 0; i < mnozica_slik.Count() / N; i++)
                    {
                        urejeno.Add(i, hevristika3());//racunam hevristiko
                    }
                }
                else {
                    for (int i = 0; i < mnozica_slik.Count(); i++)//prvo rundo imam malo drugace
                    {
                        if (a < st_stanj)
                        {
                            urejeno.Add(i, hevristika3());//racunam hevristiko
                        }
                        a++;
                    }
                }
//uredimo po ocenah
                var sortedElements = urejeno.OrderBy(kvp => kvp.Value);
//ce vsebuje 0 prekinemo
                foreach (KeyValuePair<int, int> p in sortedElements)
                {
                    if (p.Value == 0)//ce vsebuje stanje n z oceno 0
                    {
                        //poskrbim za prikaz rešitve
                        zarek_povrsti = (p.Key * N) - 1;
                        zbrisi();
                        init_miza();
                        init_slika();
                        for (int i = 0; i < N; i++)
                        {
                            zarek_povrsti++;
                            if (zarek_povrsti >= zarek_vrstice.Count()) { break; }
                            Grid.SetRow(slike[i], zarek_vrstice[zarek_povrsti]);
                            Grid.SetColumn(slike[i], zarek_stolpci[zarek_povrsti]);
                            LayoutRoot.Children.Add(slike[i]);
                        }
                        MessageBox.Show("KONEC");
                        konec_zarkov = true;
                        urejeno.Clear();
                        break;
                    }
                }
                if (konec_zarkov) {//skocim ven in ponastavim, če je konec
                    zarek_povrsti = -1;
                    mnozica_slik.Clear();
                    a = 0;
                    break;
                }
                int stevec_najboljsih_stanj = 0;
                zarek_povrsti = -1;
                zarek_vrstice2.Clear();
                zarek_stolpci2.Clear();
//vzamem k najboljših stanj
                foreach (KeyValuePair<int, int> p in sortedElements)
                 {
                     if (stevec_najboljsih_stanj == st_stanj)//vzamem le najbolsih k stanj
                     {
                        break;
                     }
//za vsako vzeto stanje razvijem naslednja
                    zarek_povrsti = (p.Key * N) - 1;
                    postavi_kraljice(mnozica_slik[p.Key]);
                    razvijanje_zarek();
                    stevec_najboljsih_stanj++;
                 }
                //nastavim stvari za naslednji loop
                mnozica_slik.Clear();
                zarek_vrstice.Clear();
                zarek_stolpci.Clear();
                zarek_povrsti = -1;
                foreach (var z in zacasna) {
                    mnozica_slik.Add(z);
                }
                foreach (var z in zarek_vrstice2)
                {
                    zarek_vrstice.Add(z);
                }
                foreach (var z in zarek_stolpci2)
                {
                    zarek_stolpci.Add(z);
                }
                zarek_vrstice2.Clear();
                zarek_stolpci2.Clear();
            }
        }

        private void razvijanje_zarek()
        {
            //razvijanje
            int original = 0;
            for (int i = 0; i < N; i++)//vsak stolpec - vzamem eno kraljico
            {
                original = Grid.GetRow(slike[i]);
                for (int j = 0; j < N; j++)//uvrstim v vsako vrstico razn tam, kjer je ze
                {
                    if (original != j)
                    {//ce ni na tem mestu
                        Grid.SetRow(slike[i], j);//dam na novo vrstico
                        for (int p= 0; p < N; p++) {
                            zarek_vrstice2.Add(Grid.GetRow(slike[p]));//dodam vrstico
                            zarek_stolpci2.Add(Grid.GetColumn(slike[p]));//dodam stolpec
                            zacasna.Add(slike);//dodam v novo moznost
                        }
                    }
                        Grid.SetRow(slike[i], original);//ponastavim nazaj
                }
            }
        }
        private int hevristika3() {
            int prva = 0;
            int sum = 0;
            for (int k = 0; k < N; k++)//grem cez N kraljic
            {
                zarek_povrsti++;
                if (k == 0)
                {
                    prva = zarek_povrsti;//zacetno stanje
                }
                if (zarek_povrsti >= zarek_vrstice.Count()) {
                    sum = 10000;//za vsak primer, robni primer
                    break; }
                int trenutna_stolpec = zarek_stolpci[zarek_povrsti];
                int trenutna_vrstica = zarek_vrstice[zarek_povrsti];
                int del_sum = 0;
                //preverim vse ostale kraljice s trenutno
                for (int i = 0; i <  N; i++)
                {
                    // Dobim od naslednje kraljice vrstico
                        int naslednja_vrstica = zarek_vrstice[prva+i];

                        // Preverim, če je v isti vrstici ali stolpcu
                        if (naslednja_vrstica == trenutna_vrstica || // ista vrstica
                        naslednja_vrstica == trenutna_vrstica - (trenutna_stolpec - i) || // ista diagonala
                        naslednja_vrstica == trenutna_vrstica + (trenutna_stolpec - i)) // ista diagonala
                        {
                            del_sum++;
                        }
                }
                sum += (del_sum - 1);//ker ta steje tudi sebe
            }
            return sum;
        }

        /// genetski algoritem
        int pop = 0; //st. kromosomov
        int odstotek_elitizma = 0;
        double elitizem = 0;
        double verj_krizenja = 0;
        double verj_mutacije = 0;
        int st_generacij = 0; //kolikokrat se bo loop izvedel
        List<List<int>> kromosomi = new List<List<int>>();
        List<List<int>> urejeni_kromosomi = new List<List<int>>();
        List<List<int>> vmesna = new List<List<int>>();
        List<List<int>> Q = new List<List<int>>();
        SortedDictionary<int, int> urejeno;
        private void genetski_algoritem() {
            kromosomi.Clear();
            pop = Convert.ToInt32(textBox5.Text);
            odstotek_elitizma = Convert.ToInt32(textBox6.Text);
            elitizem = ((pop * odstotek_elitizma) / 100);
            if (elitizem % 2 != 0) {//da je sodo stevilo
                elitizem += 1;
            }
            verj_krizenja = Convert.ToDouble(textBox7.Text);
            verj_mutacije = Convert.ToDouble(textBox8.Text);
            st_generacij = Convert.ToInt32(textBox9.Text);

            //nakljucna mnozica kromosomov
            for (int i = 0; i < pop; i++) {
               kromosomi.Add(nakljucni_kromosomi());
            }
            int izvajanje = 0;
            bool konec = false;
            //while loop
            while (izvajanje<st_generacij && konec==false)//koliko generacij zaloopam
            {
                urejeno = new SortedDictionary<int, int>();
                //izracunamo ocene vseh kromosomov
                for (int i = 0; i < pop; i++)
                {
                    urejeno.Add(i, genetska_hevristika(kromosomi[i]));
                }
                //uredimo po ocenah
                var sortedElements = urejeno.OrderBy(kvp => kvp.Value);
                urejeni_kromosomi.Clear();
                //ce vsebuje 0 prekinemo
                foreach (KeyValuePair<int, int> p in sortedElements)
                {
                    if (p.Value == 0)//ce vsebuje stanje n z oceno 0
                    {
                        konec = true;
                        MessageBox.Show("KONEC");
                        for (int i = 0; i < N; i++)//narisem
                        {
                            Grid.SetRow(slike[i], kromosomi[p.Key][i]);
                            Grid.SetColumn(slike[i], i);
                            LayoutRoot.Children.Add(slike[i]);
                        }
                    }
                    urejeni_kromosomi.Add(kromosomi[p.Key]);
                    /*
                    Dispatcher.Invoke((Action)(() => { }), DispatcherPriority.Render);
                    System.Threading.Thread.Sleep(1000);*/
                }
                if (konec) { break; }
                //naredim prazen Q
                Q.Clear();
//SELEKCIJA- ELITIZEM (izberem najbolje ocenjene in jih dam kar v novo generacijo
                for (int i = 0; i < elitizem; i++)
                {
                    Q.Add(urejeni_kromosomi[0]);
                    urejeni_kromosomi.RemoveAt(0);
                }

                for (int i = 0; i < (pop / 2); i++) {
                    if (urejeni_kromosomi.Count() <2) {//ce jih ni vec. Lahko pa jih je najvec k/2
                        break;
                    }
                    //C1, C2 ← s selekcijo izberi dva kromosoma-starša
                    List<int> C1 = urejeni_kromosomi[0];
                    List<int> C2 = urejeni_kromosomi[1];
                    urejeni_kromosomi.RemoveAt(0);
                    urejeni_kromosomi.RemoveAt(0);
//KRIZENJE
                    double intervalcek = rand_verj.Next(0, 100);
                    intervalcek = intervalcek / 100;//dobim v intervalu od 0-1
                    if (intervalcek < verj_krizenja)
                    {
                        //dobimo 2 potomca z izmenjavo genskega materiala
                        krizaj(C1,C2);//krizaj
                        C1 = vmesna[0];
                        C2 = vmesna[1];
                        vmesna.Clear();
                    }
                    else {
                        //napredujeta kar oba starsa
                    }
                    intervalcek = rand_verj.Next(0, 100);
                    intervalcek = intervalcek / 100;//dobim v intervalu od 0-1
                    if (intervalcek < verj_mutacije)
                    {
                        C1=mutiraj(C1);//mutiraj
                        C2=mutiraj(C2);//mutiraj
                    }
                    Q.Add(C1);
                    Q.Add(C2);
                }
                //kromosomi <-- Q
                kromosomi.Clear();
                for (int l = 0; l < Q.Count(); l++) {
                    kromosomi.Add(Q[l]);
                }
                izvajanje++;
            }
            if (izvajanje >= st_generacij) {
                MessageBox.Show("NISEM NAŠEL REŠITVE");
            }
        }
        private List<int> mutiraj(List<int> C) {
            int st = rand_verj.Next(0, N);
            bool iscem = true;
            while (iscem) {
                int vr = rand_verj.Next(0, N);
                if (vr != C[st]) {
                    C[st] = vr;
                    iscem = false;
                }
            }
            return C;
        }
        private void krizaj(List<int> C1, List<int> C2) {
            //izberem 2 polozaja in zamenjamo vse gene med njima
            int t1 = rand_verj.Next(0, N);
            int t2 = rand_verj.Next(0, N);
            // zamenjam cele stolpce - ena verjanta od dvotockovnega krizanja
            List<int> vmesn = new List<int>();
            for (int i = 0; i < C1.Count(); i++) {
                vmesn.Add(C1[i]);
            }
            if (t1 <= t2)
            {
                for (int i = t1; i <= t2; i++) {
                    C1[i] = C2[i];
                    C2[i] = vmesn[i];
                }
            }
            else {
                for (int i = t1; i < N; i++)//do konca
                {
                    C1[i] = C2[i];
                    C2[i] = vmesn[i];
                }
                for (int i = 0; i <= t2; i++) { // od zacetka do t2
                    C1[i] = C2[i];
                    C2[i] = vmesn[i];
                }
            }
            vmesna.Add(C1);
            vmesna.Add(C2);
        }
        private List<int> nakljucni_kromosomi()
        {
            List<int> vrstice = new List<int>();
            int i1;
            int i = 0;
            for(int k=0;k< N; k++) {
                i1 = r1.Next(0, N); //for ints
                vrstice.Add(i1);
                i++;
            }
            return vrstice;
        }
        private int genetska_hevristika(List<int>kromosom)
        {
            int sum = 0;
            for (int k = 0; k < N; k++)
            {
                int trenutna_stolpec = k;
                int trenutna_vrstica = kromosom[k];
                int del_sum = 0;
                //preverim vse kraljice
                for (int i = 0; i < N; i++)
                {
                    // Dobim od naslednje kraljice vrstico
                    int naslednja_vrstica = kromosom[i];

                    // Preverim, če je v isti vrstici ali stolpcu
                    if (naslednja_vrstica == trenutna_vrstica || // ista vrstica
                    naslednja_vrstica == trenutna_vrstica - (trenutna_stolpec - i) || // ista diagonala
                    naslednja_vrstica == trenutna_vrstica + (trenutna_stolpec - i)) // ista diagonala
                    {
                        del_sum++;
                    }
                }
                sum += (del_sum - 1);//ker ta steje tudi sebe
            }
            return sum;
        }
    }
}

