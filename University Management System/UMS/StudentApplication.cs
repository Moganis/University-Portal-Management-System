using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace UMS.StudentsInfo
{
    public partial class StudentApplication : Form
    {
        List<string> list = new List<string>();
        string login_id = "", name = "";

        SqlConnection uml_dbms = new SqlConnection("Data Source=DESKTOP-8RGM6HO\\SQLEXPRESS;Initial Catalog=ums;Integrated Security=True;TrustServerCertificate=True");


        public StudentApplication(string login_id)
        {
            this.login_id = login_id;
            InitializeComponent();
            setIDandName();
            refresh();
           // CreateScrollableGrid();
        }

        private void homebtn_Click(object sender, EventArgs e)
        {
            StudentDashboard studentDashboard = new StudentDashboard(login_id);
            studentDashboard.Show();
            this.Close();
        }

        private void resultBtn_Click(object sender, EventArgs e)
        {
            StudentResult studentResult = new StudentResult(login_id);  
            studentResult.Show();
            this.Close();
        }

        private void registrationBtn_Click(object sender, EventArgs e)
        {
            StudentReg studentReg = new StudentReg(login_id);
            studentReg.Show();
            this.Close();
        }

        private void setIDandName()
        {
            IDLabel.Text = login_id;

            string query = "select student_name from student where student_id = '" + login_id + "'";
            SqlCommand cmd = new SqlCommand(query, uml_dbms);
            if (uml_dbms.State == ConnectionState.Closed)
            {
                uml_dbms.Open();
            }
            object result = cmd.ExecuteScalar();
            NameLabel.Text = result.ToString();
            name = NameLabel.Text;
        }

        private void cmtBoxDesign()
        {
            //cmtBox.ScrollBars = RichTextBoxScrollBars.Vertical;
        }

        public void refresh()
        {
            string query = "select * from applicant where applicant_id = '" + login_id + "' ";
            SqlDataAdapter row = new SqlDataAdapter(query, uml_dbms);
            DataTable data = new DataTable();
            row.Fill(data);
            dataGridViewApplicant.DataSource = data;
        }

        private void applyBtn_Click(object sender, EventArgs e)  // faculty apply button
        {
            if (cmtBox.Text == "")
            {
                MessageBox.Show("Write Down you reason");
            }
            else
            {
                string getText = cmtBox.Text;
                cmtBox.Text = "";

                try
                {
                    string check = "select count(*) from applicant where applicant_id = '" + login_id + "' and status = 'pending' ";
                    SqlCommand run = new SqlCommand(check, uml_dbms);

                    int result1 = Convert.ToInt32(run.ExecuteScalar());

                    if (result1 >= 5)
                    {
                        MessageBox.Show("You have already 5 pending request please try again later.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        StudentDashboard studentDashboard = new StudentDashboard(login_id);
                        studentDashboard.Show();
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
