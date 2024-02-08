namespace TaxiApp_v_0_0
{
    public sealed class CEO : Central, IDriverManager
    {
        public void showOptions()
        {
            Console.WriteLine("Wybierz interesującą Ciebie opcję:" +
                "\na - dodaj kurs" +
                "\nb - dodaj kierowce" +
                "\nc - znajdz kierowce" +
                "\nd - wyswietl wszystkich kierowcow" +
                "\ne - usunac kierowce" +
                "\nf - zmienic stawke kilometrowa" +
                "\ng - zmienic prowizje");
        }

        public override async Task chooseOption()
        {
            //do zaimplementowania wykonywanie konkretnej opcji
            bool loop = true;
            while (loop)
            {
                showOptions();

                char choosenOption;
                do
                {
                    choosenOption = Console.ReadKey(true).KeyChar; // Odczytaj znak bez wyświetlania go
                    Console.WriteLine(choosenOption); // Wyświetl odczytany znak
                } while (char.IsWhiteSpace(choosenOption));

                //Kolejne opcje dla centrali i ceo do dorzucenia tutaj
                switch (choosenOption)
                {
                    case 'a':
                        List<string> calculatedRoute = new List<string>();
                        AssignRouteToDriver();
                        break;
                    case 'b':
                        AddDriver();
                        break;
                    case 'c':
                        FindDriver();
                        break;
                    case 'd':
                        ListAllDrivers();
                        break;
                    case 'e':
                        RemoveDriver();
                        break;
                    case 'f':
                        Console.WriteLine($"Obecna stawka kilometrowa wartość wynosi {RatePerKm}.\nWprowadz nową stawkę albo wpisz q aby przerwać.");
                        RatePerKm = updateRate(RatePerKm);
                        Console.WriteLine($"Aktualna wartość wynosi {RatePerKm}");
                        break;
                    case 'g':
                        Console.WriteLine($"Obecna wartość wynosi {commissionRate}\nWprowadz nową wartość prowizji w formacie dziesiętnym.\nAby przerwac wprowadz q.");
                        commissionRate = updateRate(commissionRate);
                        Console.WriteLine($"Aktualna wartość wynosi {commissionRate}");
                        break;
                    case 'x':
                        Console.WriteLine("Kończę działanie, dobranoc...");
                        loop = false;
                        break;
                    default:
                        Console.WriteLine(choosenOption);
                        Console.WriteLine("Wybrales opcję, której nie ma. Dokonaj wyboru ponownie, aby zakończyć wprowadź 'x'");
                        break;
                }
                

            }
        }

        private decimal updateRate(decimal oldValue)
        {
            while (true)
            {
                string newValue = Console.ReadLine();
                if (newValue == "q")
                {
                    Console.WriteLine("Operacja przerwana.");
                    return oldValue;
                }
                try
                {
                    decimal newDecimalValue = decimal.Parse(newValue);
                    return newDecimalValue;
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Wprowadzona wartość nie jest poprawna.\nWprowadź nową wartość ponownie.");
                }
            }

        }


    }
}
