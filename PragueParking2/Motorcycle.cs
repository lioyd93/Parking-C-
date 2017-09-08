using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking2
{
    [Serializable]
    public class Motorcycle : Vehicle
    {
        public Motorcycle(string regNr, string color, string brand)
        {
            this.regNr = regNr;
            this.color = color;
            this.brand = brand;
            size = 2;
            time = DateTime.Now;
        }

        public override string ToString()
        {
            return "Motorcycle, " + base.ToString();
        }
    }
}
