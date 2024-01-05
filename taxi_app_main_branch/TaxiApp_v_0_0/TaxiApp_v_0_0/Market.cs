using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp_v_0_0
{
    public class Market
    {
        public List<Trip> PendingTrips { get; set; }
        public List<Trip> OngoingTrips { get; set; }
        public List<Driver> AvailableDrivers { get; set; }
        public double AverageTripLength { get; set; }
    }
}