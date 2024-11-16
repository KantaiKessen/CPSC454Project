using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BankAppGUI
{
    public partial class Form1 : Form
    {
        private DBConnector dbConnector;

        public Form1()
        {
            InitializeComponent();
            dbConnector = new DBConnector("your_connection_string_here"); // Replace with actual connection string
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Load initial data (e.g., customers)
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            var customersData = dbConnector.GetData("SELECT * FROM customer");
            DisplayData(customersData);
        }

        private void LoadAccounts()
        {
            var accountsData = dbConnector.GetData("SELECT * FROM account");
            DisplayData(accountsData);
        }

        private void DisplayData(List<List<string>> data)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

            if (data.Count > 0)
            {
                // Add columns
                foreach (string col in data[0])
                {
                    dataGridView1.Columns.Add(col.Split(':')[0], col.Split(':')[0]);
                }

                // Add rows
                foreach (var row in data)
                {
                    var values = new List<string>();
                    foreach (var item in row)
                    {
                        values.Add(item.Split(':')[1].Trim());
                    }
                    dataGridView1.Rows.Add(values.ToArray());
                }
            }
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            var addCustomerForm = new AddCustomerForm(dbConnector);
            addCustomerForm.ShowDialog();
            LoadCustomers(); // Refresh after adding
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            var addAccountForm = new AddAccountForm(dbConnector);
            addAccountForm.ShowDialog();
            LoadAccounts(); // Refresh after adding
        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
            var depositForm = new TransactionForm(dbConnector, "Deposit");
            depositForm.ShowDialog();
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            var withdrawForm = new TransactionForm(dbConnector, "Withdraw");
            withdrawForm.ShowDialog();
        }
    }
}
