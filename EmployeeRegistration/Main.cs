
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeRegistration
{
    public partial class Main : Form
    {
        string imageLocation = "";


        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            radMale.Checked = true;
            cboDept.SelectedIndex = 0;
            cboTeam.SelectedIndex = 0;
            cboSearchTeam.SelectedIndex = 0;
            cboYearFrom.SelectedIndex = 0;
            cboSalaryFrom.SelectedIndex = 0;
            radMaleUp.Checked = true;
            cboDepUp.SelectedIndex = 0;
            cboTeamUp.SelectedIndex = 0;
            radMaleDel.Checked = true;
            cboDepDel.SelectedIndex = 0;
            cboTeamDel.SelectedIndex = 0;

            this.tabControl1.TabPages.Remove(PageUpdate);
            this.tabControl1.TabPages.Remove(PageDelete);
            this.tabControl1.TabPages.Remove(PageChangePsw);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string Name = txtName.Text;
            string Nrc = txtNrc.Text;
            string DOB = dateTimeBD.Value.ToShortDateString();

            string strGender = "";
            if (radMale.Checked)
                strGender = radMale.Text;
            else
                strGender = radFemale.Text;

            string JoinDate = dateTimeJoinDate.Value.ToShortDateString();
            string Department = cboDept.Items[cboDept.SelectedIndex].ToString();
            string Team = cboTeam.Items[cboTeam.SelectedIndex].ToString();
            string Position = txtPosition.Text;
            string Phone = txtPhno.Text;
            string Address = txtAddress.Text;
            int Salary = int.Parse(txtSalary.Text);

            if (pictureRegistration.Image != null)
            {
                string sql = "INSERT INTO Registration(Ename,NRC,Birthday,Gender,Joining_Date,Department,Team,EPosition,Phone_no,Address,Salary,Picture) VALUES('" + Name + "', '" + Nrc + "', '" + DOB + "', '" + strGender + "', '" + JoinDate + "','" + Department + "','" + Team + "','" + Position + "','" + Phone + "','" + Address + "','" + Salary + "',@photo);";
                Database.ExecuteNonQuery(sql, pictureRegistration.Image);
                Clear();
            }
        }

        private void Clear()
        {
            txtName.Text = "";
            txtNrc.Text = "";
            dateTimeBD.Text = DateTime.Now.ToShortDateString();
            dateTimeJoinDate.Text = DateTime.Now.ToShortDateString();
            txtPosition.Text = "";
            txtPhno.Text = "";
            txtAddress.Text = "";
            cboDept.SelectedIndex = 0;
            cboTeam.SelectedIndex = 0;
            radFemale.Checked = false;
            radMale.Checked = true;
            pictureRegistration.Image = null;
            txtSalary.Text = "";
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog pic = new OpenFileDialog();
            pic.Filter = "JPG Files|*.jpg|PNG Files|*.png";
            if (pic.ShowDialog() == DialogResult.OK)
            {
                imageLocation = pic.FileName.ToString();
                pictureRegistration.ImageLocation = imageLocation;
            }
        }

        private void btnResetImg_Click(object sender, EventArgs e)
        {
            pictureRegistration.Image = null;
        }

        //Login
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (btnLogin.Text.EndsWith("Out"))
            {
                tabControl1.TabPages.Remove(PageUpdate);
                tabControl1.TabPages.Remove(PageDelete);
                tabControl1.TabPages.Remove(PageChangePsw);
                btnLogin.Text = "Log In";
            }
            else
            {
                string UserName = textBoxUser.Text;
                string Pass = textBoxPsw.Text;
                string sql = "SELECT UserName, APassword FROM Admin where UserName='" + UserName + "' And APassword='" + Pass + "'";
                int count = Database.Retrieve(sql).Rows.Count;

                if (count == 1)
                {
                    tabControl1.TabPages.Insert(2, PageUpdate);
                    this.PageUpdate.Show();
                    tabControl1.TabPages.Insert(3, PageDelete);
                    this.PageDelete.Show();
                    tabControl1.TabPages.Insert(4, PageChangePsw);
                    this.PageChangePsw.Show();
                    textBoxUser.Text = "";
                    textBoxPsw.Text = "";
                    btnLogin.Text = "Log Out";
                }
                else if (count > 1)
                    MessageBox.Show("Dupliate");
                else
                {
                    MessageBox.Show("Username and Password is not correct");
                    textBoxUser.Text = "";
                    textBoxPsw.Text = "";
                    textBoxUser.Focus();
                }
            }
        }

        //Search Page 
        private void btnSearchEID_Click(object sender, EventArgs e)
        {
            string sql = "select * from Registration where ID =" + txtEID.Text;
            dgResult.DataSource = Database.Retrieve(sql);
        }

        private void btnResetS_Click(object sender, EventArgs e)
        {
            txtEID.Text = "";
            dgResult.DataSource = null;
        }

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            string sql = "";
            string Name = txtNameS.Text.ToString();
            string Team = cboSearchTeam.Items[cboSearchTeam.SelectedIndex].ToString();

            if (chkName.Checked == true && chkTeam.Checked == true && chkYear.Checked == true && chkSalary.Checked == true)
            {
                string YearFrom = cboYearFrom.Items[cboYearFrom.SelectedIndex].ToString();
                string YearTo = cboYearTo.Items[cboYearTo.SelectedIndex].ToString();
                int SalaryFrom = int.Parse(cboSalaryFrom.Items[cboSalaryFrom.SelectedIndex].ToString());
                int SalaryTo = int.Parse(cboSalaryTo.Items[cboSalaryTo.SelectedIndex].ToString());

                sql = "select * from Registration where Team = '" + Team + "' and Ename like'" + Name + "%' and YEAR(Joining_Date) >='" + YearFrom + "' AND YEAR(Joining_Date)<= '" + YearTo + "' And Salary>=  " + SalaryFrom + " AND Salary<=" + SalaryTo;
            }
            else if (chkName.Checked == true && chkTeam.Checked == true && chkYear.Checked == true)
            {
                string YearFrom = cboYearFrom.Items[cboYearFrom.SelectedIndex].ToString();
                string YearTo = cboYearTo.Items[cboYearTo.SelectedIndex].ToString();

                sql = "select * from Registration where Team = '" + Team + "' and Ename like'" + Name + "%' and YEAR(Joining_Date) >='" + YearFrom + "' AND YEAR(Joining_Date)<= '" + YearTo + "' ";
            }
            else if (chkName.Checked == true && chkTeam.Checked == true && chkSalary.Checked == true)
            {
                int SalaryFrom = int.Parse(cboSalaryFrom.Items[cboSalaryFrom.SelectedIndex].ToString());
                int SalaryTo = int.Parse(cboSalaryTo.Items[cboSalaryTo.SelectedIndex].ToString());

                sql = "select * from Registration where Team = '" + Team + "' and Ename like'" + Name + "%' and  Salary>=  " + SalaryFrom + " AND Salary<=" + SalaryTo;
            }
            else if (chkName.Checked == true && chkYear.Checked == true && chkSalary.Checked == true)
            {
                string YearFrom = cboYearFrom.Items[cboYearFrom.SelectedIndex].ToString();
                string YearTo = cboYearTo.Items[cboYearTo.SelectedIndex].ToString();

                int SalaryFrom = int.Parse(cboSalaryFrom.Items[cboSalaryFrom.SelectedIndex].ToString());
                int SalaryTo = int.Parse(cboSalaryTo.Items[cboSalaryTo.SelectedIndex].ToString());

                sql = "select * from Registration where Ename like'" + Name + "%' and YEAR(Joining_Date) >='" + YearFrom + "' AND YEAR(Joining_Date)<= '" + YearTo + "' And Salary>=  " + SalaryFrom + " AND Salary<=" + SalaryTo;
            }
            else if (chkTeam.Checked == true && chkYear.Checked == true && chkSalary.Checked == true)
            {
                string YearFrom = cboYearFrom.Items[cboYearFrom.SelectedIndex].ToString();
                string YearTo = cboYearTo.Items[cboYearTo.SelectedIndex].ToString();

                int SalaryFrom = int.Parse(cboSalaryFrom.Items[cboSalaryFrom.SelectedIndex].ToString());
                int SalaryTo = int.Parse(cboSalaryTo.Items[cboSalaryTo.SelectedIndex].ToString());
                sql = "select * from Registration where Team = '" + Team + "' and YEAR(Joining_Date) >='" + YearFrom + "' AND YEAR(Joining_Date)<= '" + YearTo + "' And Salary>=  " + SalaryFrom + " AND Salary<=" + SalaryTo;
            }
            else if (chkName.Checked == true && chkTeam.Checked == true)
            {
                sql = "select * from Registration where Team = '" + Team + "' and Ename like'" + Name + "%'  ";
            }
            else if (chkName.Checked == true && chkYear.Checked == true)
            {
                string YearFrom = cboYearFrom.Items[cboYearFrom.SelectedIndex].ToString();
                string YearTo = cboYearTo.Items[cboYearTo.SelectedIndex].ToString();

                sql = "select * from Registration where Ename like'" + Name + "%' and YEAR(Joining_Date) >='" + YearFrom + "' AND YEAR(Joining_Date)<= '" + YearTo + "' ";
            }
            else if (chkName.Checked == true && chkSalary.Checked == true)
            {
                int SalaryFrom = int.Parse(cboSalaryFrom.Items[cboSalaryFrom.SelectedIndex].ToString());
                int SalaryTo = int.Parse(cboSalaryTo.Items[cboSalaryTo.SelectedIndex].ToString());
                sql = "select * from Registration where Ename like'" + Name + "%' And Salary>=  " + SalaryFrom + " AND Salary<=" + SalaryTo;
            }
            else if (chkTeam.Checked == true && chkYear.Checked == true)
            {
                string YearFrom = cboYearFrom.Items[cboYearFrom.SelectedIndex].ToString();
                string YearTo = cboYearTo.Items[cboYearTo.SelectedIndex].ToString();

                sql = "select * from Registration where Team = '" + Team + "' and YEAR(Joining_Date) >='" + YearFrom + "' AND YEAR(Joining_Date)<= '" + YearTo + "' ";
            }
            else if (chkTeam.Checked == true && chkSalary.Checked == true)
            {
                int SalaryFrom = int.Parse(cboSalaryFrom.Items[cboSalaryFrom.SelectedIndex].ToString());
                int SalaryTo = int.Parse(cboSalaryTo.Items[cboSalaryTo.SelectedIndex].ToString());
                sql = "select * from Registration where Team = '" + Team + "' And Salary>=  " + SalaryFrom + " AND Salary<=" + SalaryTo;
            }
            else if (chkYear.Checked == true && chkSalary.Checked == true)
            {
                string YearFrom = cboYearFrom.Items[cboYearFrom.SelectedIndex].ToString();
                string YearTo = cboYearTo.Items[cboYearTo.SelectedIndex].ToString();

                int SalaryFrom = int.Parse(cboSalaryFrom.Items[cboSalaryFrom.SelectedIndex].ToString());
                int SalaryTo = int.Parse(cboSalaryTo.Items[cboSalaryTo.SelectedIndex].ToString());
                sql = "select * from Registration where YEAR(Joining_Date) >='" + YearFrom + "' AND YEAR(Joining_Date)<= '" + YearTo + "' And Salary>=  " + SalaryFrom + " AND Salary<=" + SalaryTo;
            }
            else if (chkName.Checked == true)
            {
                sql = "select * from Registration where EName like '" + Name + "%' ";
            }

            else if (chkTeam.Checked == true)
            {
                sql = "select * from Registration where Team = '" + Team + "' ";
            }
            else if (chkYear.Checked == true)
            {
                string YearFrom = cboYearFrom.Items[cboYearFrom.SelectedIndex].ToString();
                string YearTo = cboYearTo.Items[cboYearTo.SelectedIndex].ToString();

                sql = "select * from Registration where YEAR(Joining_Date) >='" + YearFrom + "' AND YEAR(Joining_Date)<= '" + YearTo + "'";
            }
            else if (chkSalary.Checked == true)
            {
                int SalaryFrom = int.Parse(cboSalaryFrom.Items[cboSalaryFrom.SelectedIndex].ToString());
                int SalaryTo = int.Parse(cboSalaryTo.Items[cboSalaryTo.SelectedIndex].ToString());
                sql = "select * from Registration where Salary >=" + SalaryFrom + " AND Salary<=" + SalaryTo;
            }
            else
            {
                sql = "select * from Registration ";
            }

            dgResult.DataSource = Database.Retrieve(sql);
        }

        private void btnReset1_Click(object sender, EventArgs e)
        {
            txtNameS.Text = "";
            chkName.Checked = false;
            chkTeam.Checked = false;
            chkYear.Checked = false;
            chkSalary.Checked = false;
            cboSearchTeam.SelectedIndex = 0;
            cboYearFrom.SelectedIndex = 0;
            cboYearTo.Text = null;
            cboSalaryFrom.SelectedIndex = 0;
            cboSalaryTo.Text = null;
            dgResult.DataSource = null;
        }

        private void cboYearFrom_SelectedValueChanged(object sender, EventArgs e)
        {
            cboYearTo.Items.Clear();
            int yearFrom, yearTo;
            yearFrom = int.Parse(cboYearFrom.SelectedItem.ToString());
            for (yearTo = yearFrom + 1; yearTo <= 2017; yearTo++)
            {
                cboYearTo.Items.Add(yearTo.ToString());
            }
        }

        private void cboSalaryFrom_SelectedValueChanged(object sender, EventArgs e)
        {
            cboSalaryTo.Items.Clear();
            int salaryFrom, salaryTo;
            salaryFrom = int.Parse(cboSalaryFrom.SelectedItem.ToString());
            int Temp = salaryFrom + 100000;
            for (salaryTo = Temp; salaryTo <= 2000000; )
            {
                cboSalaryTo.Items.Add(salaryTo.ToString());
                salaryTo = salaryTo + 100000;
            }
        }

        //Update Page
        private void btnSearchUp_Click(object sender, EventArgs e)
        {
            dgResultUp.DataSource = Database.Retrieve("select * from Registration where ID =" + txtIDUp.Text);
        }

        private void dgResultUp_MouseClick(object sender, MouseEventArgs e)
        {
            DataGridViewRow dr = dgResultUp.CurrentRow;
            txtNameUp.Text = dr.Cells[1].Value.ToString();
            txtNRCUp.Text = dr.Cells[2].Value.ToString();
            dateTimeUp.Text = dr.Cells[3].Value.ToString();

            string strGender = dr.Cells[4].Value.ToString(); ;
            if (strGender == "Male")
                radMaleUp.Checked = true;
            else
                radFemaleUp.Checked = true;

            dateTimeJoinUp.Text = dr.Cells[5].Value.ToString();
            cboDepUp.Text = dr.Cells[6].Value.ToString();
            cboTeamUp.Text = dr.Cells[7].Value.ToString();
            txtPositionUp.Text = dr.Cells[8].Value.ToString();
            txtPhUp.Text = dr.Cells[9].Value.ToString();
            rtxtAddUp.Text = dr.Cells[10].Value.ToString();
            txtSalaryUp.Text = dr.Cells[11].Value.ToString();
            try
            {
                byte[] temp = (byte[])dr.Cells[12].Value;
                MemoryStream ms = new MemoryStream();
                ms.Position = 0;
                ms.Write(temp, 0, temp.Length);
                pictureUpdate.Image = Image.FromStream(ms);
            }
            catch
            { }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            dgResultUp.DataSource = Database.Retrieve("select * from Registration");
        }

        private void btnResetUp_Click(object sender, EventArgs e)
        {
            txtIDUp.Text = "";
            txtNameUp.Text = "";
            txtNRCUp.Text = "";
            dateTimeUp.Text = DateTime.Now.ToShortDateString();
            dateTimeJoinUp.Text = DateTime.Now.ToShortDateString();
            radFemaleUp.Checked = false;
            radMaleUp.Checked = true;
            cboDepUp.SelectedIndex = 0;
            cboTeamUp.SelectedIndex = 0;
            txtPositionUp.Text = "";
            txtPhUp.Text = "";
            rtxtAddUp.Text = "";
            txtSalaryUp.Text = "";
            dgResultUp.DataSource = null;
            pictureUpdate.Image = null;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int i = dgResultUp.CurrentCell.RowIndex;

            DataGridViewRow dr = dgResultUp.CurrentRow;
            int id = (int)dr.Cells[0].Value;
            string Name = txtNameUp.Text;
            string Nrc = txtNRCUp.Text;
            string DOB = dateTimeUp.Value.ToShortDateString();

            string strGender = "";
            if (radMaleUp.Checked)
                strGender = radMaleUp.Text;
            else
                strGender = radFemaleUp.Text;

            string JoinDate = dateTimeJoinUp.Value.ToShortDateString();
            string Department = cboDepUp.Items[cboDepUp.SelectedIndex].ToString();
            string Team = cboTeamUp.Items[cboTeamUp.SelectedIndex].ToString();
            string Position = txtPositionUp.Text;
            string Phone = txtPhUp.Text;
            string Address = rtxtAddUp.Text;
            int Salary = int.Parse(txtSalaryUp.Text);

            string sql = "update Registration set Ename='" + Name + "',NRC='" + Nrc + "',Birthday='" + DOB + "',Gender='" + strGender + "',Joining_Date='" + JoinDate + "',Department='" + Department + "',Team='" + Team + "',EPosition='" + Position + "',Phone_no='" + Phone + "',Address='" + Address + "',Salary='" + Salary + "' ,Picture=@photo where ID=" + id;
            Image img = pictureUpdate.Image;

            Database.ExecuteNonQuery(sql, img);

            btnAll.PerformClick();

            dgResultUp.Rows[i].Selected = true;
        }

        private void btnBrowseUpdate_Click(object sender, EventArgs e)
        {
            OpenFileDialog pic = new OpenFileDialog();
            pic.Filter = "JPG Files|*.jpg|PNG Files|*.png";
            if (pic.ShowDialog() == DialogResult.OK)
            {
                imageLocation = pic.FileName.ToString();
                pictureUpdate.ImageLocation = imageLocation;
            }
        }

        //Page Delete
        private void btnSearchDel_Click(object sender, EventArgs e)
        {
            dgResultDel.DataSource = Database.Retrieve("select * from Registration where ID =" + txtIDDelete.Text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtIDDelete.Text = "";
            txtNameDel.Text = "";
            txtNrcDel.Text = "";
            dateTimeDel.Text = DateTime.Now.ToShortDateString();
            dateTimeJoinDel.Text = DateTime.Now.ToShortDateString();
            radFemaleDel.Checked = false;
            radMaleDel.Checked = true;
            cboDepDel.SelectedIndex = 0;
            cboTeamDel.SelectedIndex = 0;
            txtPositionDel.Text = "";
            txtPhDel.Text = "";
            rtxtAddDel.Text = "";
            txtSalaryDel.Text = "";
            dgResultDel.DataSource = null;
        }

        private void btnAllDel_Click(object sender, EventArgs e)
        {
            dgResultDel.DataSource = Database.Retrieve("select * from Registration");
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string sql = "delete from Registration where ID=" + dgResultDel.CurrentRow.Cells[0].Value;
            Database.ExecuteNonQuery(sql);
            btnAllDel.PerformClick();
        }

        private void dgResultDel_MouseClick(object sender, MouseEventArgs e)
        {
            DataGridViewRow dr = dgResultDel.CurrentRow;
            txtNameDel.Text = dr.Cells[1].Value.ToString();
            txtNrcDel.Text = dr.Cells[2].Value.ToString();
            dateTimeDel.Text = dr.Cells[3].Value.ToString();

            string strGender = dr.Cells[4].Value.ToString(); ;
            if (strGender == "Male")
                radMaleDel.Checked = true;
            else
                radFemaleDel.Checked = true;

            dateTimeJoinDel.Text = dr.Cells[5].Value.ToString();
            cboDepDel.Text = dr.Cells[6].Value.ToString();
            cboTeamDel.Text = dr.Cells[7].Value.ToString();
            txtPositionDel.Text = dr.Cells[8].Value.ToString();
            txtPhDel.Text = dr.Cells[9].Value.ToString();
            rtxtAddDel.Text = dr.Cells[10].Value.ToString();
            txtSalaryDel.Text = dr.Cells[11].Value.ToString();
        }

        private void btnChangePw_Click(object sender, EventArgs e)
        {
            ChangePassword changePsw = new ChangePassword();
            changePsw.Show();
        }

        private void dgResult_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
