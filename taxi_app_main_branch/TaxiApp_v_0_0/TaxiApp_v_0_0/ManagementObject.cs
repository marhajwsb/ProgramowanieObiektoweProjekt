using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Threading;

namespace TaxiApp_v_0_0
{
    public abstract class ManagementObject
    {
        protected static List<Driver> drivers = new List<Driver>();

        public void AddDriver()
        {
            string[] driverProperties = { "imie", "nazwisko", "PESEL" };

            bool areDataCorrect = false;
            while (!areDataCorrect)
            {
                List<string> driverParams = getMultipleData(driverProperties);
                try
                {
                    Driver newDriver = new Driver(driverParams[0], driverParams[1], driverParams[2]);
                    areDataCorrect = true; 
                    drivers.Add(newDriver);
                    Console.WriteLine(newDriver._name);
                    Console.WriteLine(newDriver._surname);
                    Console.WriteLine(newDriver._driverId);
                    Console.WriteLine(newDriver._driverId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
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
                    ifDeleted = true;
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

        public void findByName()
        {
            string[] driverProperties = { "imie", "nazwisko" };
            List<string> searchParams = getMultipleData(driverProperties);
            bool found = false;
            foreach (var driver in drivers)
            {
                if (driver._name == searchParams[0] && driver._surname == searchParams[1])
                {
                    Console.WriteLine($"\nID: {driver._driverId}\nImię: {driver._name}\nNazwisko: {driver._surname}\nPESEL: {driver._PESEL}\nZarobki: {driver._earnings}\n---");
                    found = true;
                }
            }
            if (!found)
            {
                Console.WriteLine("Nie znaleziono kierowcy.");
            }
        }

        public void findByPesel()
        {
            string[] driverProperties = { "PESEL" };
            List<string> searchParams = null;
            long PESEL = 0;
            bool areDataCorrect = false;
            while (!areDataCorrect)
            {
                searchParams = getMultipleData(driverProperties);
                try
                {
                    if (long.TryParse(searchParams[0], out long longPESEL))
                    {
                        PESEL = longPESEL;
                        areDataCorrect = true;
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nWprowadzony PESEL jest niepoprawny. Zweryfikuj wprowadzone dane i spróbuj ponownie.");
                }
            }

            bool found = false;
            foreach (var driver in drivers)
            {
                if (driver._PESEL == PESEL)
                {
                    Console.WriteLine($"\nID: {driver._driverId}\nImię: {driver._name}\nNazwisko: {driver._surname}\nPESEL: {driver._PESEL}\nZarobki: {driver._earnings}\n---");
                    found = true;
                }
            }
            if (!found)
            {
                Console.WriteLine("Nie znaleziono kierowcy.");
            }
        }

        public void FindDriver()
        {
            Console.Clear();

            bool loop = true;
            while (loop)
            {
                Console.WriteLine("Wybierz interesującą Ciebie opcję:" +
                "\na - wyszukiwanie wg imienia i nazwiska" +
                "\nb - wyszukiwanie wg numeru PESEL" +
                "\nx - aby wyjść");
                char choosenOption;
                do
                {
                    choosenOption = Console.ReadKey(true).KeyChar;
                    Console.WriteLine(choosenOption);
                } while (char.IsWhiteSpace(choosenOption));
                switch (choosenOption)
                {
                    case 'a':
                        findByName();
                        break;
                    case 'b':
                        findByPesel();
                        break;
                    case 'x':
                        loop = false;
                        break;
                    default:
                        Console.WriteLine(choosenOption);
                        Console.WriteLine("Wybrales opcję, której nie ma. Dokonaj wyboru ponownie, aby zakończyć wprowadź 'x'");
                        break;
                }
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
                Console.WriteLine($"\nID: {driver._driverId}\nImię: {driver._name}\nNazwisko: {driver._surname}\nPESEL: {driver._PESEL}\nZarobki: {driver._earnings}\n---");
            }
        }

        protected decimal RatePerKm = 2.5m;
        protected decimal commissionRate = 0.1m;

        public async Task AssignRouteToDriver()
        {
            Console.WriteLine("Podaj ID kierowcy:");
            int driverId = int.Parse(Console.ReadLine());

            var driver = drivers.FirstOrDefault(d => d._driverId == driverId);

            if (driver == null)
            {
                Console.WriteLine("Nie znaleziono kierowcy o podanym ID.");
                return;
            } 
            List<string> driverParameters = new List<string>();
            driverParameters.Add($"{driver._name}");
            driverParameters.Add($"{driver._surname}");

            List<string> routeDetails = await calculateRoute();

            if (routeDetails.Count > 0)
            {
                decimal distance = decimal.Parse(routeDetails[2]) / 1000;
                driver.UpdateEarningsAndReturnCommison(routeDetails[0], routeDetails[1], distance, RatePerKm, commissionRate, driverParameters);
            }
            else
            {
                Console.WriteLine("Wystąpił problem z przeliczeniem kursu.");
            }
        }

        protected static decimal budget = 0;

        public static void takeCommision(string end, string start, decimal commision, List<string> driverParameters)
        {
            budget += commision;
            Console.WriteLine($"\nZrealizowano kurs z {start} do {end}.\n" +
                $"Kurs zrealizowany przez kierowcę {driverParameters[0]} {driverParameters[1]}.\n" +
                $"Prowizja w kwocie {commision} doliczona do budżetu.\n" +
                $"Aktualny stan budżetu firmy: {budget}");
        }

        public virtual async Task<List<string>> calculateRoute()
        {
            string[] startAddressProperties = { "początkową miejscowość", "początkową ulicę", "początkowy numer budynku" };
            string[] endAddressProperties = { "końcową miejscowość", "końcową ulicę", "końcowy numer budynku" };

            string apiKey = "AIzaSyCSoBDJXMCbiCBYFIzAfT3sa_HciPuCouE";

            List<string> completeOriginAddress = getMultipleData(startAddressProperties);
            List<string> completeDestinationAddress = getMultipleData(endAddressProperties);
            List<string> jsonResponse = new List<string>();

            var queryURL = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={completeOriginAddress[0]},{completeOriginAddress[1]},{completeOriginAddress[2]}&destinations={completeDestinationAddress[0]},{completeDestinationAddress[1]},{completeDestinationAddress[2]}&key={apiKey}";

            using (var client = new HttpClient())
            {

                HttpResponseMessage response = await client.GetAsync(queryURL);

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

        public virtual List<string> getMultipleData(string[] addressProperties)
        {
            char ifContinue;
            List<string> completeAddress = new List<string>();
            string inputValue = "";

            while (true)
            {
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