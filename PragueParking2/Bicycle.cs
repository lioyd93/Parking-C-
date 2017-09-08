using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking2
{
    [Serializable]
    public class Bicycle : Vehicle
    {
        public Bicycle(string regNr, string color, string brand)
        {
            this.regNr = regNr;
            this.color = color;
            this.brand = brand;
            size = 1;
            time = DateTime.Now;
        }

        public override string ToString()
        {
            return "Bicycle, " + base.ToString();
        }
    }
}
