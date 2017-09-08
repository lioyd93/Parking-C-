using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking2
{
    [Serializable]
    public class Car : Vehicle
    {
        public Car(string regNr, string color, string brand)
        {
            this.regNr = regNr;
            this.color = color;
            this.brand = brand;
            size = 4;
            time = DateTime.Now;
        }

        public override string ToString()
        {
            return "Car, " + base.ToString();
        }
    }
}
