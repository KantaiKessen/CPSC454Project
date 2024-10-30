using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp
{
    internal class Account
    {
        private int CustomerID { get; }
        private int AccountID { get; }

        private string AccountType { get; } = string.Empty;

        private decimal balance { get; }

        public Account()
        {

        }
    }
}
