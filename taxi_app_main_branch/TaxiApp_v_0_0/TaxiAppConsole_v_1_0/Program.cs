﻿using System;
using TaxiApp_v_0_0;

class Program
{
    static async Task Main(string[] args)
    {
        ManagementObject obiekt = new CEO();
        await obiekt.chooseOption();
        Console.ReadLine();
    }
}