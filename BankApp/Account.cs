using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BankApp
{
    internal class Account
    {
        private int CustomerID { get; }
        private int AccountID { get; }

        private string AccountType { get; } = string.Empty;

        private decimal Balance { get; }

        public Account(int customerID, string accountType, decimal balance)
        {
            CustomerID = customerID;
            AccountType = accountType;
            Balance = balance;
        }

        public static Account MakeAccount()
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

        public List<string> ToInsertStrings()
        {
            string addAccountString = $"INSERT INTO account(" +
                $"customer_id, type, balance) " +
                $"VALUES ('@customer_id','@account_type','@balance');";
            string addTransactionString = $"INSERT INTO \"transaction\"(" +
                $"account_id, credit, debit) " +
                $"VALUES (IDENT_CURRENT ('account'), @balance, 0);";
            return new List<string> { addAccountString, addTransactionString };
        }

        public List<Dictionary<string, object>> ToInsertDictionaries()
        {
            return new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object>
                {
                    { "@customer_id", CustomerID },
                    { "@account_type", AccountType },
                    { "@balance", Balance }
                },
                new Dictionary<string, object>
                {
                    { "@balance", Balance }
                }
            };   
        }
    }
}
