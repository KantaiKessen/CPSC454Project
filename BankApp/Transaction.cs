using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    internal class Transaction
    {
        int TransactionID { get; }
        int AccountID { get; }
        decimal credit { get; set; }
        decimal debit { get; set; }

        public Transaction(int accountID, decimal credit, decimal debit)
        {
            this.AccountID = accountID;
            this.credit = credit;
            this.debit = debit;
        }

        // This can lead to negative balance.
        public List<string> ToTransactions()
        {
            return new List<string>
            {
                "UPDATE account " +
                $"SET balance += ({credit} - {debit}) " +
                $"WHERE account_id = {AccountID};",
                "INSERT INTO \"transaction\"(account_id, credit, debit) " +
                $"VALUES({AccountID}, {credit}, {debit});"
            };
        }
    }
}
