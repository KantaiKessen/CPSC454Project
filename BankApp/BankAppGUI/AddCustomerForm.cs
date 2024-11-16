using System;
using System.Windows.Forms;

namespace BankAppGUI
{
    public partial class AddCustomerForm : Form
    {
        private DBConnector dbConnector;

        public AddCustomerForm(DBConnector dbConnector)
        {
            InitializeComponent();
            this.dbConnector = dbConnector;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var ssn = txtSSN.Text;
            var forename = txtForename.Text;
            var lastName = txtLastName.Text;
            var address = txtAddress.Text;
            var city = txtCity.Text;
            var state = txtState.Text;
            var zip = txtZip.Text;

            var customer = new Customer(0, ssn, forename, lastName, address, city, state, zip);
            dbConnector.ExecuteSqlTransaction(new List<string> { customer.ToInsertString() });
            MessageBox.Show("Customer added successfully!");
            this.Close();
        }
    }
}
