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
    public partial class FacultyResult : Form
    {
        private Panel panel; // Full-screen panel
        string login_id = "";
        SqlConnection uml_dbms = new SqlConnection("Data Source=DESKTOP-8RGM6HO\\SQLEXPRESS;Initial Catalog=ums;Integrated Security=True;TrustServerCertificate=True;");

        public FacultyResult(string username)
        {
            login_id = username;
            InitializeComponent();
            SetupScrollPanel(); // Initialize the full-form panel
            courseList(); // Add courses
        }

        List<List<string>> courses = new List<List<string>>
        {
        };

        private void SetupScrollPanel()
        {
            panel = new Panel();
            panel.Dock = DockStyle.Fill; // Make panel cover the full form
            panel.AutoScroll = true; // Enable scrolling

            this.Controls.Add(panel); // Add the panel to the form
        }

        private void courseList()
        {
            int xPos1 = 300; // X position for the first column
            int xPos2 = 550; // X position for the second column
            int yPos = 40;   // Initial Y position
            int row = 0;     // Row counter to alternate columns

            // Clear the panel before adding new courses
            panel.Controls.Clear();

            try
            {
                if (uml_dbms.State == ConnectionState.Closed)
                {
                    uml_dbms.Open();
                }

                string query = "SELECT c.course_name, c.day, c.time, c.course_id, c.section " +
                               "FROM course c " +
                               "INNER JOIN Faculty_Course fc ON c.course_id = fc.course_id " +
                               "WHERE fc.faculty_id = '" + login_id + "' AND fc.Status = 2";

                SqlCommand cmd = new SqlCommand(query, uml_dbms);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string courseName = reader["course_name"].ToString();
                    string day = reader["day"].ToString();
                    string time = reader["time"].ToString();
                    string courseId = reader["course_id"].ToString();
                    string section = reader["section"].ToString();

                    int xPos = (row % 2 == 0) ? xPos1 : xPos2; // Alternate between columns
                    int yOffset = (row / 2) * 50; // Adjust Y position for each row

                    Label label1 = new Label();
                    label1.Text = courseName + " [ " + section + " ] "; // Course Name
                    label1.AutoSize = true;
                    label1.Font = new Font("Arial", 12, FontStyle.Bold);
                    label1.Location = new Point(xPos, yPos + yOffset);
                    label1.ForeColor = Color.Blue;
                    label1.Cursor = Cursors.Hand;

                    // Attach click event to open student data
                    label1.Click += (sender, e) => Label1_Click(sender, e, courseId);

                    panel.Controls.Add(label1);

                    Label label2 = new Label();
                    label2.Text = day + " - " + time; // Day & Time
                    label2.AutoSize = true;
                    label2.Font = new Font("Arial", 10, FontStyle.Italic);
                    label2.Location = new Point(xPos, yPos + yOffset + 20);
                    label2.ForeColor = Color.DarkGray;

                    panel.Controls.Add(label2);

                    row++; // Move to the next column
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading courses: " + ex.Message);
            }
        }



        private void Label1_Click(object sender, EventArgs e, string course_Id)
        {
            StudentData studentData = new StudentData(login_id,course_Id);
            studentData.Show();
            this.Close();
        }

        private void homebtn_Click(object sender, EventArgs e)
        {
            FacultyDashboard facultyDashboard = new FacultyDashboard(login_id);
            facultyDashboard.Show();
            this.Close();
        }

        private void registrationBtn_Click(object sender, EventArgs e)
        {
            FacultyReg facultyReg = new FacultyReg(login_id);
            facultyReg.Show();
            this.Close();
        }

        private void applicationBtn_Click(object sender, EventArgs e)
        {
            FacultyApplication facultyApplication = new FacultyApplication(login_id);
            facultyApplication.Show();
            this.Close();
        }

        private void logoutBtn_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Close();
        }
    }
}
