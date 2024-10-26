using Microsoft.Data.SqlClient;
using System;

namespace BankApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "change it yourself";
            DBConnector dbconnect = new DBConnector(connectionString);
            dbconnect.GetCustomers();
            dbconnect.GetAccounts();
            dbconnect.GetCustomerAccounts("99999");
            //Customer john = new Customer(-1, "111223333", "John", "Doe", "321 A St", "Anytown", "AA", "12344", "1234");
            //List<string> commands = new List<string>();
            //commands.Add(john.ToInsertString());
            //dbconnect.ExecuteSqlTransaction(commands);
        }
    }
}
