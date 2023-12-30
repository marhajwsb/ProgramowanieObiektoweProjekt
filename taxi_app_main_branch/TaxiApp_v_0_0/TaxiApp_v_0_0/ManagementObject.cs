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
        //***KONIEC ZARZĄDZANIA MENU***
    }
}