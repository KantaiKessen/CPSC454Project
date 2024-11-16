using System;
using System.Windows.Forms;

namespace BankAppGUI
{
    public partial class TransactionForm : Form
    {
        private DBConnector dbConnector;
        private string transactionType;

        public TransactionForm(DBConnector dbConnector, string transactionType)
        {
            InitializeComponent();
            this.dbConnector = dbConnector;
            this.transactionType = transactionType;
            lblTransactionType.Text = transactionType;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var accountID = int.Parse(txtAccountID.Text);
            var amount = decimal.Parse(txtAmount.Text);

            Transaction transaction = transactionType == "Deposit"
                ? new Transaction(accountID, amount, 0, transactionType)
                : new Transaction(accountID, 0, amount, transactionType);

            dbConnector.ExecuteSqlTransaction(new List<string> { transaction.ToInsertString() });
            MessageBox.Show($"{transactionType} successful!");
            this.Close();
        }
    }
}
