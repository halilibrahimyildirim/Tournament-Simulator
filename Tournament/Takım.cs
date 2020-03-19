using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Tournament
{
    abstract class Takım : IComparable<Takım> , ICloneable//Kaynak:https://stackoverflow.com/questions/5359318/how-to-clone-objects?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
    {
        //Field
        protected int powerRank;
        protected string takımİsmi;
        protected bool maglubiyet=false;
        protected List<int> attigiGolSay = new List<int>();
        protected List<int> yedigiGolSay = new List<int>();


        //Encapsulation
        public bool takım_maglubiyet { get { return this.maglubiyet; } }
        public string takım_isim { get { return this.takımİsmi; } }
        public int power_rank { get { return this.powerRank; } }
        public List<int> AttigiGolSay { get => attigiGolSay;}
        public List<int> YedigiGolSay { get => yedigiGolSay;}

        //Method
        abstract public int oyunOyna();
        abstract public int a_golDondur(int a);
        abstract public int y_golDondur(int a);

        public static bool sonuc(Takım takimA, Takım takimB)//İki takımın mücadelesi (Eğer berabere kalınırsa bu fonksiyon false döndürür)
        {
            int takimA_puan = takimA.oyunOyna();//İki takımında maç performans alınır
            int takimB_puan = takimB.oyunOyna();
            if (takimA_puan > takimB_puan)//Yüksek olen kazandı kabul edilir
            {
                takimB.maglubiyet = true;
                int atilanGol;
                atilanGol=Randomizer.randomSayi(1, 7);//rastgele maç sonucu oluşturulur
                takimA.attigiGolSay.Add(atilanGol);
                takimB.yedigiGolSay.Add(atilanGol);
                atilanGol = Randomizer.randomSayi(0, atilanGol-1);
                takimA.yedigiGolSay.Add(atilanGol);
                takimB.attigiGolSay.Add(atilanGol);
                return true;
            }
            else if (takimA_puan < takimB_puan)
            {
                takimA.maglubiyet = true;
                int atilanGol;
                atilanGol = Randomizer.randomSayi(1, 7);
                takimA.yedigiGolSay.Add(atilanGol);
                takimB.attigiGolSay.Add(atilanGol);
                atilanGol = Randomizer.randomSayi(0, atilanGol - 1);
                takimA.attigiGolSay.Add(atilanGol);
                takimB.yedigiGolSay.Add(atilanGol);
                return true;
            }
            else return false;
        }

        public int CompareTo(Takım a)//Kullanmayı planladık ama nasip olmadı
        {
            return this.powerRank.CompareTo(a.powerRank);
        }

        public object Clone() //Nesnemiz bir kopyasını oluşturmak için kullandık
        {
            return this.MemberwiseClone();
        } 
    }
}
