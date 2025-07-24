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
    public partial class FacultyChangePass : Form
    {
        string login_id="";
        SqlConnection uml_dbms = new SqlConnection("Data Source=DESKTOP-8RGM6HO\\SQLEXPRESS;Initial Catalog=ums;Integrated Security=True;TrustServerCertificate=True");
        public FacultyChangePass(string username)
        {
            login_id = username;
            InitializeComponent();
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                MessageBox.Show("Write your old Password");
                return;
            }
            else
            {
                // check here old pass correct or not if no show a message and return

                string oldPass = textBox1.Text;
                string query = "select count(*) from login where id = '" + login_id + "' and password = '" + oldPass + "' ";
                SqlCommand cmd = new SqlCommand(query, uml_dbms);

                if (uml_dbms.State == ConnectionState.Closed)
                {
                    uml_dbms.Open();
                }
                int result = (int)cmd.ExecuteScalar();

                if (result <= 0 )
                {
                    MessageBox.Show("Old Password is wrong.","Wrong Password",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }

                if (textBox2.Text == "")
                {
                    MessageBox.Show("Write your new Password");
                    return;
                }
                else
                {
                    if(textBox3.Text == "")
                    {
                        MessageBox.Show("Please, write the password again.");
                        return;
                    }
                    else
                    {
                        
                        if(textBox2.Text == textBox3.Text)
                        {
                            if (textBox2.Text == oldPass)
                            {
                                MessageBox.Show("New password and old password cannot be same.");
                                return;
                            }
                            string newPass = textBox2.Text;
                            string query1 = "update login set password = '"+newPass+"'  where id = '" + login_id +"' ";
                            SqlCommand cmd1 = new SqlCommand(query1, uml_dbms);

                            if (uml_dbms.State == ConnectionState.Closed)
                            {
                                uml_dbms.Open();
                            }
                            cmd1.ExecuteNonQuery();

                            MessageBox.Show("Password change successful");
                            FacultyDashboard facultyDashboard = new FacultyDashboard(login_id);
                            facultyDashboard.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("New password & confirm password did not match");
                            return;
                        }
                    }
                }

            }
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            FacultyDashboard facultyDashboard = new FacultyDashboard(login_id);
            facultyDashboard.Show();
            this.Close();
        }
    }
}
