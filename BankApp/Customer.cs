using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public Customer(int customer_id, string SSN, string Forename,
            string Last_name, string Street_address, string City, string State,
            string ZIP)
        {
            this.customer_id = customer_id;
            this.SSN = SSN;
            this.Forename = Forename;
            this.Last_name = Last_name;
            this.Street_address = Street_address;
            this.City = City;
            this.State = State;
            this.ZIP = ZIP;
        }

        public static Customer MakeCustomer()
        {
            Console.Clear();
            int numFieldsCorrect = 0;
            Console.WriteLine("Please Type In New Customer Information");
            string ssn = String.Empty;
            string forename = String.Empty;
            string lastName = String.Empty;
            string streetAddress = String.Empty;
            string city = String.Empty;
            string state = String.Empty;
            string zip = String.Empty;
            while (numFieldsCorrect < 3)
            {
                Console.Write("Please Type in 9 digit SSN (NUMBERS ONLY): ");
                ssn = Console.ReadLine();
                if (Regex.IsMatch(ssn, @"^\d{9}$"))
                {
                    numFieldsCorrect++;
                }
                else
                {
                    Console.WriteLine("Incorrect Format!");
                    numFieldsCorrect = 0;
                    continue;
                }
                Console.Write("Please reenter to confirm: ");
                if (Console.ReadLine() != ssn)
                {
                    Console.WriteLine("SSN does not match, restarting account creation.");
                    numFieldsCorrect = 0;
                    continue;
                }
                Console.Write("Please Type in Forename: ");
                forename = Console.ReadLine();
                Console.Write("Last Name: ");
                lastName = Console.ReadLine();
                Console.Write("Street Address: ");
                streetAddress = Console.ReadLine();
                Console.Write("City: ");
                city = Console.ReadLine();
                Console.Write("State (2 letter capitalized): ");
                state = Console.ReadLine();
                if (Regex.IsMatch(state, @"^[A-Z]{2}$"))
                {
                    numFieldsCorrect++;
                }
                else
                {
                    Console.WriteLine("The State input had incorrect format.");
                    numFieldsCorrect = 0;
                    continue;
                }
                Console.Write("ZIP (5 digits): ");
                zip = Console.ReadLine();
                if (Regex.IsMatch(zip, @"^\d{5}$"))
                {
                    numFieldsCorrect++;
                }
                else
                {
                    Console.WriteLine("Wrong Zip Format!");
                    numFieldsCorrect = 0;
                    continue;
                }
            }
            return new Customer(0, ssn, forename, lastName, streetAddress, city, state, zip);
        }

        public string ToInsertString()
        {
            return $"INSERT INTO customer(" +
                $"ssn, forename, last_name, street_address, city, state, zip) " +
                $"VALUES ('{SSN}','{Forename}','{Last_name}'," +
                $"'{Street_address}','{City}','{State}','{ZIP}');";
        }

        public Dictionary<string, object> ToInsertDictionary()
        {
            return new Dictionary<string, object>
            {
                { "@ssn", SSN },
                { "@forename", Forename },
                { "@last_name", Last_name },
                { "@street_address", Street_address },
                { "@city", City },
                { "@state", State },
                { "@zip", ZIP }
            };
        }
    }
}
