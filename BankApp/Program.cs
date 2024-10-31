using Microsoft.Data.SqlClient;
using System;
using System.Text.RegularExpressions;

namespace BankApp
{
    internal class Program
    {
        const string versionNumber = "version 0.03 build 20241031";
        static void Main()
        {
            string input;
            DBConnector dbconnect = new(File.ReadAllText("./secrets"));
            do
            {
                Console.Clear();
                DisplayOptions();
                input = Console.ReadLine().Trim().ToLower();
                List<string> transactions = new List<string>();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        dbconnect.GetCustomers();
                        KeyInterrupt();
                        break;

                    case "2":
                        Console.Clear();
                        dbconnect.GetAccounts();
                        KeyInterrupt();
                        break;

                    case "3":
                        Console.Clear();
                        dbconnect.GetCustomerByCustID(GetIntegerID());
                        KeyInterrupt();
                        break;
                    case "4":
                        Console.Clear();
                        dbconnect.GetAccountByAccID(GetIntegerID());
                        KeyInterrupt();
                        break;
                    case "5":
                        Console.Clear();
                        dbconnect.GetCustomerAccounts(GetIntegerID());
                        KeyInterrupt();
                        break;
                    case "6":
                        Console.Clear();
                        transactions.Clear();
                        Customer newCustomer = MakeCustomer();
                        transactions.Add(newCustomer.ToInsertString());
                        dbconnect.ExecuteSqlTransaction(transactions);
                        KeyInterrupt();
                        break;
                    case "7":
                        Console.Clear();
                        transactions.Clear();
                        Account newAccount = MakeAccount();
                        transactions.AddRange(newAccount.ToInsertStrings()); 
                        dbconnect.ExecuteSqlTransaction(transactions);
                        KeyInterrupt();
                        break;
                    case "8":
                        Console.Clear();
                        transactions.Clear();
                        Transaction creditTransaction = MakeTransaction("Credit");
                        transactions.AddRange(creditTransaction.ToTransactions());
                        dbconnect.ExecuteSqlTransaction(transactions);
                        KeyInterrupt();
                        break;
                    case "9":
                        Console.Clear();
                        transactions.Clear();
                        Transaction debitTransaction = MakeTransaction("Debit");
                        transactions.AddRange(debitTransaction.ToTransactions());
                        dbconnect.ExecuteSqlTransaction(transactions);
                        KeyInterrupt();
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
            Console.WriteLine("5. View accounts by customer id");
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
                Console.Clear();
                Console.WriteLine("Bad input, please try again.");
                Console.Write("Please Enter ID");
                input = Console.ReadLine().Trim().ToLower();
            }
            return input;
        }

        static Customer MakeCustomer()
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
                if(Regex.IsMatch(ssn, @"^\d{9}$"))
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
        static void KeyInterrupt()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static Account MakeAccount()
        {
            int customerID = -1;
            int customerIDVerif = -2;
            string accountType = string.Empty;
            decimal balance = 0;
            decimal balanceVerif = -1;

            Console.Clear();
            Console.WriteLine("Please Type In New Account Information");
            while (true)
            {
                Console.Write("Please Type In Customer ID: ");
                string customerIDString = Console.ReadLine();
                Console.Write("Please Type In Customer ID again: ");
                string customerIDVerifString = Console.ReadLine();
                if (Int32.TryParse(customerIDString, out customerID))
                {
                    if (Int32.TryParse(customerIDVerifString, out customerIDVerif) && customerIDVerif == customerID)
                        break;
                    else
                    {
                        Console.WriteLine("IDs do not match");
                    }
                }
                 Console.WriteLine("Error, please try again.");
            }
            while (!Regex.IsMatch(accountType, @"^[Cc]hecking|[Ss]avings?$"))
            {
                Console.Write("Please type in account type (Checking/Savings): ");
                accountType = Console.ReadLine();
            }
            while (true)
            {
                Console.Write("Please Type In Starting Balance: ");
                string balanceString = Console.ReadLine();
                Console.Write("Please Type In Starting Balance again: ");
                string balanceVerifString = Console.ReadLine();
                if (Decimal.TryParse(balanceString, out balance))
                {
                    if (Decimal.TryParse(balanceVerifString, out balanceVerif) && balance == balanceVerif)
                        break;
                    else
                    {
                        Console.WriteLine("Balances do not match");
                    }
                }
                Console.WriteLine("Error, please try again.");
            }
            return new Account(customerID, accountType, balance);
        }

        static Transaction MakeTransaction(string type)
        {
            int accountID = 0;
            int accountIDVerif = -1;
            decimal credit = 0.00m;
            decimal debit = 0.00m;
            Console.Clear();
            do
            {
                Console.Write("Please type in account ID: ");
                if (!Int32.TryParse(Console.ReadLine(), out accountID))
                {
                    Console.WriteLine("Invalid input. Amount must be an account ID");
                    continue;
                }
                Console.Write("Please verify account ID: ");
                if (!Int32.TryParse(Console.ReadLine(), out accountIDVerif))
                {
                    Console.WriteLine("Invalid input. Amount must be an account ID");
                    continue;
                }
            }
            while (accountID != accountIDVerif);
            if (type == "Credit")
            {
                while (credit < 0.01m)
                {
                    Console.Write("Please type in amount to credit: ");
                    if (!Decimal.TryParse(Console.ReadLine(), out credit))
                    {
                        Console.WriteLine("Invalid input. Amount must be positive and a number.");
                        credit = 0.00m;
                    }
                }
            }
            else if (type == "Debit")
            {
                while (debit < 0.01m)
                {
                    Console.Write("Please type in amount to debit: ");
                    if (!Decimal.TryParse(Console.ReadLine(), out debit))
                    {
                        Console.WriteLine("Invalid input. Amount must be positive and a number.");
                        debit = 0.00m;
                    }
                }
            }
            else
            {
                throw new ArgumentException("Please supply a valid argument.");
            }
            return new Transaction(accountID, credit, debit);
        }
    }
}
