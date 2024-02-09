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
            showOptions();
            //do zaimplementowania wykonywanie konkretnej opcji
            bool loop = true;
            while (loop)
            {
                char choosenOption;
                do
                {
                    choosenOption = Console.ReadKey(true).KeyChar; 
                    Console.WriteLine(choosenOption); 
                } while (char.IsWhiteSpace(choosenOption));

                //Kolejne opcje dla centrali i ceo do dorzucenia tutaj
                switch (choosenOption)
                {
                    case 'a':
                        List<string> calculatedRoute = new List<string>();
                        AssignRouteToDriver();
                        Thread.Sleep(1000);
                        showOptions();
                        break;
                    case 'b':
                        AddDriver();
                        showOptions();
                        break;
                    case 'c':
                        FindDriver();
                        showOptions();
                        break;
                    case 'd':
                        ListAllDrivers();
                        showOptions();
                        break;
                    case 'e':
                        RemoveDriver();
                        showOptions();
                        break;
                    case 'f':
                        Console.WriteLine($"Obecna stawka kilometrowa wartość wynosi {RatePerKm}.\nWprowadz nową stawkę albo wpisz q aby przerwać.");
                        RatePerKm = updateRate(RatePerKm);
                        Console.WriteLine($"Aktualna wartość wynosi {RatePerKm}");
                        showOptions();
                        break;
                    case 'g':
                        Console.WriteLine($"Obecna wartość wynosi {commissionRate}\nWprowadz nową wartość prowizji w formacie dziesiętnym.\nAby przerwac wprowadz q.");
                        commissionRate = updateRate(commissionRate);
                        Console.WriteLine($"Aktualna wartość wynosi {commissionRate}");
                        showOptions();
                        break;
                    case 'x':
                        Console.WriteLine("Kończę działanie, dobranoc...");
                        loop = false;
                        break;
                    default:
                        Console.WriteLine(choosenOption);
                        Console.WriteLine("Wybrales opcję, której nie ma. Dokonaj wyboru ponownie, aby zakończyć wprowadź 'x'");
                        showOptions();
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
