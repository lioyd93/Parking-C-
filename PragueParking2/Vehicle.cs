using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking2
{
    [Serializable]
    public abstract class Vehicle
    {
        public int size;
        public string regNr;
        public string brand;
        public string color;
        protected DateTime time;

        public DateTime GetDateTime()
        {
            return time;
        }

        public override string ToString()
        {
            return $"{color} {brand} {regNr}";
        }
    }
}
