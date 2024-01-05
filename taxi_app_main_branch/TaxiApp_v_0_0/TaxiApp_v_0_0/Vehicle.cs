using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp_v_0_0
{
    public class Vehicle
    {
        private static int lastVehicleId = 0;

        public int VehicleId { get; private set; }
        public string VIN { get; set; }
        public int Year { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public double PurchasePrice { get; set; }
        public double CurrentValuation { get; set; }
        public double MaintenanceCosts { get; set; }
        public double AverageFuelConsumption { get; set; }
        public double Mileage { get; set; }
        public double Capacity { get; set; }
        public Driver CurrentDriver { get; set; }
        public int NumberOfTrips { get; set; }
        public double MileageInCompany { get; set; }
        public string RegistrationNumber { get; set; }

        public Vehicle(string vin, int year, string brand, string model, double purchasePrice, double currentValuation,
                   double maintenanceCosts, double averageFuelConsumption, double mileage, double capacity,
                   Driver currentDriver, int numberOfTrips, double mileageInCompany, string registrationNumber)
        {
            VehicleId = lastVehicleId++;
            VIN = vin;
            Year = year;
            Brand = brand;
            Model = model;
            PurchasePrice = purchasePrice;
            CurrentValuation = currentValuation;
            MaintenanceCosts = maintenanceCosts;
            AverageFuelConsumption = averageFuelConsumption;
            Mileage = mileage;
            Capacity = capacity;
            CurrentDriver = currentDriver;
            NumberOfTrips = numberOfTrips;
            MileageInCompany = mileageInCompany;
            RegistrationNumber = registrationNumber;
        }

        public void UpdateMileage(double newMileage)
        {
            if (newMileage > Mileage)
            {
                Mileage = newMileage;
                MileageInCompany += newMileage - Mileage;
            }
            else
            {
                Console.WriteLine("Nowy przebieg musi być większy niż obecny przebieg.");
            }
        }

        public void UpdateValuation(double newValuation)
        {
            CurrentValuation = newValuation;
        }

        public void RegisterVehicle(Driver driver, List<Vehicle> vehiclesList)
        {

            Console.WriteLine("Wpisz dane pojazdu:");

            Console.WriteLine("VIN: ");
            VIN = Console.ReadLine();

            Console.WriteLine("Rok: ");
            Year = int.Parse(Console.ReadLine());

            Console.WriteLine("Marka: ");
            Brand = Console.ReadLine();

            Console.WriteLine("Model: ");
            Model = Console.ReadLine();

            Console.WriteLine("Cena zakupu: ");
            PurchasePrice = double.Parse(Console.ReadLine());

            Console.WriteLine("Aktualna wycena: ");
            CurrentValuation = double.Parse(Console.ReadLine());

            Console.WriteLine("Koszty utrzymania: ");
            MaintenanceCosts = double.Parse(Console.ReadLine());

            Console.WriteLine("Średnie zużycie paliwa: ");
            AverageFuelConsumption = double.Parse(Console.ReadLine());

            Console.WriteLine("Przebieg: ");
            Mileage = double.Parse(Console.ReadLine());

            Console.WriteLine("Pojemność: ");
            Capacity = double.Parse(Console.ReadLine());

            Console.WriteLine("Liczba podróży: ");
            NumberOfTrips = int.Parse(Console.ReadLine());

            Console.WriteLine("Przebieg w firmie: ");
            MileageInCompany = double.Parse(Console.ReadLine());

            Console.WriteLine("Numer rejestracyjny: ");
            RegistrationNumber = Console.ReadLine();

            Vehicle newVehicle = new Vehicle(VIN, Year, Brand, Model, PurchasePrice, CurrentValuation,
                MaintenanceCosts, AverageFuelConsumption, Mileage, Capacity, driver, NumberOfTrips,
                MileageInCompany, RegistrationNumber);

            vehiclesList.Add(newVehicle);

            CurrentDriver = driver;

            driver.AssignVehicle(newVehicle);

            Console.WriteLine($"{Brand} {Model} zarejstrowany na {driver.FirstName} {driver.LastName}.");

        }

        public void DeregisterVehicle(List<Vehicle> vehiclesList, Vehicle vehicle)
        {
            vehiclesList.Remove(vehicle);
        }
    }

}