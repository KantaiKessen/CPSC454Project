using Microsoft.Data.SqlClient;
using System;

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

                        dbconnect.GetCustomers();
                        break;

                    case "2":

                        dbconnect.GetAccounts();
                        break;

                    case "3":
                        dbconnect.GetCustomerByCustID(GetIntegerID());
                        break;
                    case "4":
                        dbconnect.GetAccountByAccID(GetIntegerID());
                        break;
                    case "x":
                        {
                            Console.WriteLine("Please select a valid option!");
                            break;
                        }

                    default:
                        break;
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
        }

        static string GetIntegerID()
        {
            string input;
            Console.Write("Please Enter ID");
            input = Console.ReadLine().Trim().ToLower();
            while (String.IsNullOrWhiteSpace(input) || !input.All(char.IsAsciiDigit))
            {
                Console.WriteLine("Bad input, please try again.");
                Console.Write("Please Enter ID");
                input = Console.ReadLine().Trim().ToLower();
            }
            return input;
        }
    }


}
