using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeRegistration
{
    public partial class ChangePassword : Form
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            
            if (txtCurrentPsw.Text == "" || txtNewPsw.Text == "" || txtRePsw.Text == "")
            {
                MessageBox.Show("Please input your new Password and Retype Password");
            }
            else
            {
                string currentPsw = txtCurrentPsw.Text;
                string newPsw = txtNewPsw.Text;
                string retypePsw = txtRePsw.Text;
                string sql = "SELECT APassword FROM Admin where APassword='" + currentPsw + "'";
                int count = Database.Retrieve(sql).Rows.Count;

                if (count == 1)
                {
                    if (newPsw == retypePsw)
                    {
                        Database.ExecuteNonQuery("update Admin set APassword='" + newPsw + "' where APassword='" + currentPsw + "'");
                        MessageBox.Show("Update Ok");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Your Password is incorrect!!");
                    }
                }
                else if (count > 1)
                    MessageBox.Show("Dupliate");
                else
                {
                    MessageBox.Show("Current Password and DB Password is not correct");            
                    txtCurrentPsw.Focus();
                }
                
            }
            
           
        }


    }
}
