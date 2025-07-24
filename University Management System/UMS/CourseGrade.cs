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

namespace UMS.StudentsInfo
{
    public partial class CourseGrade : Form
    {
        private string login_id;
        private string course_id;
        private Panel panel;
        SqlConnection uml_dbms = new SqlConnection("Data Source=DESKTOP-8RGM6HO\\SQLEXPRESS;Initial Catalog=ums;Integrated Security=True;TrustServerCertificate=True");


        public CourseGrade(string login_id, string course_id)
        {
            this.login_id = login_id;
            this.course_id = course_id;
            InitializeComponent();
            SetupScrollPanel();
            LoadResults();
        }

        private void SetupScrollPanel()
        {
            panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;
            this.Controls.Add(panel);
        }

        private void LoadResults()
        {
            try
            {
                if (uml_dbms.State == ConnectionState.Closed)
                {
                    uml_dbms.Open();
                }

                string query = "SELECT c.course_name, sc.marks " +
                               "FROM student_course sc " +
                               "INNER JOIN course c ON sc.course_id = c.course_id " +
                               "WHERE sc.student_id = '" + login_id + "' AND sc.course_id = '" + course_id + "'";

                SqlCommand cmd = new SqlCommand(query, uml_dbms);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read()) 
                {
                    string courseName = reader["course_name"].ToString();
                    string marks = reader["marks"].ToString().Trim();
                    string letterGrade = "-";

                    if (!string.IsNullOrEmpty(marks) && marks != "-")
                    {
                        if (int.TryParse(marks, out int numericMarks)) 
                        {
                            letterGrade = GetLetterGrade(numericMarks);
                        }
                        else
                        {
                            marks = "-"; 
                        }
                    }

                    label1.Text = courseName;
                    markLabel.Text = marks; 
                    gradeLabel.Text = letterGrade;
                }
                else
                {
                    MessageBox.Show("No results found for this course.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading results: " + ex.Message);
            }
        }

        private string GetLetterGrade(int marks)
        {
            if (marks >= 90) return "A+";
            if (marks >= 85) return "A";
            if (marks >= 80) return "B+";
            if (marks >= 75) return "B";
            if (marks >= 70) return "C+";
            if (marks >= 65) return "C";
            if (marks >= 60) return "D+";
            if (marks >= 50) return "D";
            return "F";
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            StudentResult studentResult = new StudentResult(login_id);
            studentResult.Show();
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}