using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking2
{
    [Serializable]
    public class Trike : Vehicle
    {
        public Trike(string regNr, string color, string brand)
        {
            this.regNr = regNr;
            this.color = color;
            this.brand = brand;
            size = 3;
            time = DateTime.Now;
        }

        public override string ToString()
        {
            return "Trike, " + base.ToString();
        }
    }
}
