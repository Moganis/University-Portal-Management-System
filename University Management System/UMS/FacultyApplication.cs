using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using UMS.StudentsInfo;

namespace UMS
{
    public partial class FacultyApplication : Form
    {
        List<string> list = new List<string>();
        string login_id = "",name = "";

        SqlConnection uml_dbms = new SqlConnection("Data Source=DESKTOP-8RGM6HO\\SQLEXPRESS;Initial Catalog=ums;Integrated Security=True;TrustServerCertificate=True");
        

        public FacultyApplication(string username)
        {
            login_id = username;
            InitializeComponent();
            setIDandName();
            refresh();
            CreateScrollableGrid();
        }

        private void homebtn_Click(object sender, EventArgs e)
        {
            FacultyDashboard facultyDashboard = new FacultyDashboard(login_id);
            facultyDashboard.Show();
            this.Close();
        }
        private void resultBtn_Click_1(object sender, EventArgs e)
        {
            FacultyResult facultyResult = new FacultyResult(login_id);
            facultyResult.Show();
            this.Close();
        }
        private void registrationBtn_Click(object sender, EventArgs e)
        {
            FacultyReg facultyReg = new FacultyReg(login_id);
            facultyReg.Show();
            this.Close();
        }
        private void setIDandName()
        {
            IDLabel.Text = login_id;

            string query = "select faculty_name from faculty where faculty_id = '" + login_id + "'";
            SqlCommand cmd = new SqlCommand(query, uml_dbms);
            if (uml_dbms.State == ConnectionState.Closed)
            {
                uml_dbms.Open();
            }
            object result = cmd.ExecuteScalar();
            NameLabel.Text = result.ToString();
            name = NameLabel.Text;

        }

        private void applyBtn_Click(object sender, EventArgs e) // faculty apply button
        {
            if(cmtBox.Text == "")
            {
                MessageBox.Show("Write Down you reason");
            }
            else
            {
                string getText = cmtBox.Text;
                cmtBox.Text = "";

                try
                {
                    string check = "select count(*) from applicant where applicant_id = '"+login_id+"' and status = 'pending' ";
                    SqlCommand run = new SqlCommand(check, uml_dbms);

                    int result1 = Convert.ToInt32(run.ExecuteScalar());

                    if (result1 >= 5)
                    {
                        MessageBox.Show("You have already 5 pending request please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        FacultyDashboard facultyDashboard = new FacultyDashboard(login_id);
                        facultyDashboard.Show();
                        this.Close();
                        return;
                    }




                    string query = "insert into applicant values('" + login_id + "','" + name + "','" + getText + "','Pending')";
                    SqlCommand cmd = new SqlCommand(query, uml_dbms);

                    if (uml_dbms.State == ConnectionState.Closed)
                    {
                        uml_dbms.Open();
                    }

                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        MessageBox.Show("Sent Successfully");
                    }
                    else
                    {
                        MessageBox.Show("Try again later ");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                refresh();

            }
            
        }

        public void refresh()
        {
            string query = "select * from applicant where applicant_id = '"+login_id+"' ";
            SqlDataAdapter row = new SqlDataAdapter(query, uml_dbms);
            DataTable data = new DataTable();
            row.Fill(data);
            dataGridViewApplicant.DataSource = data;
        }

        private void CreateScrollableGrid()  
        {
       
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
