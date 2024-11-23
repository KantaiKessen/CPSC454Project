
/*
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
            Console.WriteLine("All Customers: ");
            PrintData(data);
        }

        public void GetCustomerByCustID(int customer)
        {
            string query = $"SELECT * FROM customer WHERE customer_id = @customer;";
            List<List<string>> data = new List<List<string>>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@customer", customer);
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
            Console.WriteLine($"Customer {customer}: ");
            PrintData(data);
        }

        public void GetAccounts()
        {
            string query = "SELECT * FROM account;";
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
            Console.WriteLine($"All Accounts: ");
            PrintData(data);
        }

        public void GetAccountByAccID(int accountid)
        {
            string query = $"SELECT * FROM account WHERE account_id = @accountid;";
            List<List<string>> data = new List<List<string>>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@accountid", accountid);
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
            Console.WriteLine($"All Accounts with ID {accountid}: ");
            PrintData(data);
        }

        public void GetCustomerAccounts(int customerid)
        {
            string query = $"select c.customer_id, c.forename, c.last_name, a.account_id, a.balance " +
                $"FROM customer AS c " +
                $"LEFT JOIN account AS a " +
                $"ON c.customer_id = a.customer_id " +
                $"WHERE c.customer_id = @customerid;";
            List<List<string>> data = new List<List<string>>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@customerid", customerid);
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
            Console.WriteLine($"All Accounts: ");
            PrintData(data);
        }


    }
}
*/

using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BankApp
{
    public class DBConnector
    {
        private readonly string connectionString;

        public DBConnector(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public static void PrintData(List<List<string>> data)
        {
            foreach (var datum in data)
            {
                foreach (var s in datum)
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine("--------------------");
            }
            Console.WriteLine("End of Data\n\n");
        }

        private List<List<string>> ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            var data = new List<List<string>>();

            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.Add(param.Key, GetSqlDbType(param.Value)).Value = param.Value;
                            }
                        }

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var datum = new List<string>();
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    var row = $"{reader.GetName(i)}: {reader.GetValue(i)}";
                                    datum.Add(row);
                                }
                                data.Add(datum);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception Type: {0}", ex.GetType());
                    Console.WriteLine("Message: {0}", ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return data;
        }

        public void GetCustomers()
        {
            var query = "SELECT * FROM customer;";
            var data = ExecuteQuery(query);
            Console.WriteLine("All Customers: ");
            PrintData(data);
        }

        public void GetCustomerByCustID(int customerId)
        {
            var query = "SELECT * FROM customer WHERE customer_id = @customerId;";
            var parameters = new Dictionary<string, object>
            {
                { "@customerId", customerId }
            };
            var data = ExecuteQuery(query, parameters);
            Console.WriteLine($"Customer {customerId}: ");
            PrintData(data);
        }

        public void GetAccounts()
        {
            var query = "SELECT * FROM account;";
            var data = ExecuteQuery(query);
            Console.WriteLine("All Accounts: ");
            PrintData(data);
        }

        public void GetAccountByAccID(int accountId)
        {
            var query = "SELECT * FROM account WHERE account_id = @accountId;";
            var parameters = new Dictionary<string, object>
            {
                { "@accountId", accountId }
            };
            var data = ExecuteQuery(query, parameters);
            Console.WriteLine($"All Accounts with ID {accountId}: ");
            PrintData(data);
        }

        public void GetCustomerAccounts(int customerid)
        {
            var query = $"select c.customer_id, c.forename, c.last_name, a.account_id, a.type, a.balance " +
                $"FROM customer AS c " +
                $"LEFT JOIN account AS a " +
                $"ON c.customer_id = a.customer_id " +
                $"WHERE c.customer_id = @customerid;";
            var parameters = new Dictionary<string, object>
            {
                { "@customerid", customerid   }
            };
            var data = ExecuteQuery(query, parameters);
            Console.WriteLine($"All Accounts of customer {customerid}: ");
            PrintData(data);
        }

        public void ExecuteSqlTransaction(List<string> commands)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                using (var command = connection.CreateCommand())
                {
                    command.Connection = connection;
                    command.Transaction = transaction;

                    try
                    {
                        foreach (var transact in commands)
                        {
                            command.CommandText = transact;
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        Console.WriteLine("Transaction successful");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception Type: {0}", ex.GetType());
                        Console.WriteLine("Message: {0}", ex.Message);
                        transaction.Rollback();
                    }
                }
            }
        }

        private SqlDbType GetSqlDbType(object value)
        {
            return value switch
            {
                int _ => SqlDbType.Int,
                string _ => SqlDbType.NVarChar,
                DateTime _ => SqlDbType.DateTime,
                bool _ => SqlDbType.Bit,
                // Add mappings as needed
                _ => SqlDbType.Variant
            };
        }
    }
}

