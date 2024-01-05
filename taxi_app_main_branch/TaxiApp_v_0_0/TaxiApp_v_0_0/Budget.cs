using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp_v_0_0
{
    public class Budget
    {
        public double IncomeFromTrips { get; set; }

        public void AddIncome(double amount)
        {
            IncomeFromTrips += amount;
        }

        public void SubstractExpenses(double amount)
        {
            IncomeFromTrips -= amount;
        }
    }
}