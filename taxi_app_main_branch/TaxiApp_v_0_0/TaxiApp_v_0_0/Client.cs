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

        public int ClientId { get; private set; }
        public string Name { get; set; }
        public int Rating { get; set; }
        public string PESEL { get; set; }
        public string CardNumber { get; set; }
        public int NumberOfTrips { get; set; }
        public bool IsInTrip { get; set; }
        public bool HasFinancialAreas { get; set; }
        public string PhoneNumber { get; set; }

        public Client(string name, int rating, string pesel, string cardNumber, int numberOfTrips,
                  bool isInTrip, bool hasFinancialAreas, string phoneNumber)
        {
            ClientId = lastClientId++;
            Name = name;
            Rating = rating;
            PESEL = pesel;
            CardNumber = cardNumber;
            NumberOfTrips = numberOfTrips;
            IsInTrip = isInTrip;
            HasFinancialAreas = hasFinancialAreas;
            PhoneNumber = phoneNumber;
        }
    }
}