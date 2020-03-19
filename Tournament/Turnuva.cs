using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tournament
{
    class Turnuva
    {
        public struct eş //eş leri tutan struct
        {
            private Takım t1;
            private Takım t2;
            private int tur;

            public eş(Takım t1, Takım t2, int tur)
            {
                this.t1 = t1;
                this.t2 = t2;
                this.tur = tur;
            }

            public Takım T1 { get => t1; }
            public Takım T2 { get => t2; }
            public int Tur { get => tur; }
        }


        private List<eş> eşleşmeler = new List<eş>();
        private Takım[] takımlar;
        private Takım[][][] Gruplar;
        private Takım[][][] Finalistler;

        private Kura platini;//Eski uefa başkanı
        private int takımSayısı;
        private Takım kazanan;

        public Takım[] Takımlar { get => takımlar; }
        public Takım Kazanan { get => kazanan; }

        public Turnuva(int takımSay, int seed)//Consturctor
        {
            takımlar = new Takım[takımSay];//istenilen takımsayısı boyutunda Takım dizisi oluşturur
            takımSayısı = takımSay;
            platini = new Kura(seed);//Platinin kura çekmesi için gerek seed enjekte edilir
            TurnuvaBaslat(this);//Oluşturlan instance classın statik methodu na gönderilerek işlemler başlar
        }
        private static void TurnuvaBaslat(Turnuva t)
        {
            //Random olarak performans seviyeleri üretilir
            int[] tempDizi = new int[t.takımSayısı];
            for (int i = 0; i < t.takımSayısı; i++)
            {
                tempDizi[i] = Randomizer.randomSayi(35, 100);
            }
            Array.Sort(tempDizi);
            Array.Reverse(tempDizi);//Büyükten küçüğe doğru sıralaması yapılır

            int grupBuyuklugu = 8;//her gruptaki takım sayısı 8
            for (int i = 0; i < t.takımSayısı / grupBuyuklugu; i++)//Oluşturlan performans seviyesi dizisinden değerleri alarak seribaşları oluşturulur
            {
                t.takımlar[i] = new seribaşıTakım(i + 1, tempDizi[i]);
            }
            for (int i = t.takımSayısı / grupBuyuklugu; i < t.takımSayısı; i++)//Ardından aynı şekilde diğer takımlar oluşturulur
            {
                t.takımlar[i] = new normalTakım(i + 1, tempDizi[i]);
            }
            //GRUPLAR
            int turSayisi = (int)Math.Log(grupBuyuklugu, 2) + 1;//8 kişilik bir grubun sonuçlanabilmesi için gereken tur sayısı

            Takım[][][] Gruplar = new Takım[turSayisi][][];//[Tur][Hangi Grup][Kim] dizisinin oluşturulduğu yer
            for (int j = 0; j < turSayisi; j++)
            {
                Gruplar[j] = new Takım[t.takımSayısı / grupBuyuklugu][];
                for (int i = 0; i < t.takımSayısı / grupBuyuklugu; i++)
                    Gruplar[j][i] = new Takım[grupBuyuklugu];
            }
            int sayac = 0;
            for (int j = 0; j < grupBuyuklugu; j++)
            {
                for (int i = 0; i < t.takımSayısı / grupBuyuklugu; i++)
                {
                    Gruplar[0][i][j] = t.takımlar[sayac];//Düzenli bir şekilde her gruba bir seribaşı ve yedi normal takım gelicek şekilde dağıtılır
                    sayac++;
                }
            }

            for (int i = 0; i < t.takımSayısı / grupBuyuklugu; i++) t.platini.kuraCek(Gruplar[0][i], grupBuyuklugu);//gruplar kendi içinde karıştırılarak eşleşmeler belirlenir

            int flag = 0;
            Takım t1 = null, t2 = null;
            for (int i = 0; i < turSayisi - 1; i++)
            {
                for (int k = 0; k < t.takımSayısı / grupBuyuklugu; k++)
                    for (int j = 0; j < grupBuyuklugu; j++)
                    {
                        Gruplar[i + 1][k][j] = (Takım)Gruplar[i][k][j].Clone();//Bir önceki turun kazananları bir sonraki turun oyuncuları olacağı için sürekli olarak kopyalanarak ilerlenir
                    }//Aynı zamanda shallow copy oluşturularak tur tur görünüm hissi verilir

                for (int k = 0; k < t.takımSayısı / grupBuyuklugu; k++)
                    for (int j = 0; j < grupBuyuklugu; j++)
                    {
                        if (!Gruplar[i + 1][k][j].takım_maglubiyet && flag == 0) { t1 = Gruplar[i + 1][k][j]; flag++; continue; }//Mağlubiyeti bulunmayan ilk takım seçilir ve ikinci takım için continue lanır
                        if (!Gruplar[i + 1][k][j].takım_maglubiyet && flag == 1) { t2 = Gruplar[i + 1][k][j]; flag--; }
                        if (t1 != null && t2 != null)//iki takımda bulunduktan sonra
                        {
                            eş eşleşme = new eş(t1, t2, i);// eş oluşturulup
                            t.eşleşmeler.Add(eşleşme);//eş dizisine atılır
                            while (!Takım.sonuc(t1, t2)) ;//oyun oynatılır ve false dönüp beraber olduğu sürece oyun tekrarlanır
                            t1 = null;
                            t2 = null;
                        }
                    }
            }
            //Her gruptan kalan son takımlar ortak tek bir gruba toplanır
            int finalTurSay = (int)Math.Log(t.takımSayısı / grupBuyuklugu, 2) + 1;//Gerçekleşicek final tur sayısı
            Takım[][][] Finalistler = new Takım[finalTurSay][][];
            for (int j = 0; j < finalTurSay; j++)
            {
                Finalistler[j] = new Takım[1][];
                Finalistler[j][0] = new Takım[t.takımSayısı / grupBuyuklugu];
            }

            for (int i = 0; i < t.takımSayısı / grupBuyuklugu; i++)
            {
                for (int k = 0; k < grupBuyuklugu; k++)
                {
                    if (!Gruplar[turSayisi - 1][i][k].takım_maglubiyet) Finalistler[0][0][i] = (Takım)Gruplar[turSayisi - 1][i][k].Clone();
                }
            }
            t1 = null;
            t2 = null;
            //Üstteki mantıkla tekrar oynatılarak mağlubiyeti false olan tek takım şampiyon olarak belirlenir
            for (int i = 0; i < finalTurSay - 1; i++)
            {
                for (int j = 0; j < t.takımSayısı / grupBuyuklugu; j++)
                {
                    Finalistler[i + 1][0][j] = (Takım)Finalistler[i][0][j].Clone();
                }

                for (int j = 0; j < t.takımSayısı / grupBuyuklugu; j++)
                {
                    if (!Finalistler[i + 1][0][j].takım_maglubiyet && flag == 0) { t1 = Finalistler[i + 1][0][j]; flag++; continue; }
                    if (!Finalistler[i + 1][0][j].takım_maglubiyet && flag == 1) { t2 = Finalistler[i + 1][0][j]; flag--; }
                    if (t1 != null && t2 != null)
                    {
                        eş eşleşme = new eş(t1, t2, i + 3);
                        t.eşleşmeler.Add(eşleşme);
                        while (!Takım.sonuc(t1, t2)) ;
                        t1 = null;
                        t2 = null;
                    }
                }
            }
            for (int i = 0; i < t.takımSayısı / grupBuyuklugu; i++)
            {
                if (!Finalistler[finalTurSay - 1][0][i].takım_maglubiyet)
                    t.kazanan = Finalistler[finalTurSay - 1][0][i];//Son turdaki grubun içindeki elemanlar gezilir ve mağlubiyeti false olan şampiyonumuz bulunur
            }


            //Erişim sağlayabilmek için yedekleme
            t.Gruplar = Gruplar; 
            t.Finalistler = Finalistler;

        }
        public static void GrupBastir(Turnuva t, Control[] c, int istenenGrup)
        {
            int sayac = 0;
            if (istenenGrup < t.takımlar.Count() / 8)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (!t.Gruplar[i][istenenGrup][j].takım_maglubiyet)//Gruplar için
                        {
                            ((Button)c[sayac]).Text = t.Gruplar[i][istenenGrup][j].takım_isim;//Chartın üzerindeki buttonları gezerek içlerini doldurur.
                            sayac++;
                        }
                    }

                }
            }
            else
            {
                for (int i = 0; i < (int)Math.Log(t.takımSayısı / 8, 2) + 1; i++)
                {
                    for (int j = 0; j < t.takımSayısı / 8; j++)
                    {
                        if (!t.Finalistler[i][0][j].takım_maglubiyet)//Finaller için
                        {
                            ((Button)c[sayac]).Text = t.Finalistler[i][0][j].takım_isim;
                            sayac++;
                        }
                    }

                }
            }
        }
        public static void PuanTablosuBastir(Turnuva t, DataGridView dataGridView1)
        {
            dataGridView1.Columns.Add("Takım", "Takım");
            dataGridView1.Columns.Add("Attığı Gol", "Attığı Gol");
            dataGridView1.Columns.Add("Yediği Gol", "Yediği Gol");
            dataGridView1.Columns.Add("Averaj", "Averaj");
            dataGridView1.Columns.Add("Performans Seviyesi", "Performans Seviyesi");
            DataGridViewCheckBoxColumn checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.Name = "Seribaşı";
            checkColumn.HeaderText = "Seribaşı";
            dataGridView1.Columns.Add(checkColumn);
            dataGridView1.Columns["Seribaşı"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            for (int i = 0; i < t.takımSayısı; i++)//Tüm takımların puan tablosonu hesaplar ve doldurur
            {
                Takım siradaki = t.Takımlar[i];
                int toplamGol = 0;
                int toplam_Y_Gol = 0;
                bool seribasi = false;
                if (siradaki is seribaşıTakım) seribasi = true;
                for (int j = 0; j < siradaki.YedigiGolSay.Count; j++)
                {
                    toplam_Y_Gol += siradaki.YedigiGolSay[j];
                    toplamGol += siradaki.AttigiGolSay[j];
                }
                dataGridView1.Rows.Add(siradaki.takım_isim, toplamGol, toplam_Y_Gol, toplamGol - toplam_Y_Gol, siradaki.power_rank, seribasi);
            }
        }
        public static void takımBastir(Turnuva t, Control[] c, string istenenTakımIsmi)
        {
            Takım arananTakım = new seribaşıTakım(0, 0);
            foreach (Takım temp in t.takımlar)
            {
                if (temp.takım_isim == istenenTakımIsmi)//istenen takım isimi ile aranır
                {
                    arananTakım = temp;
                    break;
                }
            }
            ((Label)c[0]).Text = arananTakım.takım_isim;

            if (arananTakım is seribaşıTakım)
                ((Label)c[4]).Text = "Seribaşı - PS\n" + arananTakım.power_rank;
            else
                ((Label)c[4]).Text = "PS\n" + arananTakım.power_rank.ToString();

            int toplam_Y_Gol = 0;
            int toplamGol = 0;
            for (int j = 0; j < arananTakım.YedigiGolSay.Count; j++)
            {
                toplam_Y_Gol += arananTakım.YedigiGolSay[j];
                toplamGol += arananTakım.AttigiGolSay[j];
            }

            ((Label)c[1]).Text = "Atılan Gol Sayısı\n" + toplamGol.ToString();
            ((Label)c[2]).Text = "Yenilen Gol Sayısı\n" + toplam_Y_Gol.ToString();
            ((Label)c[3]).Text = "Averaj\n" + (toplamGol - toplam_Y_Gol).ToString();

            ((DataGridView)c[5]).Columns.Add("Tur", "Tur");
            ((DataGridView)c[5]).Columns.Add("Ev Sahibi", "Ev Sahibi");
            ((DataGridView)c[5]).Columns.Add("E", "E");
            ((DataGridView)c[5]).Columns.Add("D", "D");
            ((DataGridView)c[5]).Columns.Add("Deplasman", "Deplasman");
            int tur = 0;
            for (int i = 0; i < t.eşleşmeler.Count; i++)//Bulunduktan sonra gerekli controllere bilgileri yazdırılır
            {
                eş siradaki = t.eşleşmeler[i];
                if (siradaki.T1.takım_isim == istenenTakımIsmi)
                {
                    ((DataGridView)c[5]).Rows.Add(tur + 1, siradaki.T1.takım_isim, arananTakım.AttigiGolSay[tur], arananTakım.YedigiGolSay[tur], siradaki.T2.takım_isim);
                    tur++;
                }
                else if (siradaki.T2.takım_isim == istenenTakımIsmi)
                {
                    ((DataGridView)c[5]).Rows.Add(tur + 1, siradaki.T1.takım_isim, arananTakım.YedigiGolSay[tur], arananTakım.AttigiGolSay[tur], siradaki.T2.takım_isim);
                    tur++;
                }
            }

        }
        public static void turBastir(Turnuva t, DataGridView dataGridView3, int tur)
        {
            dataGridView3.Rows.Clear();
            for (int i = 0; i < t.eşleşmeler.Count; i++)//Seçilen turdaki eşleşmeler gezilerek bilgileri bastırılır
            {
                eş siradaki = t.eşleşmeler[i];
                if (siradaki.Tur == tur)
                {
                    dataGridView3.Rows.Add(siradaki.T1.takım_isim, siradaki.T1.a_golDondur(tur), siradaki.T2.a_golDondur(tur), siradaki.T2.takım_isim);
                }
            }

        }
    }
}
