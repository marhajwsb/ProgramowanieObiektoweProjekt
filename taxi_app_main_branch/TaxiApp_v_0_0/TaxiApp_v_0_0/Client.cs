using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp_v_0_0
{

    public class Client
    {
        private static int lastClientId = 0;
        //private ogranicza dostęp do ID tylko z poziomu klasy Client
        public int ClientId { get; private set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public string PESEL { get; set; }
        public string CardNumber { get; set; }
        public int NumberOfTrips { get; set; }
        public bool IsInTrip { get; set; }
        public bool HasFinancialAreas { get; set; }
        public string PhoneNumber { get; set; }

        //w ramach tej klasy trzeba stworzyć metodę w stylu CreateFromUserInput
        /*
         * przykładowa implementacja
         * public static Client CreateFromUserInput(){
        //pole do wprowadzania danych

        //return new Client(...)
        }
         */

        public Client(string name, int rating, string pesel, string cardNumber, int numberOfTrips,
                  bool isInTrip, bool hasFinancialAreas, string phoneNumber)
        {
            ClientId = lastClientId++;
            Name = name;
            Rating = rating;
            PESEL = pesel;
            CardNumber = cardNumber;
            NumberOfTrips = numberOfTrips;

            //is in trip powinno być w konstruktorze ustawiane na false
            //w funkcji przypisywania do kursu będziemy ustawiać czy jest na kursie czy nie;
            IsInTrip = isInTrip;
            HasFinancialAreas = hasFinancialAreas;
            PhoneNumber = phoneNumber;
        }
    }
}