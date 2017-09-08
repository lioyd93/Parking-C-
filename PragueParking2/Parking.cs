using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace PragueParking2
{
    public static class Parking
    {
        public const int lotSize = 100;
        static private ParkingSpace[] parkingSpaces = new ParkingSpace[lotSize];
        private static string filename = "parkingFile";

        public static void InitiateParkingSpaceArray()
        {
            for (int i = 0; i < lotSize; i++)
                parkingSpaces[i] = new ParkingSpace();
        }

        public static void FillLot()
        {
            Random random = new Random();
            int i = 1;
            int result;
            int enoughSpace;
            bool keepGoing = true;
            string reg = "ABC";
            string color = "Red";
            string brand = "Suzuki";
            while (keepGoing)
            {
                result = random.Next(4);
                if (result == 0)
                {
                    Vehicle car = new Car(reg + i, color, brand);
                    enoughSpace = Add(car);
                    if (enoughSpace == -1)
                        keepGoing = false;
                }
                else if (result == 1)
                {
                    Vehicle bicycle = new Bicycle(reg + i, color, brand);
                    enoughSpace = Add(bicycle);
                    if (enoughSpace == -1)
                        keepGoing = false;
                }
                else if (result == 2)
                {
                    Vehicle mc = new Motorcycle(reg + i, color, brand);
                    enoughSpace = Add(mc);
                    if (enoughSpace == -1)
                        keepGoing = false;
                }
                else if (result == 3)
                {
                    Vehicle trike = new Trike(reg + i, color, brand);
                    enoughSpace = Add(trike);
                    if (enoughSpace == -1)
                        keepGoing = false;
                }
                i++;
            }
            for (int index = 0; index < lotSize; index += 2)
            {
                parkingSpaces[index].Remove(parkingSpaces[index].GetVehicleList().First());
            }
        }

        public static int Add(Vehicle vehicle)
        {
            for (int i = 0; i < lotSize; i++)
            {
                if (vehicle.size <= parkingSpaces[i].remaining)
                {
                    parkingSpaces[i].Add(vehicle);
                    return i;
                }
            }
            return -1;
        }

        public static TimeSpan TimeParked(string regNr)
        {
            DateTime added = DateTime.MinValue;
            DateTime removed = DateTime.MinValue;

            int parkingLotId = Find(regNr);
            if (parkingLotId == -1)
            {
                return TimeSpan.MaxValue;
            }

            foreach (Vehicle vehicle in parkingSpaces[parkingLotId].GetVehicleList())
            {
                if (vehicle.regNr == regNr)
                {
                    added = vehicle.GetDateTime();
                    removed = DateTime.Now;
                    break;
                }
            }

            return removed - added;
        }

        public static int Remove(string regNr)
        {
            int parkingLotId = Find(regNr);
            if (parkingLotId == -1)
            {
                return -1;
            }

            else
            {
                foreach (Vehicle vehicle in parkingSpaces[parkingLotId].GetVehicleList())
                {
                    if (vehicle.regNr == regNr)
                    {
                        parkingSpaces[parkingLotId].Remove(vehicle);
                        break;
                    }
                }
            }
            return parkingLotId;
        }

        public static int Find(string regNr)
        {
            int notFound = -1;
            for (int i = 0; i < lotSize; i++)
            {
                if (parkingSpaces[i].Find(regNr) != null)
                    return i;
            }
            return notFound;
        }

        public static bool Move(string regNr, int destination)
        {
            int oldPosition = Find(regNr);
            if (oldPosition == -1)
                return false;
            Vehicle vehicle = parkingSpaces[oldPosition].Find(regNr);
            if (parkingSpaces[destination].remaining >= vehicle.size)
            {
                parkingSpaces[destination].Add(vehicle);
                parkingSpaces[oldPosition].Remove(vehicle);
                return true;
            }
            return false;
        }

        public static ParkingSpace[] Content()
        {
            return parkingSpaces;
        }

        public static void SaveParkingLotJson()
        {
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = new StreamWriter(@"C:\Users\Elev1\Documents\Visual Studio 2017\Projects\PragueParking2\PragueParking2\bin\Debug\parkingLot.json"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, parkingSpaces);
            }
        }

        public static void SaveParkingLots()
        {
            // Persist to file
            using (System.IO.FileStream stream = File.Create(filename))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, parkingSpaces);
            }
            Console.WriteLine("The parking lots are saved.");
        }
        public static void LoadParkingLots()
        {
            using (FileStream stream = File.OpenRead(filename))
            {
                // Read binary file
                var formatter = new BinaryFormatter();
                // Interpret file as a ParkingSpace array.
                ParkingSpace[] result = (ParkingSpace[])formatter.Deserialize(stream);
                // Copy result to ParkingSpaces array.                
                Array.Copy(result, parkingSpaces, result.Length);
            } // end using Filestream
            Console.WriteLine("Parking lots have been successfully loaded.");
            //displayParkingLots();            
        }

        public static StringBuilder Optimize()
        {
            StringBuilder commands = new StringBuilder();
            List<int> unpairedTrikeLocations = new List<int>();
            List<int> unpairedBikeLocations = new List<int>();
            
            for (int i = 0; i < lotSize; i++)
            {
                bool hasTrike = false;
                bool hasBike = false;

                foreach (Vehicle vehicle in parkingSpaces[i].GetVehicleList())
                {
                    if (vehicle is Trike)
                        hasTrike = true;
                    else if (vehicle is Bicycle)
                        hasBike = true;
                }

                if (hasBike && !hasTrike)
                    unpairedBikeLocations.Add(i);

                if (hasTrike && !hasBike)
                    unpairedTrikeLocations.Add(i);
            }

            while (unpairedBikeLocations.Count != 0)
            {
                for (int i = 0; i < parkingSpaces[unpairedBikeLocations.First()].GetVehicleList().Count; i++)
                {
                    Vehicle vehicle = parkingSpaces[unpairedBikeLocations.First()].GetVehicleList().ElementAt(i);
                    if (vehicle is Bicycle && unpairedTrikeLocations.Count != 0)
                    {
                        parkingSpaces[unpairedTrikeLocations.First()].Add(vehicle);
                        parkingSpaces[unpairedBikeLocations.First()].Remove(vehicle);
                        commands.Append($"Please move vehicle {vehicle.regNr} from space {unpairedBikeLocations.First() + 1} to space {unpairedTrikeLocations.First() + 1}.\n");
                        i--;
                        unpairedTrikeLocations.RemoveAt(0);
                    }
                }

                if (unpairedTrikeLocations.Count != 0)
                    unpairedBikeLocations.RemoveAt(0);
                else
                    break;
            }

            List<int> emptySpaces = new List<int>();
            List<int> almostEmptySpaces = new List<int>();
            List<int> halfEmptySpaces = new List<int>();
            List<int> quarterEmptySpaces = new List<int>();

            for (int i = 0; i < lotSize; i++)
            {
                if (parkingSpaces[i].remaining == 0 && emptySpaces.Count != 0)
                {
                    int destination = emptySpaces.First();
                    for (int j = 0; j < parkingSpaces[i].GetVehicleList().Count; j++)
                    {
                        Vehicle vehicle = parkingSpaces[i].GetVehicleList().ElementAt(j);
                        parkingSpaces[destination].Add(vehicle);
                        parkingSpaces[i].Remove(vehicle);
                        j--;
                        commands.Append($"Please move vehicle {vehicle.regNr} from space {i + 1} to space {destination + 1}.\n");
                    }

                    emptySpaces.RemoveAt(0);
                }
                else if (parkingSpaces[i].remaining != 0)
                {

                    for (int j = 0; j < parkingSpaces[i].GetVehicleList().Count; j++)
                    {
                        Vehicle vehicle = parkingSpaces[i].GetVehicleList().ElementAt(j);
                        int destination = Int32.MaxValue;
                        int firstEmpty;
                        if (emptySpaces.Count == 0)
                            firstEmpty = Int32.MaxValue;
                        else
                            firstEmpty = emptySpaces.First();

                        int firstAlmost;
                        if (almostEmptySpaces.Count == 0)
                            firstAlmost = Int32.MaxValue;
                        else
                            firstAlmost = almostEmptySpaces.First();

                        int firstHalf;
                        if (halfEmptySpaces.Count == 0)
                            firstHalf = Int32.MaxValue;
                        else
                            firstHalf = halfEmptySpaces.First();

                        int firstQuarter;
                        if (quarterEmptySpaces.Count == 0)
                            firstQuarter = Int32.MaxValue;
                        else
                            firstQuarter = quarterEmptySpaces.First();

                        if (vehicle is Trike && emptySpaces.Count + almostEmptySpaces.Count != 0)
                        {
                            

                            destination = Math.Min(firstEmpty, firstAlmost);
                            parkingSpaces[destination].Add(vehicle);
                            parkingSpaces[i].Remove(vehicle);
                            j--;
                            commands.Append($"Please move vehicle {vehicle.regNr} from space {i + 1} to space {destination + 1}.\n");

                            if (firstEmpty == destination)
                                emptySpaces.RemoveAt(0);
                            else
                                almostEmptySpaces.RemoveAt(0);
                        }
                        else if (vehicle is Motorcycle && emptySpaces.Count + almostEmptySpaces.Count + halfEmptySpaces.Count != 0)
                        {
                            destination = Math.Min(firstEmpty, firstAlmost);
                            destination = Math.Min(destination, firstHalf);
                            parkingSpaces[destination].Add(vehicle);
                            parkingSpaces[i].Remove(vehicle);
                            j--;
                            commands.Append($"Please move vehicle {vehicle.regNr} from space {i + 1} to space {destination + 1}.\n");

                            if (firstEmpty == destination)
                            {
                                emptySpaces.RemoveAt(0);
                            }
                            else if (firstAlmost == destination)
                            {
                                almostEmptySpaces.RemoveAt(0);
                            }
                            else
                            {
                                halfEmptySpaces.RemoveAt(0);
                            }
                        }
                        else if (vehicle is Bicycle && emptySpaces.Count + almostEmptySpaces.Count + halfEmptySpaces.Count + quarterEmptySpaces.Count != 0)
                        {
                            destination = Math.Min(firstEmpty, firstAlmost);
                            destination = Math.Min(destination, firstHalf);
                            destination = Math.Min(destination, firstQuarter);
                            parkingSpaces[destination].Add(vehicle);
                            parkingSpaces[i].Remove(vehicle);
                            j--;
                            commands.Append($"Please move vehicle {vehicle.regNr} from space {i + 1} to space {destination + 1}.\n");

                            if (firstEmpty == destination)
                            {
                                emptySpaces.RemoveAt(0);
                            }
                            else if (firstAlmost == destination)
                            {
                                almostEmptySpaces.RemoveAt(0);
                            }
                            else if (firstHalf == destination)
                            {
                                halfEmptySpaces.RemoveAt(0);
                            }
                            else
                            {
                                quarterEmptySpaces.RemoveAt(0);
                            }   
                        }

                        if (destination != Int32.MaxValue)
                        {
                            int remainingSpace = parkingSpaces[destination].remaining;

                            if (remainingSpace == 1)
                            {
                                quarterEmptySpaces.Add(destination);
                                quarterEmptySpaces.Sort();
                            }
                            else if (remainingSpace == 2)
                            {
                                halfEmptySpaces.Add(destination);
                                halfEmptySpaces.Sort();
                            }
                            else if (remainingSpace == 3)
                            {
                                almostEmptySpaces.Add(destination);
                                almostEmptySpaces.Sort();
                            }
                        }
                    }
                }

                if (parkingSpaces[i].remaining == 4)
                    emptySpaces.Add(i);
                else if (parkingSpaces[i].remaining == 3)
                    almostEmptySpaces.Add(i);
                else if (parkingSpaces[i].remaining == 2)
                    halfEmptySpaces.Add(i);
                else if (parkingSpaces[i].remaining == 1)
                    quarterEmptySpaces.Add(i);
            }
            return commands;
        }
    }
}
