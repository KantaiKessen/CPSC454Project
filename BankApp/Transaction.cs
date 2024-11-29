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

        public static Transaction MakeTransaction(string type)
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

        // This can lead to negative balance.
        public List<string> ToTransactionStrings()
        {
            return new List<string>
            {
                "UPDATE account " +
                $"SET balance += (@credit - @debit) " +
                $"WHERE account_id = @accountID;",
                "INSERT INTO \"transaction\"(account_id, credit, debit) " +
                $"VALUES(@accountID, @credit, @debit);"
            };
        }

        public List<Dictionary<string, object>> ToTransactionDictionaries()
        {
            return new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"@accountID", AccountID},
                    {"@credit", credit},
                    {"@debit", debit}
                },
                new Dictionary<string, object>
                {
                    {"@accountID", AccountID},
                    {"@credit", credit},
                    {"@debit", debit}
                }
            };
        }

    }
}
