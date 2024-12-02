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

        public void TestConnection()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection successful");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception Type: {0}", ex.GetType());
                    Console.WriteLine("Message: {0}", ex.Message);
                }
            }
        }

        private List<List<string>> ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            var data = new List<List<string>>();

            using (var connection = new SqlConnection(connectionString))
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
            return data;
        }

        public void ExecuteSqlCommand(string sqlstring, Dictionary<string, object> parameters = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(sqlstring, connection))
                {
                    try
                    {
                        if (parameters != null)
                        {
                            foreach (var param in parameters)
                            {
                                command.Parameters.Add(param.Key, GetSqlDbType(param.Value)).Value = param.Value;
                            }
                        }

                        command.ExecuteNonQuery();
                        Console.WriteLine("Command executed successfully");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception Type: {0}", ex.GetType());
                        Console.WriteLine("Message: {0}", ex.Message);
                    }
                }
            }
        }

        public void ExecuteSqlCommands(List<string> sqlstrings, List<Dictionary<string, object>> parameters = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    using (var transaction = connection.BeginTransaction())
                    {
                        command.Transaction = transaction;
                        try
                        {
                            for (int i = 0; i < sqlstrings.Count; i++)
                            {
                                command.CommandText = sqlstrings[i];
                                if (parameters != null)
                                {
                                    foreach (var param in parameters[i])
                                    {
                                        command.Parameters.Add(param.Key, GetSqlDbType(param.Value)).Value = param.Value;
                                    }
                                }
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();
                            }
                            transaction.Commit();
                            Console.WriteLine("Commands executed successfully");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Exception Type: {0}", ex.GetType());
                            Console.WriteLine("Message: {0}", ex.Message);
                            try
                            {
                                transaction.Rollback();
                            }
                            catch (Exception ex2)
                            {
                                Console.WriteLine("Exception Type: {0}", ex2.GetType());
                                Console.WriteLine("Message: {0}", ex2.Message);
                            }
                        }
                    }
                }
            }
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

        public void TransferFunds(int fromAccountId, int toAccountId, decimal amount)
        {
            List<string> queries = new List<string>()
            {
                "UPDATE account SET balance -= @Amount WHERE account_id = @FromAccountId;",
                "UPDATE account SET balance += @Amount WHERE account_id = @ToAccountId;",
                "INSERT INTO \"transaction\" (account_id, credit, debit) VALUES (@FromAccountId, 0, @Amount);",
                "INSERT INTO \"transaction\" (account_id, credit, debit) VALUES (@ToAccountId, @Amount, 0);"
            };

            List<Dictionary<String, Object>> parameterList = new List<Dictionary<String, Object>>()
            {
                new Dictionary<String, Object>
                {
                    { "@Amount", amount },
                    { "@FromAccountId", fromAccountId }
                },
                new Dictionary<String, Object>
                {
                    { "@Amount", amount },
                    { "@ToAccountId", toAccountId }
                },
                new Dictionary<String, Object>
                {
                    { "@Amount", amount },
                    { "@FromAccountId", fromAccountId }
                },
                new Dictionary<String, Object>
                {
                    { "@Amount", amount },
                    { "@ToAccountId", toAccountId }
                }
            };

            ExecuteSqlCommands(queries, parameterList);
        }

        private SqlDbType GetSqlDbType(object value)
        {
            return value switch
            {
                int _ => SqlDbType.Int,
                string _ => SqlDbType.NVarChar,
                DateTime _ => SqlDbType.DateTime,
                bool _ => SqlDbType.Bit,
                float _ => SqlDbType.Float,
                decimal _ => SqlDbType.Decimal,
                _ => SqlDbType.Variant
            };
        }
    }
}

