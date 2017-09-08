using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking2
{
    [Serializable]
    public class ParkingSpace
    {
        public int capacity;
        public int remaining;
        List<Vehicle> vehicles;

        public ParkingSpace()
        {
            capacity = 4;
            remaining = 4;
            vehicles = new List<Vehicle>();
        }

        public void Add(Vehicle vehicle)
        {
            vehicles.Add(vehicle);
            remaining = remaining - vehicle.size;
        }

        public void Remove(Vehicle vehicle)
        {
            vehicles.Remove(vehicle);
            remaining += vehicle.size;
        }

        public Vehicle Find(string regNr)
        {
            for (int i = 0; i < vehicles.Count; i++)
            {
                if (vehicles.ElementAt(i).regNr.Equals(regNr))
                    return vehicles.ElementAt(i);
            }
            return null;
        }

        public ParkingSpace Content()
        {
            return this;
        }

        public List<Vehicle> GetVehicleList()
        {
            return vehicles;
        }

        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append($"Capacity: {capacity}, Remaining capacity: {remaining}\n");
            foreach (Vehicle vehicle in vehicles)
            {
                s.Append(vehicle.ToString() + "\n");
            }
            return s.ToString();
        }
    }
}
