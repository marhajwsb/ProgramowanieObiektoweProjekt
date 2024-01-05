using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp_v_0_0
{
    public class Driver
    {
        private static int lastDriverId = 0;

        public int DriverId { get; private set; }
        public string PESEL { get; set; }
        public DateTime BirthDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime LicenseValidity { get; set; }
        public bool IsAtWork { get; set; }
        public bool IsSelfEmployed { get; set; }
        public double KilometersDriven { get; set; }
        public bool IsOnBreak { get; set; }
        public double DailyEarnings { get; set; }
        public double WeeklyEarnings { get; set; }
        public double MonthlyEarnings { get; set; }
        public double YearlyEarnings { get; set; }
        public string BusinessRegistrationNumber { get; set; }
        public bool HasOwnCar { get; set; }
        public Vehicle CurrentVehicle { get; set; }

        public Driver(string pesel, DateTime birthDate, string firstName, string lastName, string licenseNumber,
                  DateTime licenseValidity, bool isAtWork, bool isSelfEmployed, double kilometersDriven,
                  bool isOnBreak, double dailyEarnings, double weeklyEarnings, double monthlyEarnings,
                  double yearlyEarnings, string businessRegistrationNumber, bool hasOwnCar, Vehicle currentVehicle)
        {
            DriverId = lastDriverId++;
            PESEL = pesel;
            BirthDate = birthDate;
            FirstName = firstName;
            LastName = lastName;
            LicenseNumber = licenseNumber;
            LicenseValidity = licenseValidity;
            IsAtWork = isAtWork;
            IsSelfEmployed = isSelfEmployed;
            KilometersDriven = kilometersDriven;
            IsOnBreak = isOnBreak;
            DailyEarnings = dailyEarnings;
            WeeklyEarnings = weeklyEarnings;
            MonthlyEarnings = monthlyEarnings;
            YearlyEarnings = yearlyEarnings;
            BusinessRegistrationNumber = businessRegistrationNumber;
            HasOwnCar = hasOwnCar;
            CurrentVehicle = currentVehicle;
        }

        public void AssignVehicle(Vehicle vehicle)
        {
            CurrentVehicle = vehicle;
            Console.WriteLine($"{vehicle.Brand} {vehicle.Model} przypisany do kierowcy {FirstName} {LastName}.");
        }

        public void EndShift()
        {
            IsAtWork = false;
            IsOnBreak = false;
            Console.WriteLine($"{FirstName} {LastName} zakończył zmianę.");
        }

        public void DeductMoney(double amount)
        {
            if (amount > 0 && DailyEarnings - amount >= 0)
            {
                DailyEarnings -= amount;
                Console.WriteLine($"Odjęto {amount} od dziennych zarobków kierowcy {FirstName} {LastName}");
            }
            else
            {
                Console.WriteLine("Nieprawidłowa kwota.");
            }
        }

        public void GiveBonus(double amount)
        {
            if (amount > 0)
            {
                DailyEarnings += amount;
                Console.WriteLine($"Premia o wysokości {amount} dla kierowcy {FirstName} {LastName}.");
            }
            else
            {
                Console.WriteLine("Nieprawidłowa kwota.");
            }
        }

        public void PayOut()
        {

        }

        public void AssignTrip(Trip trip)
        {
            if (IsAtWork)
            {
                if (trip != null)
                {
                    if (trip.Driver == null)
                    {
                        Console.WriteLine($"Przypisywanie podróży do: {FirstName} {LastName}. Szczegóły przejazdu: Klient: {trip.Client.Name}, Kilometry: {trip.Kilometers}");
                        trip.Driver = this;
                    }
                    else
                    {
                        Console.WriteLine($"Podróż została już przypisana innemu kierowcy.");
                    }
                }
                else
                {
                    Console.WriteLine("Szczegóły podróży nie są określone. Nie można przypisać podróży.");
                }
            }
            else
            {
                Console.WriteLine($"{FirstName} {LastName} nie jest obecnie w pracy. Nie można przypisać podróży.");
            }
        }

        public void FinishTrip()
        {

        }

        public void RemoveOwnCar()
        {
            if (HasOwnCar)
            {
                Console.WriteLine($"{FirstName} {LastName} usunął swój pojazd.");
                HasOwnCar = false;
            }
            else
            {
                Console.WriteLine($"{FirstName} {LastName} nie posiada własnego pojazdu.");
            }
        }

        public void Dismiss()
        {

        }
    }
}