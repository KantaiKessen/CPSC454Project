using Microsoft.Data.SqlClient;
using System;
using System.Text.RegularExpressions;

namespace BankApp
{
    internal class Program
    {
        const string versionNumber = "version 0.01 build 20241029";
        static void Main()
        {
            string input;
            DBConnector dbconnect = new(File.ReadAllText("./secrets"));
            do
            {
                Console.Clear();
                DisplayOptions();
                input = Console.ReadLine().Trim().ToLower();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        dbconnect.GetCustomers();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;

                    case "2":
                        Console.Clear();
                        dbconnect.GetAccounts();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;

                    case "3":
                        Console.Clear();
                        dbconnect.GetCustomerByCustID(GetIntegerID());
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case "4":
                        Console.Clear();
                        dbconnect.GetAccountByAccID(GetIntegerID());
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case "5":
                        break;
                    case "6":
                        Customer newCustomer = MakeCustomer();
                        Console.WriteLine("Command: Please confirm with Y");
                        Console.WriteLine(newCustomer.ToInsertString());
                        string confirmation = Console.ReadLine();
                        if (confirmation == "y" || confirmation == "Y")
                        {
                            List<string> transactions = new List<string> { newCustomer.ToInsertString() };
                            dbconnect.ExecuteSqlTransaction(transactions);
                        }
                        else
                        {
                            break;
                        }
                        
                        break;
                    case "7":
                        break;
                    case "8":
                        break;
                    case "x":
                        break;
                    default:
                    {
                        Console.WriteLine("Please select a valid option!");
                        break;
                    }
                }
            } while (input != "x");
            Console.WriteLine("Exiting . . .");
            Thread.Sleep(2000);
            Environment.Exit(0);
        }

        static void DisplayOptions()
        {
            Console.WriteLine($"Bank App {versionNumber}");
            Console.WriteLine("Please select an option:");
            Console.WriteLine("1. View customers");
            Console.WriteLine("2. View accounts");
            Console.WriteLine("3. Select customer by customer id");
            Console.WriteLine("4. Select account by account id");
            Console.WriteLine("5. View accounts by customer");
            Console.WriteLine("6. Add customer");
            Console.WriteLine("7. Add account");
            Console.WriteLine("8. Deposit");
            Console.WriteLine("9. Withdraw");
            Console.WriteLine("x. Exit");
            Console.Write("> ");
        }

        static string GetIntegerID()
        {
            string input;
            Console.Write("Please Enter ID: ");
            input = Console.ReadLine().Trim().ToLower();
            while (String.IsNullOrWhiteSpace(input) || !input.All(char.IsAsciiDigit))
            {
                Console.WriteLine("Bad input, please try again.");
                Console.Write("Please Enter ID");
                input = Console.ReadLine().Trim().ToLower();
            }
            return input;
        }

        static Customer MakeCustomer()
        {
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
                Console.Write("Please Type in 9 digit SSN: ");
                ssn = Console.ReadLine();
                if(Regex.IsMatch(ssn, @"^\d{9}|\d{3}-\d{2}-\d{4}$"))
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
    }
}
