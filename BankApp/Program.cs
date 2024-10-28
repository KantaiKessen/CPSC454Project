using Microsoft.Data.SqlClient;
using System;

namespace BankApp
{
    internal class Program
    {
        const string versionNumber = "version 0.01 build 20241028";
        static void Main(string[] args)
        {
            string input;
            DBConnector dbconnect = new DBConnector(File.ReadAllText("./secrets"));
            do
            {
                Console.Clear();
                Console.WriteLine($"Bank App {versionNumber}");
                Console.WriteLine("Please select an option:");
                Console.WriteLine("1. View customers");
                Console.WriteLine("2. View accounts");
                Console.WriteLine("3. Select customer by customer id");
                Console.WriteLine("4. Select account by account id");
                Console.WriteLine("5. View accounts by customer");
                Console.WriteLine("x. Exit");
                input = Console.ReadLine().Trim().ToLower();
            } while (input != "x");
            Console.WriteLine("Exiting . . .");
            Thread.Sleep(3000);
        }
    }


}
