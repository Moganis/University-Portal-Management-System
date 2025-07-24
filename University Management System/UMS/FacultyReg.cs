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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace UMS
{
    public partial class FacultyReg : Form
    {
        SqlConnection uml_dbms = new SqlConnection("Data Source=DESKTOP-8RGM6HO\\SQLEXPRESS;Initial Catalog=ums;Integrated Security=True;TrustServerCertificate=True");
        private Panel panel; // Full-screen panel
        string login_id = "";
        public FacultyReg(string username)
        {
            login_id = username;
            InitializeComponent();
            SetupScrollPanel();
            addCourses(login_id);
            courseList();
        }

        private void homebtn_Click(object sender, EventArgs e)
        {
            FacultyDashboard facultyDashboard = new FacultyDashboard(login_id);
            facultyDashboard.Show();
            this.Close();
        }

        private void resultBtn_Click(object sender, EventArgs e)
        {
            FacultyResult facultyResult = new FacultyResult(login_id);
            facultyResult.Show();
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

        private void SetupScrollPanel()
        {
            panel = new Panel();
            panel.Dock = DockStyle.Fill; // Make panel cover the full form
            panel.AutoScroll = true; // Enable scrolling

            this.Controls.Add(panel); // Add the panel to the form
        }

        List<List<string>> courses = new List<List<string>>();

        private void addCourses(string facultyId)
        {
            try
            {
                if (uml_dbms.State == ConnectionState.Closed)
                {
                    uml_dbms.Open();
                }

                string query1 = "SELECT c.course_id, c.course_name, c.section, c.day, c.time " +
                "FROM course c " +
                "LEFT JOIN Faculty_Course fc ON c.course_id = fc.course_id " +
                "AND fc.faculty_id = '" + facultyId + "' " +
                "WHERE c.status = 'Approved' " +
                "AND (fc.course_id IS NULL OR fc.Status <> 2);";

                using (SqlCommand cmd = new SqlCommand(query1, uml_dbms))
                {
                    cmd.Parameters.AddWithValue("@Faculty_Id", facultyId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader["course_id"].ToString();
                            string name = reader["course_name"].ToString();
                            string section = reader["section"].ToString();
                            string day = reader["day"].ToString();
                            string time = reader["time"].ToString(); 

                            List<string> course = new List<string>{name, section, day, time,  id};
                            courses.Add(course);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Courses: " + ex.Message);
            }
        }

        private void courseList()
        {
            int xPos = 350; // X position for the label
            int yPos = 20;  // Y position starts at the top

            foreach (var item in courses)
            {
                Label course = new Label();
                Label section = new Label();
                course.Text = item[0];
                section.Text = item[1];
                // Course Name Label
                Label label3 = new Label();
                label3.Text = course.Text + "  [ " + section.Text + " ]"; // Correct concatenation
                label3.AutoSize = true;
                label3.Font = new Font("Arial", 12, FontStyle.Bold);
                label3.Location = new Point(xPos, yPos);
                label3.ForeColor = Color.Blue;

                panel.Controls.Add(label3);

                // Day & Time Label
                Label day = new Label();
                day.Text = item[2];
                Label time = new Label();
                time.Text = item[3];
                Label id = new Label();
                id.Text = item[4];

                Label label4 = new Label();

                label4.Text = day.Text + "  [ " + time.Text+ " ]" ; 

                label4.AutoSize = true;
                label4.Font = new Font("Arial", 10, FontStyle.Italic);
                label4.Location = new Point(xPos, yPos + 25);
                label4.ForeColor = Color.DarkGray;

                panel.Controls.Add(label4);

                // CheckBox (Positioned before label3)
                CheckBox checkBox = new CheckBox();
                checkBox.Location = new Point(xPos - 30, yPos); // Positioned to the left of label3
                checkBox.AutoSize = true;

                // Store both labels in the Tag property
                checkBox.Tag = new Tuple<Label, Label, Label, Label, Label>(course,section, day, time, id);
               
                
                // Check if this course is already assigned (status = 1)
                bool isChecked = false;
                try
                {
                    if (uml_dbms.State == ConnectionState.Closed)
                    {
                        uml_dbms.Open();
                    }

                    string checkQuery = "SELECT COUNT(*) FROM Faculty_Course " +
                                        "WHERE faculty_id = '" + login_id + "' " +
                                        "AND course_id = '" + id.Text + "' " +
                                        "AND Status = 1";

                    SqlCommand checkCmd = new SqlCommand(checkQuery, uml_dbms);
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        isChecked = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking assigned courses: " + ex.Message);
                }

                checkBox.Checked = isChecked; // Set the checkbox state

                // Attach event handler to checkbox
                checkBox.CheckedChanged += CheckBox_CheckedChanged;

                panel.Controls.Add(checkBox);

                yPos += 50; // Move down for the next course
            }
        }

        // Event handler for checkbox click
        private void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                var labels = checkBox.Tag as Tuple<Label, Label, Label, Label, Label>;
                if (labels != null)
                {
                    string courseName = labels.Item1.Text;
                    string section = labels.Item2.Text;
                    string day = labels.Item3.Text;
                    string time = labels.Item4.Text;
                    string courseId = labels.Item5.Text;

                    string courseInfo = $"{courseName} - {section} - {day} - {time}";

                    if (uml_dbms.State == ConnectionState.Closed)
                    {
                        uml_dbms.Open();
                    }

                    if (checkBox.Checked)
                    {
                        string clashQuery = "SELECT COUNT(*) FROM Faculty_Course fc " +
                                             "INNER JOIN course c ON fc.course_id = c.course_id " +
                                             "WHERE fc.faculty_id = '" + login_id + "' " +
                                             "AND c.day = '" + day + "' " +
                                             "AND c.time = '" + time + "' AND c.status = 'Approved' AND (fc.status ='1' or fc.status = '2')";

                        SqlCommand clashCmd = new SqlCommand(clashQuery, uml_dbms);
                        int clashCount = (int)clashCmd.ExecuteScalar();

                        if (clashCount > 0)
                        {
                            MessageBox.Show("This course conflicts with an already assigned course.","Course Conflict", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            checkBox.Checked = false; // Uncheck the box
                            return;
                        }

                        // If no clash, insert the course
                        string insertQuery = "INSERT INTO Faculty_Course (faculty_id, course_id, status) VALUES ('" + login_id + "', '" + courseId + "','1')";
                        SqlCommand insertCmd = new SqlCommand(insertQuery, uml_dbms);
                        int result = insertCmd.ExecuteNonQuery();

                        MessageBox.Show("Course Added: " + courseInfo, "Course Added", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        // If unchecked, remove the course
                        string deleteQuery = "DELETE FROM Faculty_Course WHERE faculty_id = '" + login_id + "' AND course_id = '" + courseId + "'";
                        SqlCommand deleteCmd = new SqlCommand(deleteQuery, uml_dbms);
                        int result = deleteCmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Course Removed: " + courseInfo, "Course Removal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            return;
                        }

                    }
                }
            }
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (uml_dbms.State == ConnectionState.Closed)
                {
                    uml_dbms.Open();
                }

                string query = "UPDATE Faculty_Course SET Status = 2 WHERE faculty_id = '" + login_id + "' and status = '1'";

                SqlCommand cmd = new SqlCommand(query, uml_dbms);
                int result = cmd.ExecuteNonQuery();

                if (result > 0)
                {
                    MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    FacultyDashboard facultyDashboard = new FacultyDashboard(login_id);
                    facultyDashboard.Show();
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Select at least one course to confirm.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating courses: " + ex.Message);
            }
        }

    }
}
