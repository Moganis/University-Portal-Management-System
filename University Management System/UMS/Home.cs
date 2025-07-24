using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UMS
{
    public partial class Home : Form
    {
        SqlConnection uml_dbms = new SqlConnection("Data Source=DESKTOP-8RGM6HO\\SQLEXPRESS;Initial Catalog=ums;Integrated Security=True;TrustServerCertificate=True;");

        public Home()
        {
            InitializeComponent();
            totalAdmins();
            totalStudents();
            totalFacultys();
            totalDepts();
            totalCourses();
        }

        private void studentbtn_Click(object sender, EventArgs e)
        {
            Student student = new Student();
            student.Show();
            this.Hide();
        }
        private void facultybtn_Click(object sender, EventArgs e)
        {
            Teachers teachers = new Teachers();
            teachers.Show();
            this.Hide();
        }
        private void dptbtn_Click(object sender, EventArgs e)
        {
            Department department = new Department();
            department.Show();
            this.Hide();
        }
        private void coursebtn_Click(object sender, EventArgs e)
        {
            Courses courses = new Courses();
            courses.Show();
            this.Hide();
        }
        private void applicationBtn_Click(object sender, EventArgs e)
        {
            
        }



        private void totalAdmins()
        {
            string query = "select count(*) from login where role = 'admin'";
            SqlCommand cmd = new SqlCommand(query, uml_dbms);
            if (uml_dbms.State == ConnectionState.Closed)
            {
                uml_dbms.Open();
            }
            int result = (int)cmd.ExecuteScalar();
            totalAdmin.Text = result.ToString();
        }
        private void totalStudents()
        {
            string query = "select count(*) from login where role = 'student'";
            SqlCommand cmd = new SqlCommand(query, uml_dbms);
            if (uml_dbms.State == ConnectionState.Closed)
            {
                uml_dbms.Open();
            }
            int result = (int)cmd.ExecuteScalar();
            totalStudent.Text = result.ToString();
        }
        private void totalFacultys()
        {
            string query = "select count(*) from login where role = 'faculty'";
            SqlCommand cmd = new SqlCommand(query, uml_dbms);
            if (uml_dbms.State == ConnectionState.Closed)
            {
                uml_dbms.Open();
            }
            int result = (int)cmd.ExecuteScalar();
            totalFaculty.Text = result.ToString();
        }
        private void totalDepts()
        {
            string query = "select count(*) from department";
            SqlCommand cmd = new SqlCommand(query, uml_dbms);
            if (uml_dbms.State == ConnectionState.Closed)
            {
                uml_dbms.Open();
            }
            int result = (int)cmd.ExecuteScalar();
            totalDpt.Text = result.ToString();
        }
        private void totalCourses()
        {
            string query = "select count(*) from course";
            SqlCommand cmd = new SqlCommand(query, uml_dbms);
            if (uml_dbms.State == ConnectionState.Closed)
            {
                uml_dbms.Open();
            }
            int result = (int)cmd.ExecuteScalar();
            totalDpt.Text = result.ToString();
        }


        // logout button
        private void pictureBox11_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Login login = new Login();
                login.Show();
                this.Close();
            }
        }



    }
}
