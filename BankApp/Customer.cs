using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    internal class Customer
    {
        private int customer_id;
        public string SSN { get; }
        = string.Empty;
        public string Forename { get; set; }
        = String.Empty;
        public string Last_name { get; set; }
        = String.Empty;
        public string Street_address { get; set; }
        = String.Empty;
        public string City { get; set; }
        = String.Empty;
        public string State { get; set; }
        = String.Empty;
        public string ZIP { get; set; }
        = String.Empty;
        public string ZIP_4 { get; set; }
        = String.Empty;

        public Customer(int customer_id, string SSN, string Forename,
            string Last_name, string Street_address, string City, string State,
            string ZIP, string ZIP_4)
        {
            this.customer_id = customer_id;
            this.SSN = SSN;
            this.Forename = Forename;
            this.Last_name = Last_name;
            this.Street_address = Street_address;
            this.City = City;
            this.State = State;
            this.ZIP = ZIP;
            this.ZIP_4 = ZIP_4;
        }

        public string ToInsertString()
        {
            return $"INSERT INTO customer(" +
                $"ssn, forename, last_name, street_address, city, state, zip, zip_4) " +
                $"VALUES ('{SSN}','{Forename}','{Last_name}'," +
                $"'{Street_address}','{City}','{State}','{ZIP}','{ZIP_4}');";
        }
    }
}
