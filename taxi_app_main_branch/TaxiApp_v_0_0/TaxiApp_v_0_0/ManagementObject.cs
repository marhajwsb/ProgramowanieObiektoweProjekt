using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace TaxiApp_v_0_0
{
    //jak od tej klasy będziemy tworzyć CEO to warto pamiętać, żeby był klasą sealed (zamkniętą)
    //od CEO nikt nie będzie dziedziczył
    public abstract class ManagementObject
    {
        //**ZARZĄDZANIE KURSAMI***
        //ta funkcja jest pierwotnie void, ale ostatecznie powinna zwracać dane typu int/string
        //z koordynantami dla podanego adresu
        public virtual List<string> getAddress(string direction, string[] addressProperties)
        {
            Console.Clear();
            char ifContinue;
            List<string> completeAddress = new List<string>();
            string inputValue = "";

            while (true)
            {
                foreach (string val in addressProperties)
                {
                    while (true)
                    {
                        Console.WriteLine("Podaj {0} {1}", direction, val);
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

                Console.WriteLine("Czy adres \n'{0}" +
                    "\n{1}" +
                    "\n{2}'\n" +
                    "jest poprawny? Type [y/n]", completeAddress[0], completeAddress[1], completeAddress[2]);

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

        public virtual async Task<List<string>> calculateRoute()
        {
            string[] direction = { "początkową/-y", "docelową/-y" };
            string[] addressProperties = { "miejscowość", "ulicę", "numer budynku" };

            //Klucz API do wrzucenia do oddzielnego pliku                                             !!!!TO-DO!!!!!
            var apiKey = "AIzaSyCSoBDJXMCbiCBYFIzAfT3sa_HciPuCouE";

            List<string> completeOriginAddress = getAddress(direction[0], addressProperties);
            List<string> completeDestinationAddress = getAddress(direction[1], addressProperties);
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
        //***ZARZĄDZNIE MENU***
        public virtual void showOptions()
        {
            Console.WriteLine("Wybierz interesującą Ciebie opcję:" +
                "\na - dodaj kurs");
        }

        public virtual async Task chooseOption()
        {
            //do zaimplementowania wykonywanie konkretnej opcji
            bool loop = true;
            while (loop)
            {
                showOptions();
                char choosenOption = Console.ReadKey().KeyChar;
                //Kolejne opcje dla centrali i ceo do dorzucenia tutaj
                switch (choosenOption)
                {
                    case 'a':
                        List<string> calculatedRoute = new List<string>();
                        calculatedRoute = await calculateRoute();
                        Console.WriteLine("Co zwraca calculateRoute?\nAdres docelowy: {0}" +
                            "\nAdres początkowy: {1}" +
                            "\nOdległość w metrach: {2}" +
                            "\nCzas w minutach: {3}", calculatedRoute[0], calculatedRoute[1], calculatedRoute[2], calculatedRoute[3]);
                        break;
                    case 'x':
                        Console.WriteLine("Kończę działanie, dobranoc...");
                        loop = false;
                        break;
                    default:
                        Console.WriteLine("Wybrales opcję, której nie ma. Dokonaj wyboru ponownie, aby zakończyć wprowadź 'x'");
                        break;
                }
            }
        }

            // Dodawanie klienta
    public virtual Client AddClient()
    {
        Console.WriteLine("Dodawanie nowego klienta:");
        Console.Write("Imię: ");
        string name = Console.ReadLine();
        Console.Write("Ocena: ");
        int rating = int.Parse(Console.ReadLine());
        Console.Write("PESEL: ");
        string pesel = Console.ReadLine();
        Console.Write("Numer karty: ");
        string cardNumber = Console.ReadLine();
        Console.Write("Liczba podróży: ");
        int numberOfTrips = int.Parse(Console.ReadLine());
        Console.Write("Czy klient jest w trakcie podróży? (true/false): ");
        bool isInTrip = bool.Parse(Console.ReadLine());
        Console.Write("Czy klient ma zaległości finansowe? (true/false): ");
        bool hasFinancialAreas = bool.Parse(Console.ReadLine());
        Console.Write("Numer telefonu: ");
        string phoneNumber = Console.ReadLine();

        Client newClient = new Client(name, rating, pesel, cardNumber, numberOfTrips, isInTrip, hasFinancialAreas, phoneNumber);
        Console.WriteLine($"Nowy klient {newClient.Name} został dodany do systemu.");
        return newClient;
    }

    // Dodawanie kierowcy
    public virtual Driver AddDriver()
    {
        Console.WriteLine("Dodawanie nowego kierowcy:");
        Console.Write("PESEL: ");
        string pesel = Console.ReadLine();
        Console.Write("Data urodzenia (YYYY-MM-DD): ");
        DateTime birthDate = DateTime.Parse(Console.ReadLine());
        Console.Write("Imię: ");
        string firstName = Console.ReadLine();
        Console.Write("Nazwisko: ");
        string lastName = Console.ReadLine();
        Console.Write("Numer prawa jazdy: ");
        string licenseNumber = Console.ReadLine();
        Console.Write("Ważność prawa jazdy (YYYY-MM-DD): ");
        DateTime licenseValidity = DateTime.Parse(Console.ReadLine());
        Console.Write("Czy kierowca jest w pracy? (true/false): ");
        bool isAtWork = bool.Parse(Console.ReadLine());
        Console.Write("Czy kierowca jest samozatrudniony? (true/false): ");
        bool isSelfEmployed = bool.Parse(Console.ReadLine());
        Console.Write("Liczba przejechanych kilometrów: ");
        double kilometersDriven = double.Parse(Console.ReadLine());
        Console.Write("Czy kierowca jest na przerwie? (true/false): ");
        bool isOnBreak = bool.Parse(Console.ReadLine());
        Console.Write("Dzienny zarobek: ");
        double dailyEarnings = double.Parse(Console.ReadLine());
        Console.Write("Tygodniowy zarobek: ");
        double weeklyEarnings = double.Parse(Console.ReadLine());
        Console.Write("Miesięczny zarobek: ");
        double monthlyEarnings = double.Parse(Console.ReadLine());
        Console.Write("Roczny zarobek: ");
        double yearlyEarnings = double.Parse(Console.ReadLine());
        Console.Write("Numer rejestracyjny firmy: ");
        string businessRegistrationNumber = Console.ReadLine();
        Console.Write("Czy kierowca posiada własny pojazd? (true/false): ");
        bool hasOwnCar = bool.Parse(Console.ReadLine());

        Driver newDriver = new Driver(pesel, birthDate, firstName, lastName, licenseNumber, licenseValidity,
            isAtWork, isSelfEmployed, kilometersDriven, isOnBreak, dailyEarnings, weeklyEarnings, monthlyEarnings,
            yearlyEarnings, businessRegistrationNumber, hasOwnCar, null); // null, bo aktualny pojazd jeszcze nie przypisany

        Console.WriteLine($"Nowy kierowca {newDriver.FirstName} {newDriver.LastName} został dodany do systemu.");
        return newDriver;
    }

    // Dodawanie pojazdu
    public virtual Vehicle AddVehicle(Driver driver)
    {
        Console.WriteLine("Dodawanie nowego pojazdu:");
        Console.Write("VIN: ");
        string vin = Console.ReadLine();
        Console.Write("Rok: ");
        int year = int.Parse(Console.ReadLine());
        Console.Write("Marka: ");
        string brand = Console.ReadLine();
        Console.Write("Model: ");
        string model = Console.ReadLine();
        Console.Write("Cena zakupu: ");
        double purchasePrice = double.Parse(Console.ReadLine());
        Console.Write("Aktualna wycena: ");
        double currentValuation = double.Parse(Console.ReadLine());
        Console.Write("Koszty utrzymania: ");
        double maintenanceCosts = double.Parse(Console.ReadLine());
        Console.Write("Średnie zużycie paliwa: ");
        double averageFuelConsumption = double.Parse(Console.ReadLine());
        Console.Write("Przebieg: ");
        double mileage = double.Parse(Console.ReadLine());
        Console.Write("Pojemność: ");
        double capacity = double.Parse(Console.ReadLine());
        Console.Write("Numer rejestracyjny: ");
        string registrationNumber = Console.ReadLine();

        Vehicle newVehicle = new Vehicle(vin, year, brand, model, purchasePrice, currentValuation,
            maintenanceCosts, averageFuelConsumption, mileage, capacity, driver, 0, 0, registrationNumber);

        driver.AssignVehicle(newVehicle);

        Console.WriteLine($"Nowy pojazd {newVehicle.Brand} {newVehicle.Model} został dodany do systemu.");
        return newVehicle;
    }
}
        //***KONIEC ZARZĄDZANIA MENU***
    }
}
