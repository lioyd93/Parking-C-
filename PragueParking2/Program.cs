using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PragueParking2._0a
{
    public class Program
    {

        static string SetRegNr()
        {
            Console.Clear();
            Console.Write("Registration number: ");
            return Console.ReadLine();
        }

        static string SetColor()
        {
            Console.Clear();
            Console.Write("Color: ");
            return Console.ReadLine();
        }

        static string SetBrand()
        {
            Console.Clear();
            Console.Write("Brand: ");
            return Console.ReadLine();
        }

        static bool ValidateVehicleBrand(string vehicleBrand)
        {
            bool valid = false;
            Regex r1 = new Regex("^[A-Za-z0-9]+$");
            Match match = r1.Match(vehicleBrand);
            if (vehicleBrand.Length <= 15 && match.Success)
            {
                valid = true;
            }
            else
            {
                valid = false;
            }
            return valid;
        }

        static bool ValidateVehicleColors(string vehicleColor)
        {
            bool valid = false;
            Regex r1 = new Regex("^[a-zA-Z]+$");
            Match match = r1.Match(vehicleColor);
            if (vehicleColor.Length <= 15 && match.Success)
            {
                valid = true;
            }
            else
            {
                valid = false;
            }
            return valid;
        }

        static bool ValidateVehicleRegistrationNumber(string vehicleRegistrationNumber)
        {
            bool valid = false;
            Regex r1 = new Regex("^[A-Za-z0-9]+$");
            Match match = r1.Match(vehicleRegistrationNumber);
            if (vehicleRegistrationNumber.Length <= 15 && match.Success)
            {
                valid = true;
            }
            else
            {
                valid = false;
            }
            return valid;
        }

        static void MakeAndAdd(string type, string regNr, string color, string brand)
        {
            int position = -1;

            switch (type)
            {

                case "1":
                    // bicycle
                    Vehicle bike = new Bicycle(regNr, color, brand);
                    position = Parking.Add(bike);
                    break;
                case "2":
                    // motorcycle
                    Vehicle mc = new Motorcycle(regNr, color, brand);
                    position = Parking.Add(mc);
                    break;
                case "3":
                    //trike
                    Vehicle trike = new Trike(regNr, color, brand);
                    position = Parking.Add(trike);
                    break;
                case "4":
                    //car
                    Vehicle car = new Car(regNr, color, brand);
                    position = Parking.Add(car);
                    break;
            }

            if (position == -1)
            {
                Console.WriteLine("Not enough space.");
            }
            else
            {
                Console.WriteLine($"The vehicle {regNr} has been added in space {position + 1}.");
            }
        }

        static void DisplayParkingLotColor()
        {
            ParkingSpace[] content = Parking.Content();
            int lineCounter = 0;
            for (int i = 0; i < Parking.lotSize; i++)
            {
                if (content[i].Content().remaining == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\u25a0 ");
                }
                else if (content[i].Content().remaining == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("\u25a0 ");
                }
                else if (content[i].Content().remaining == 3)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("\u25a0 ");
                }
                else if (content[i].Content().remaining == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\u25a0 ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("\u25a0 ");
                }

                lineCounter++;
                ///Only show 10 each line
                if (lineCounter % 10 == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nRed: full || Gray: 3/4 full || Yellow: 1/2 full || Blue: 1/4 full || Green: empty");
        }

        static void DisplayParkingLotColorBlind()
        {
            ParkingSpace[] content = Parking.Content();
            int lineCounter = 0;
            StringBuilder display = new StringBuilder();
            for (int i = 0; i < Parking.lotSize; i++)
            {
                if (content[i].Content().remaining == 0)
                {
                    display.Append(" X");
                }
                else if (content[i].Content().remaining == 1)
                {
                    display.Append(" >");
                }
                else if (content[i].Content().remaining == 3)
                {
                    display.Append(" <");
                }
                else if (content[i].Content().remaining == 2)
                {
                    display.Append(" /");
                }
                else
                {
                    display.Append(" O");
                }
                lineCounter++;
                ///Only show 10 each line
                if (lineCounter % 10 == 0)
                {
                    display.Append("\n");
                }
            }
            Console.WriteLine(display);
            Console.Write("\n(X) = full || (>) = 3/4 full || (/) = 1/2 full || (<) = 1/4 full || (O) = empty");
        }

        static void DisplayParkingLotText()
        {
            ParkingSpace[] content = Parking.Content();
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < Parking.lotSize; i++)
            {
                output.Append($"Space {i + 1}\n{content[i].Content().ToString()}\n");
            }
            Console.Write(output);
        }

        static void InitiateProgram()
        {
            Parking.InitiateParkingSpaceArray();
            Parking.FillLot();
            bool menu = true;
            while (menu)
            {
                Console.Clear();
                Console.WriteLine("## Prague Parking2.0a ##\n");
                Console.Write("1. Add\n2. Remove\n3. Move\n4. Search\n5. Optimize\n6. Display\n7. Save\n8. Load\n9. Exit\n\n");
                switch (Console.ReadLine())
                {

                    //add vehicle
                    case "1":
                        // type, brand, color, regnr
                        Console.Clear();
                        Console.WriteLine("Add vehicle: \n");
                        string vehicleType, vehicleBrand, vehicleColor, vehicleRegistrationNumber;
                        Console.Write("1. Bicycle\n2. Motorcycle\n3. Trike\n4. Car\n\n");
                        vehicleType = Console.ReadLine();
                        if (vehicleType == "1" || vehicleType == "2" || vehicleType == "3" || vehicleType == "4")
                        {
                            Console.Clear();
                            Console.Write("Brand: ");
                            vehicleBrand = Console.ReadLine();
                            if (!ValidateVehicleBrand(vehicleBrand))
                            {
                                Console.WriteLine("Brand input is invalid, try again with a maximum of fiftheen letters and numbers.");
                                break;
                            }
                            else
                            {
                                Console.Clear();
                                Console.Write("Colour: ");
                                vehicleColor = Console.ReadLine();
                                if (!ValidateVehicleColors(vehicleColor))
                                {
                                    Console.WriteLine("Color input is invalid, try again with a maximum of fiftheen letters.");
                                    break;
                                }
                                else
                                {
                                    Console.Clear();
                                    Console.Write("Registration number: ");
                                    vehicleRegistrationNumber = Console.ReadLine();

                                    if (!ValidateVehicleRegistrationNumber(vehicleRegistrationNumber))
                                    {
                                        Console.WriteLine("Registration number input is invalid, try again with a maximum of fiftheen letters and numbers.");
                                        break;
                                    }
                                    else
                                    {
                                        MakeAndAdd(vehicleType, vehicleRegistrationNumber, vehicleColor, vehicleBrand);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("You pressed something besides 1-4, try again.");
                        }
                        break;
                    case "2":
                        //remove vehicle
                        int result;
                        Console.Clear();
                        Console.WriteLine("Remove vehicle:\n");
                        Console.Write("Registration number: ");
                        vehicleRegistrationNumber = Console.ReadLine();
                        TimeSpan timestoodhere = Parking.TimeParked(vehicleRegistrationNumber);
                        if (timestoodhere == TimeSpan.MaxValue)
                            Console.WriteLine("That vehicle is not registered here.");
                        else
                        {
                            var timeoutput = string.Format($"{timestoodhere.Hours}:{timestoodhere.Minutes}:{timestoodhere.Seconds}");
                            Console.WriteLine(timeoutput);
                            Console.WriteLine();
                            result = Parking.Remove(vehicleRegistrationNumber);
                            Console.WriteLine($"The vehicle {vehicleRegistrationNumber} has been unregistered. Please remove it from space {result + 1}.");
                        }
                        break;
                    case "3":
                        // move vehicle
                        int destination;
                        Console.Clear();
                        Console.WriteLine("Move vehicle:\n");
                        Console.Write("Registration number: ");
                        vehicleRegistrationNumber = Console.ReadLine();
                        result = Parking.Find(vehicleRegistrationNumber);
                        if (result == -1)
                        {
                            Console.WriteLine($"The vehicle {vehicleRegistrationNumber} was not found. Returning to main menu.");
                        }
                        else
                        {
                            Console.Write($"The vehicle {vehicleRegistrationNumber} is currently parked in space {result + 1}. Which space would you like to move it to? ");
                            bool succeeded = Int32.TryParse(Console.ReadLine(), out  destination);
                            if (!succeeded)
                                Console.WriteLine("Bad input. Returning to main menu.");
                            else
                            {
                                if (destination < 1 || destination > Parking.lotSize)
                                    Console.WriteLine("That parking space does not exist. Returning to main menu.");
                                else
                                {
                                    succeeded = Parking.Move(vehicleRegistrationNumber, destination - 1);
                                    if (!succeeded)
                                        Console.WriteLine($"There is not enough space for vehicle {vehicleRegistrationNumber} in space {destination}. Returning to main menu.");
                                    else
                                        Console.WriteLine($"The vehicle {vehicleRegistrationNumber} is now registered in space {destination}. Please move it from space {result + 1}.");
                                }
                            }
                        }
                        break;
                    case "4":
                        // search vehicle
                        Console.Clear();
                        Console.Write("Registration number: ");
                        vehicleRegistrationNumber = Console.ReadLine();
                        result = Parking.Find(vehicleRegistrationNumber);
                        if (result == -1)
                            Console.WriteLine($"The vehicle {vehicleRegistrationNumber} is not registered.");
                        else
                            Console.WriteLine($"The vehicle {vehicleRegistrationNumber} is registered in space {result + 1}.");
                        break;

                    case "5":
                        // optimize
                        Console.Write(Parking.Optimize());
                        break;
                    
                    case "6":
                        Console.Clear();
                        Console.Write("Which display do you want to use?\n\n1. Parking lot in colours\n2. Parking lot friendly for colourblinded\n3. List with all vehicles\n\n");
                        switch (Console.ReadLine())
                        {
                            case "1":
                                Console.Clear();
                                DisplayParkingLotColor();
                                break;
                            case "2":
                                Console.Clear();
                                DisplayParkingLotColorBlind();
                                break;
                            case "3":
                                Console.Clear();
                                DisplayParkingLotText();
                                break;
                            default:
                                Console.WriteLine("You typed something else than 1-3, try again");
                                break;
                        }
                        break;
                    
                    case "7":
                        Parking.SaveParkingLots();
                        break;
                    // save
                    case "8":
                        Parking.LoadParkingLots();
                        break;
                    // load
                    case "9":
                        // exit
                        menu = false;
                        break;
                    default:
                        Console.WriteLine("You pressed something besides 1-9, try again.");
                        break;
                }
                if (menu)
                    Console.ReadKey();
            }
        }


        static void Main(string[] args)
        {
            // prague parking 2.0
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            string[] logo =
            {
            "______                            ______          _    _               _____  _____ \n",
            "| ___ \\                           | ___ \\        | |  (_)             / __  \\|  _  |\n",
            "| |_/ / __ __ _  __ _ _   _  ___  | |_/ /_ _ _ __| | ___ _ __   __ _  `' / /'| |/' |\n",
            "|  __/ '__/ _` |/ _` | | | |/ _ \\ |  __/ _` | '__| |/ / | '_ \\ / _` |   / /  |  /| |\n",
            "| |  | | | (_| | (_| | |_| |  __/ | | | (_| | |  |   <| | | | | (_| | ./ /___\\ |_/ /\n",
           "\\_|  |_|  \\__,_|\\__, |\\__,_|\\___| \\_|  \\__,_|_|  |_|\\_\\_|_| |_|\\__, | \\_____(_)___/ \n",
            "                 __/ |                                          __/ |               \n",
            "                |___/                                          |___/                \n",
            };

            /// 3s Animation
            foreach (string row in logo)
            {
                for (int i = 0; i < row.Length; i++)
                {
                    Console.Write(row[i]);
                    Thread.Sleep(1);
                }
            }
            Thread.Sleep(1500);

            // Run the menu
            InitiateProgram();
        }
    }
}