using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tournament
{
    class normalTakım : Takım
    {
        public override int oyunOyna()//Kendi power seviyesinde 110 a kadar yükselen bir sayi döndürür.Maç bazında performans seviyesi.
        {
            return Randomizer.randomSayi(this.powerRank, 110);
        }

        public override int a_golDondur(int indis)//İstenilen turdaki attığı gol sayisini verir
        {
            return (int)this.attigiGolSay[indis];
        }

        public override int y_golDondur(int indis)//İstenilen turdaki yediği gol sayisini verir
        {
            return (int)this.yedigiGolSay[indis];
        }

        public normalTakım(int no,int pr) //Constructor
        {
            this.powerRank = pr;
            this.takımİsmi = "Takım" + no;
        }
    }
}
