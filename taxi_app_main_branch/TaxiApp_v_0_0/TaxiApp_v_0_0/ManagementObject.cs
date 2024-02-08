using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace TaxiApp_v_0_0
{
    //jak od tej klasy będziemy tworzyć CEO to warto pamiętać, żeby był klasą sealed (zamkniętą)
    //od CEO nikt nie będzie dziedziczył
    public abstract class ManagementObject
    {
        //protected hermetyzacja
        protected static List<Driver> drivers = new List<Driver>();

        //**ZARZĄDZANIE KIEROWCAMI***
        public void AddDriver()
        {
            string[] driverProperties = { "imie", "nazwisko" };
            List<string> driverParams = getMultipleData(driverProperties);
            
            Console.WriteLine(driverParams[0]);

            Driver newDriver = new Driver(driverParams[0], driverParams[1]);
            drivers.Add(newDriver);
            Console.WriteLine(newDriver._name);
            Console.WriteLine(newDriver._surname);
            Console.WriteLine(newDriver._driverId);
        }

        public void RemoveDriver()
        {
            string[] driverId = { "id kierowcy" };
            List<string> driverParam = getMultipleData(driverId);
            int driverIdToFind;
            bool ifDeleted = false;
            bool isValidNumber = int.TryParse(driverParam[0], out driverIdToFind);

            if (isValidNumber)
            {
                var driver = drivers.FirstOrDefault(d => d._driverId == driverIdToFind);
                if (driver != null)
                {
                    drivers.Remove(driver);
                    ifDeleted = true; // Zwraca true, jeśli kierowca został usunięty.
                }
            }
            
            if (ifDeleted)
            {
                Console.WriteLine("Kierowca usunięty.");
            } else
            {
                Console.WriteLine("Kierowca o podanym id {0} nie znaleziony.", driverParam[0]);
            }
        }

        public void FindDriver()
        {
            string[] driverProperties = { "imie", "nazwisko" };
            List<string> searchParams = getMultipleData(driverProperties);
            bool found = false;
            foreach (var driver in drivers)
            {
                if (driver._name == searchParams[0] && driver._surname == searchParams[1])
                {
                    Console.WriteLine($"ID: {driver._driverId} Imię: {driver._name} Nazwisko: {driver._surname} Zarobki: {driver._earnings}");
                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("Nie znaleziono kierowcy.");
            }
        }

        public static void ListAllDrivers()
        {
            if (drivers.Count == 0)
            {
                Console.WriteLine("Brak kierowców na liście.");
                return;
            }

            Console.WriteLine("Lista kierowców:");
            foreach (var driver in drivers)
            {
                Console.WriteLine($"ID: {driver._driverId} Imię: {driver._name} Nazwisko: {driver._surname} Zarobki: {driver._earnings}");
            }
        }
        //**KONIEC ZARZĄDZANIA KIEROWCAMI***

        //KURS
        protected decimal RatePerKm = 2.5m; // Przykładowa wartość
        protected decimal commissionRate = 0.1m;

        public async Task AssignRouteToDriver()                         //moze zmienic nazwe na realizeCourse?
        {
            // Pobieranie ID kierowcy
            Console.WriteLine("Podaj ID kierowcy:");
            int driverId = int.Parse(Console.ReadLine());

            // Znajdowanie kierowcy
            var driver = drivers.FirstOrDefault(d => d._driverId == driverId);
            if (driver == null)
            {
                Console.WriteLine("Nie znaleziono kierowcy o podanym ID.");
                return;
            }

            List<string> routeDetails = await calculateRoute();

            foreach (string element in routeDetails)
            {
                Console.WriteLine(element);
            }

            if (routeDetails.Count > 0)
            {
                decimal distance = decimal.Parse(routeDetails[2]) / 1000;
                driver.UpdateEarningsAndReturnCommison(distance, RatePerKm, commissionRate);
            }
            else
            {
                Console.WriteLine("Nie udało się obliczyć kursu.");
            }
        }

        protected static decimal budget = 0;

        public static void takeCommision(decimal commision)
        {
            Console.WriteLine("wzinto prowizje");
            budget += commision;
            Console.WriteLine(budget);
        }
        //KURS
        //**ZARZĄDZANIE KURSAMI***
        public virtual async Task<List<string>> calculateRoute()
        {
            string[] startAddressProperties = { "początkową miejscowość", "początkową ulicę", "początkowy numer budynku" };
            string[] endAddressProperties = { "końcową miejscowość", "końcową ulicę", "końcowy numer budynku" };

            //Klucz API do wrzucenia do oddzielnego pliku                                             !!!!TO-DO!!!!!
            var apiKey = "AIzaSyCSoBDJXMCbiCBYFIzAfT3sa_HciPuCouE";

            List<string> completeOriginAddress = getMultipleData(startAddressProperties);
            List<string> completeDestinationAddress = getMultipleData(endAddressProperties);
            List<string> jsonResponse = new List<string>();

            var queryURL = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={completeOriginAddress[0]},{completeOriginAddress[1]},{completeOriginAddress[2]}&destinations={completeDestinationAddress[0]},{completeDestinationAddress[1]},{completeDestinationAddress[2]}&key={apiKey}";

            using (var client = new HttpClient())
            {
                //Zapytanie odbywa się asynchroncznie; w przypadku rozbudowy aplikacji
                //albo dodania interfejsu graficznego warto przerobić na asynchroniczne przetwarzanie zapytań
                HttpResponseMessage response = await client.GetAsync(queryURL);
                //dobrze dodać try ... catch do wyłapania i obsługi ewentualnych błędów
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var jsonDocument = JsonDocument.Parse(responseBody);
                    jsonResponse.Add(jsonDocument.RootElement.GetProperty("destination_addresses")[0].ToString());
                    jsonResponse.Add(jsonDocument.RootElement.GetProperty("origin_addresses")[0].ToString());

                    var rows = jsonDocument.RootElement.GetProperty("rows");
                    var elements = rows[0].GetProperty("elements");
                    var distance = elements[0].GetProperty("distance");
                    var distanceValue = distance.GetProperty("value");
                    jsonResponse.Add(distanceValue.ToString());

                    var duration = elements[0].GetProperty("duration");
                    var durationValue = duration.GetProperty("value");
                    jsonResponse.Add(durationValue.ToString());

                    return jsonResponse;
                }
                else
                {
                    Console.WriteLine("Nie udało się uzyskać odpowiedzi.");
                }
            }
            return new List<string>();
        }
        //***KONIEC ZARZĄDZANIA KURSAMI***
        //pobieranie danych
        public virtual List<string> getMultipleData(string[] addressProperties)
        {
            char ifContinue;
            List<string> completeAddress = new List<string>();
            string inputValue = "";

            while (true)
            {
                Console.Clear();
                foreach (string val in addressProperties)
                {
                    while (true)
                    {
                        Console.WriteLine("Podaj {0}", val);
                        inputValue = Console.ReadLine();

                        if (!string.IsNullOrEmpty(inputValue))
                        {
                            completeAddress.Add(inputValue);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Wprowadzono pusty znak. Sprobuj ponownie.");
                        }
                    }
                }
                Console.Clear();
                Console.WriteLine("Czy poniższe dane są poprawne?");
                foreach (string address in completeAddress)
                {
                    Console.WriteLine(address);
                }
                Console.WriteLine("Wprowadz 'y' aby zatwierdzić dane lub wprowadź 'n' aby ponownie podać dane.");
                do
                {
                    ifContinue = Console.ReadKey().KeyChar;
                    if (ifContinue == 'y')
                    {
                        return completeAddress;
                    }
                    else if (ifContinue == 'n')
                    {
                        completeAddress.Clear();
                        Console.WriteLine("\nWprowadź ponownie dane.");
                    }
                    else
                    {
                        Console.WriteLine("\nWprowadź 'y' w celu zatwierdzenia, 'n' w celu ponownego wprowadzenia danych" +
                            "Czy na pewno wprowadziłeś poprawną opcję?");
                    }
                } while (ifContinue != 'y' && ifContinue != 'n');
            }
        }

        public virtual async Task chooseOption()
        {

        }
    }
}