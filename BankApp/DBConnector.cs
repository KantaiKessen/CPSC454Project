using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BankApp
{
    public class DBConnector
    {
        private string connectionString;
        public DBConnector(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public static void PrintData(List<List<string>> data)
        {
            foreach (List<string> datum in data)
            {
                foreach (string s in datum)
                    Console.WriteLine(s);
                Console.WriteLine("--------------------");
            }
            Console.WriteLine("End of Data\n\n");

        }

        public List<List<string>> GetData(string query)
        {
            List<List<string>> data = new List<List<string>>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            List<string> datum = new List<string>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string row = $"{reader.GetName(i)}: {reader.GetValue(i)}";
                                datum.Add(row);
                            }
                            data.Add(datum);
                        }
                        reader.Close();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception Type: {0}", ex.GetType());
                    Console.WriteLine("Message: {0}", ex.Message);
                }
                connection.Close();
            }
            return data;
        }

        public void ExecuteSqlTransaction(List<string> commands)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                transaction = connection.BeginTransaction();
                // Must assign both transaction object and connection
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    // Display the commands to the user
                    Console.WriteLine("Please review the following commands:");
                    for (int i = 0; i < commands.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {commands[i]}");
                    }

                    // Get confirmation from the user
                    Console.Write("Do you want to execute these commands? (y/n): ");
                    string confirmation = Console.ReadLine().Trim().ToLower();

                    if (confirmation == "y")
                    {
                        foreach (string transact in commands)
                        {
                            command.CommandText = transact;
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        Console.WriteLine("Transaction successful");
                    }
                    else
                    {
                        Console.WriteLine("Transaction cancelled.");
                        transaction.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
            }
        }

        public void GetCustomers()
        {
            string query = "SELECT * FROM customer;";
            List<List<string>> data = GetData(query);
            Console.WriteLine("All Customers: ");
            PrintData(data);
        }

        public void GetCustomerByCustID(string customer)
        {
            string query = $"SELECT * FROM customer WHERE customer_id = {customer};";
            List<List<string>> data = GetData(query);
            Console.WriteLine($"Customer {customer}: ");
            PrintData(data);
        }

        public void GetAccounts()
        {
            string query = "SELECT * FROM account;";
            List<List<string>> data = GetData(query);
            Console.WriteLine($"All Accounts: ");
            PrintData(data);
        }

        public void GetAccountByAccID(string account)
        {
            string query = $"SELECT * FROM customer WHERE account_id = {account};";
            List<List<string>> data = GetData(query);
            Console.WriteLine($"All Accounts: ");
            PrintData(data);
        }

        public void GetCustomerAccounts(string customer)
        {
            string query = $"select c.customer_id, c.forename, c.last_name " +
                $"from customer as c " +
                $"left join account as a " +
                $"on c.customer_id = a.customer_id;" +
                $"where c.customer_id = {customer} ";
            List<List<string>> data = GetData(query);
            Console.WriteLine($"All Accounts: ");
            PrintData(data);
        }


    }
}
