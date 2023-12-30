using System;

using TaxiApp_v_0_0;

class Program
{
    static async Task Main(string[] args)
    {
        Central obiekt = new Central();
        await obiekt.chooseOption();

        Console.ReadLine();
    }
}