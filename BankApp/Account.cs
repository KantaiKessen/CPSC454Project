using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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

        public List<string> ToInsertStrings()
        {
            string addAccountString = $"INSERT INTO account(" +
                $"customer_id, type, balance) " +
                $"VALUES ('{CustomerID}','{AccountType}','{Balance}');";
            string addTransactionString = $"INSERT INTO \"transaction\"(" +
                $"account_id, credit, debit) " +
                $"VALUES (IDENT_CURRENT ('account'), {Balance}, 0);";
            return new List<string> { addAccountString, addTransactionString };
        }
    }
}
