using Microsoft.Data.SqlClient;
using System;
using System.Text.RegularExpressions;

namespace BankApp
{
    internal class Program
    {
        const string versionNumber = "version 0.04 build 20241123";
        static void Main()
        {
            string input;
            DBConnector dbconnect = new(File.ReadAllText("./secrets"));
            dbconnect.TestConnection();
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
                        Customer newCustomer = Customer.MakeCustomer();
                        dbconnect.ExecuteSqlCommand(newCustomer.ToInsertString(), newCustomer.ToInsertDictionary());
                        KeyInterrupt();
                        break;
                    case "7":
                        Console.Clear();
                        transactions.Clear();
                        Account newAccount = Account.MakeAccount();
                        dbconnect.ExecuteSqlCommands(newAccount.ToInsertStrings(), newAccount.ToInsertDictionaries());
                        KeyInterrupt();
                        break;
                    case "8":
                        Console.Clear();
                        transactions.Clear();
                        Transaction creditTransaction = Transaction.MakeTransaction("Credit");
                        dbconnect.ExecuteSqlCommands(creditTransaction.ToTransactionStrings(), creditTransaction.ToTransactionDictionaries());
                        KeyInterrupt();
                        break;
                    case "9":
                        Console.Clear();
                        transactions.Clear();
                        Transaction debitTransaction = Transaction.MakeTransaction("Debit");
                        dbconnect.ExecuteSqlCommands(debitTransaction.ToTransactionStrings(), debitTransaction.ToTransactionDictionaries());
                        KeyInterrupt();
                        break;
                    case "10":
                        Console.Clear();
                        TransferFunds(dbconnect);
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

        static int GetIntegerID()
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
            return int.Parse(input);
        }

        static decimal GetDecimalAmount()
        {
            string input;
            Console.Write("Please Enter Amount: ");
            input = Console.ReadLine().Trim().ToLower();
            while (String.IsNullOrWhiteSpace(input) || !Regex.IsMatch(input, @"^\d+\.\d{0,2}$"))
            {
                Console.Clear();
                Console.WriteLine("Bad input, please try again.");
                Console.Write("Please Enter Amount: ");
                input = Console.ReadLine().Trim().ToLower();
            }
            return decimal.Parse(input);
        }

        static void KeyInterrupt()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        static void TransferFunds(DBConnector dbconnect)
        {
            Console.Clear();
            Console.WriteLine("Transfer funds");
            Console.WriteLine("Please select an account to transfer funds from:");
            int fromAccount = GetIntegerID();
            Console.WriteLine("Please select an account to transfer funds to:");
            int toAccount = GetIntegerID();
            Console.WriteLine("Please enter the amount to transfer:");
            decimal amount = GetDecimalAmount();
            dbconnect.TransferFunds(fromAccount, toAccount, amount);
        }
    }
}
