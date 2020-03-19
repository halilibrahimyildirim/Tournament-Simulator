using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tournament
{
    class takimSayisiHatasi : ApplicationException
    {
        public takimSayisiHatasi(string message) : base(message) { }
    }

    class negatifSayiHatasi : ApplicationException
    {
        public negatifSayiHatasi(string message) : base(message) { }
    }

    class takimIsmiHatasi : ApplicationException
    {
        public takimIsmiHatasi(string message) : base(message) { }
    }

    class turHatasi : ApplicationException
    {
        public turHatasi(string message) : base(message) { }
    }
}
