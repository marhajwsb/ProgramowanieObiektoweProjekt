namespace TaxiApp_v_0_0
{
    public class Driver
    {
        private static int primaryKey;
        private int driverId;
        private string name;
        private string surname;
        private decimal earnings;

        public Driver(string name, string surname)
        {
            primaryKey += 1;
            driverId = primaryKey;
            this.name = name;
            this.surname = surname;
            earnings = 0;
        }

        public void UpdateEarningsAndReturnCommison(string end, string start, decimal distance, decimal ratePerKm, decimal commisionRate, List<string> driverParams)
        {
            decimal commision = (distance * ratePerKm) * commisionRate;
            ManagementObject.takeCommision(end, start, commision, driverParams);
            earnings = (distance * ratePerKm) - commision;
        }

/*        public decimal GetEarnings()
        {
            return earnings;
        }*/

        public decimal _earnings
        {
            get { return earnings; }
        }

        public int _driverId
        {
            get { return driverId; }
            set { driverId = value; }
        }

        public string _name
        {
            get { return name; }
            set { name = value; }
        }

        public string _surname
        {
            get { return surname; }
            set { surname  = value; }
        }
        
    }
}
