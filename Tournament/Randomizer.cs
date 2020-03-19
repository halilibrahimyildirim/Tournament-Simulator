using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tournament
{
    class Randomizer
    {
        private static Random rand = new Random();
        public static int randomSayi()
        {
            return rand.Next();
        }
        public static int randomSayi(int sinir1,int sinir2)
        {
            return rand.Next(sinir1, sinir2 + 1);
        }
    }
}
