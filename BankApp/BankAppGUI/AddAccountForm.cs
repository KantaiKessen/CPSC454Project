using System;
using System.Windows.Forms;

namespace BankAppGUI
{
    public partial class AddAccountForm : Form
    {
        private DBConnector dbConnector;

        public AddAccountForm(DBConnector dbConnector)
        {
            InitializeComponent();
            this.dbConnector = dbConnector;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var customerID = int.Parse(txtCustomerID.Text);
            var accountType = txtAccountType.Text;
            var balance = decimal.Parse(txtBalance.Text);

            var account = new Account(customerID, accountType, balance);
            dbConnector.ExecuteSqlTransaction(account.ToInsertStrings());
            MessageBox.Show("Account added successfully!");
            this.Close();
        }
    }
}
