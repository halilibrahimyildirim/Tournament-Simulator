using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tournament
{
    class Kura
    {
        private Random rnd;
        public Kura(int seed)
        {
            rnd = new Random(seed);
        }
        public void kuraCek(Takım[] grup,int grupB)
        {
            int x;
            for(int i=0;i< grupB; i++)//Gönderilen grubu karmak için
            {
                do
                {
                    x = this.rnd.Next(0, 7);
                } while (x == i);
                Takım temp;
                temp = grup[i];
                grup[i] = grup[x];
                grup[x] = temp;

            }
        }
    }
}
