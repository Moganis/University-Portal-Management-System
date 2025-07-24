using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using UMS.StudentsInfo;

namespace UMS
{
    public partial class Login : Form
    {
        SqlConnection uml_dbms = new SqlConnection("Data Source=DESKTOP-8RGM6HO\\SQLEXPRESS;Initial Catalog=ums;Integrated Security=True;TrustServerCertificate=True");
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
           
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string username = userID.Text;
                string pass = userPass.Text;

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(pass))
                {
                    MessageBox.Show("Please enter both username and password.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string query = "select * from login where id = '" + username + "' and password = '" + pass + "'";
                SqlDataAdapter login_row = new SqlDataAdapter(query, uml_dbms);
                DataTable login_data = new DataTable();
                login_row.Fill(login_data);

                if (login_data.Rows.Count == 1)
                {
                    if (login_data.Rows[0][2].ToString() == "admin")
                    {
                        Home home = new Home();
                        home.Show();
                        this.Hide();
                    }
                    else
                    {
                        char[] newName = username.ToCharArray();
                        for (int i = 0; i < newName.Length; i++)
                        {
                            if (Char.IsLetter(newName[i]))
                            {
                                newName[i] = Char.ToUpper(newName[i]);
                            }
                        }
                        username = new string(newName);


                        if (login_data.Rows[0][3].ToString() == "Approved")
                        {
                            if (login_data.Rows[0][2].ToString() == "student")
                            {
                                StudentDashboard student = new StudentDashboard (username);
                                student.Show();
                                this.Hide();
                            }

                            else
                            {
                                FacultyDashboard facultyDashboard = new FacultyDashboard(username);
                                facultyDashboard.Show();
                                this.Hide();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Your account is Disabled. Please contact with admin", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid username or password. Please try again.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: "+ ex.Message);
            }


        }
    }
}
