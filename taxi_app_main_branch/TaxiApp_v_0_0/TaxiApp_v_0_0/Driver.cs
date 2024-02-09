namespace TaxiApp_v_0_0
{
    public class Driver
    {
        private static int primaryKey;
        private int driverId;
        private string name;
        private string surname;
        private long PESEL;
        private decimal earnings;

        public Driver(string name, string surname, string PESEL)
        {
            primaryKey += 1;
            driverId = primaryKey;
            this.name = name;
            this.surname = surname;
            Console.WriteLine(PESEL);
            if (long.TryParse(PESEL, out long longPESEL))
            {
                this.PESEL = longPESEL;
            }
            else
            {
                throw new ArgumentException("Podane dane nie są poprawne. Sprawdź dane i spróbuj ponownie...");
            }
            earnings = 0;
        }

        public void UpdateEarningsAndReturnCommison(string end, string start, decimal distance, decimal ratePerKm, decimal commisionRate, List<string> driverParams)
        {
            decimal commision = (distance * ratePerKm) * commisionRate;
            ManagementObject.takeCommision(end, start, commision, driverParams);
            earnings = (distance * ratePerKm) - commision;
        }

        public long _PESEL
        {
            get { return PESEL; }
            set { PESEL = value; }
        }

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
