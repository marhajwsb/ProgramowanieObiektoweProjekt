using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp_v_0_0
{
    public class Trip
    {
        public Client Client { get; set; }
        public Driver Driver { get; set; }
        public string PaymentType { get; set; }
        public double Kilometers { get; set; }

        public void StartTrip()
        {

        }

        public void CancelTrip()
        {

        }

        public void FinishTrip()
        {

        }
    }
}