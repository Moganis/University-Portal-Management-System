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
    public partial class Student : Form
    {
        SqlConnection uml_dbms = new SqlConnection("Data Source=DESKTOP-8RGM6HO\\SQLEXPRESS;Initial Catalog=ums;Integrated Security=True;TrustServerCertificate=True");
        
        string gender;
        List<string> intek_list = new List<string> { "Spring", "Summer", "Fall"}; // intake list
        List<string> dept_list = new List<string> { "CSE", "EEE", "BBA","English","Architechture","Pharmacy" }; // dept list
        List<List<string>> list = new List<List<string>>();
        List<string> statusList = new List<string> { "Approved", "Pending" };


        public Student()  
        {
            InitializeComponent();
            addDpt();
            addIntake();
            addStatus();
            refresh();
        }

        public void addStatus()
        {
            foreach (string status in statusList)
            {
                comboBoxStatus.Items.Add(status);
            }
            comboBoxStatus.DataSource = statusList;
        }

        private void homebtn_Click(object sender, EventArgs e)
        {
            Home home = new Home();
            home.Show();
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
            Courses course = new Courses();
            course.Show();
            this.Hide();
        }



        private void Reset()  
        {
            studentIDTb.Text = "";
            studentNameTb.Text = "";
            intakeCombo.SelectedItem = intakeCombo.Items[0];
            studentDOB.Text = DateTime.Now.ToString();
            dptCombo.SelectedItem = dptCombo.Items[0];
            studentPassTb.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            radioButton3.Checked = false;

        }

        public void refresh()
        {
            string query = "select * from student ";
            SqlDataAdapter row = new SqlDataAdapter(query, uml_dbms);
            DataTable data = new DataTable();
            row.Fill(data);
            dataGridViewStudent.DataSource = data;
        }



        private void addbtn_Click(object sender, EventArgs e) 
        {
            try
            {
                string login_id = studentIDTb.Text;
                string name = studentNameTb.Text;
                string intake = intakeCombo.Text;
                string dob = studentDOB.Text;
                string dept = dptCombo.Text;
                string pass = studentPassTb.Text;
                string status = comboBoxStatus.Text;
                string gender = "";

                if (radioButton1.Checked == true) { gender = "Male"; }
                else if (radioButton2.Checked == true) { gender = "Female"; }
                else if (radioButton3.Checked == true) { gender = "Others"; }

                if (login_id == "" || name == "" || intake == "" || dob == "" || dept == "" || pass == "" || (radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false))
                {
                    if (login_id == "")
                    {
                        MessageBox.Show("Please provide student id.");
                    }
                    else if (name == "")
                    {
                        MessageBox.Show("Please provide student name.");
                    }
                    else if (dob == "")
                    {
                        MessageBox.Show("Please provide student date of birth.");
                    }
                    else if ((radioButton1.Checked == false && radioButton2.Checked == false && radioButton3.Checked == false))
                    {
                        MessageBox.Show("Please select student gender.");
                    }
                    else
                    {
                        MessageBox.Show("Please provide student password.");
                    }
                }
                else
                {
                    char[] newName = login_id.ToCharArray();
                    for (int i = 0; i < newName.Length; i++)
                    {
                        if (Char.IsLetter(newName[i]))
                        {
                            newName[i] = Char.ToUpper(newName[i]);
                        }
                    }
                    login_id = new string(newName);

                    if (uml_dbms.State == ConnectionState.Closed)
                    {
                        uml_dbms.Open();
                    }
                    string query = "select * from login where id = '" + login_id + "' ";
                    SqlDataAdapter login_row = new SqlDataAdapter(query, uml_dbms);

                    DataTable login_data = new DataTable();
                    login_row.Fill(login_data);

                    if (login_data.Rows.Count == 0)
                    {
                        string insert_query = "insert into login values('" + login_id + "','" + pass + "','" + "student" + "','"+status+"')";
                        SqlCommand login_table_insert_cmd = new SqlCommand(insert_query, uml_dbms);

                        string insert_query_student = "INSERT INTO student VALUES('" + login_id + "','" + name + "','" + Convert.ToDateTime(dob).ToString("yyyy-MM-dd") + "','" + gender + "','" + dept + "', '"+status+"')";
                        SqlCommand Student_table_insert_cmd = new SqlCommand(insert_query_student, uml_dbms);

                        if (uml_dbms.State == ConnectionState.Closed)
                        {
                            uml_dbms.Open();
                        }
                        if (uml_dbms.State == ConnectionState.Open)
                        {
                            int result = login_table_insert_cmd.ExecuteNonQuery();
                            int result1 = Student_table_insert_cmd.ExecuteNonQuery();

                            if (result > 0)
                            {
                                MessageBox.Show("Successfully Added ");
                            }
                            else
                            {
                                MessageBox.Show("Not Added ");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Database Connection is not Established");
                        }
                        Reset();
                    }
                    else
                    {
                        MessageBox.Show("This id is not available.");
                    }
                }
                refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            if (studentIDTb.Text == "")
            {
                MessageBox.Show("Please Provide Student Id.");
            }
            else if (studentPassTb.Text != "")
            {
                MessageBox.Show("You cannot change the password");
            }
            else
            {
                try
                {
                    string login_id = studentIDTb.Text;
                    string name = studentNameTb.Text;
                    string intake = intakeCombo.Text;
                    string dob = studentDOB.Text;
                    string dept = dptCombo.Text;
                    string pass = studentPassTb.Text;
                    string gender = "";
                    string status = comboBoxStatus.Text;
                    int result = 0;

                    char[] newName = name.ToCharArray();
                    for (int i = 0; i < newName.Length; i++)
                    {
                        if (Char.IsLetter(newName[i]))
                        {
                            newName[i] = Char.ToUpper(newName[i]);
                        }
                    }
                    name = new string(newName);

                    if (radioButton1.Checked == true) { gender = "Male"; }
                    else if (radioButton2.Checked == true) { gender = "Female"; }
                    else if (radioButton3.Checked == true) { gender = "Others"; }

                    if (name != "")
                    {
                        string update_query = "update student set student_name =  '" + name + "' where student_id = '" + login_id + "' ";
                        SqlCommand student_table_update_cmd = new SqlCommand(update_query, uml_dbms);

                        if (uml_dbms.State == ConnectionState.Closed)
                        {
                            uml_dbms.Open();
                        }
                        if (uml_dbms.State == ConnectionState.Open)
                        {
                            result += student_table_update_cmd.ExecuteNonQuery();
                        }
                    }
                    if (dob != "")
                    {
                        string update_query = "update student set date_of_birth =  '" + Convert.ToDateTime(dob).ToString("yyyy-MM-dd") + "'  where student_id = '" + login_id + "' ";
                        SqlCommand student_table_update_cmd = new SqlCommand(update_query, uml_dbms);

                        if (uml_dbms.State == ConnectionState.Closed)
                        {
                            uml_dbms.Open();
                        }
                        if (uml_dbms.State == ConnectionState.Open)
                        {
                            result += student_table_update_cmd.ExecuteNonQuery();
                        }
                    }

                    if (gender != "")
                    {
                        string update_query = "update student set gender =  '" + gender + "'  where student_id = '" + login_id + "' ";
                        SqlCommand student_table_update_cmd = new SqlCommand(update_query, uml_dbms);

                        if (uml_dbms.State == ConnectionState.Closed)
                        {
                            uml_dbms.Open();
                        }
                        if (uml_dbms.State == ConnectionState.Open)
                        {
                            result += student_table_update_cmd.ExecuteNonQuery();
                        }
                    }

                    if (dept!= "")
                    {
                        string update_query = "update student set department =  '" + dept + "'  where student_id = '" + login_id + "' ";
                        SqlCommand student_table_update_cmd = new SqlCommand(update_query, uml_dbms);

                        if (uml_dbms.State == ConnectionState.Closed)
                        {
                            uml_dbms.Open();
                        }
                        if (uml_dbms.State == ConnectionState.Open)
                        {
                            result += student_table_update_cmd.ExecuteNonQuery();
                        }
                    }

                    if (status != "")
                    {
                        string update_query = "update student set status =  '" + status + "'  where student_id = '" + login_id + "' ";
                        SqlCommand student_table_update_cmd = new SqlCommand(update_query, uml_dbms);

                        if (uml_dbms.State == ConnectionState.Closed)
                        {
                            uml_dbms.Open();
                        }
                        if (uml_dbms.State == ConnectionState.Open)
                        {
                            result += student_table_update_cmd.ExecuteNonQuery();
                        }

                        string update_query1 = "update login set status =  '" + status + "'  where id = '" + login_id + "' ";
                        SqlCommand login_table_update_cmd = new SqlCommand(update_query1, uml_dbms);

                        if (uml_dbms.State == ConnectionState.Closed)
                        {
                            uml_dbms.Open();
                        }
                        if (uml_dbms.State == ConnectionState.Open)
                        {
                            result += login_table_update_cmd.ExecuteNonQuery();
                        }
                    }

                    if (result > 0)
                    {
                        MessageBox.Show("Successfully Updated.");

                    }
                    else
                    {
                        MessageBox.Show("This id does not exist.");
                    }

                    Reset();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            refresh();
        }

        private void deletebtn_Click(object sender, EventArgs e)
        {
            string login_id = studentIDTb.Text;

            if (string.IsNullOrEmpty(studentIDTb.Text))
            {
                MessageBox.Show("Please provide an id to delete information.");
                return;
            }

            if (uml_dbms.State == ConnectionState.Closed)
            {
                uml_dbms.Open();
            }
            string query = "select * from login where id = '" + login_id + "' ";
            SqlDataAdapter login_row = new SqlDataAdapter(query, uml_dbms);

            DataTable login_data = new DataTable();
            login_row.Fill(login_data);

            if (login_data.Rows.Count == 0)
            {
                MessageBox.Show("This id does not exist");
                return;
            }

            int result = 0;

            string delete_query = "delete from login where id = '"+login_id+"'";
            string delete_query_student = "delete from student where student_id = '"+ login_id + "'";

            SqlCommand login_table_delete_cmd = new SqlCommand(delete_query, uml_dbms);
            SqlCommand Student_table_delete_cmd = new SqlCommand(delete_query_student, uml_dbms);

            if (uml_dbms.State == ConnectionState.Closed)
            {
                uml_dbms.Open();
            }
            if (uml_dbms.State == ConnectionState.Open)
            {
                result += Student_table_delete_cmd.ExecuteNonQuery();
                if (result != 0)
                    result += login_table_delete_cmd.ExecuteNonQuery();
            }
            else
            {
                MessageBox.Show("Database Connection is not Established");
            }

            if (result > 0)
            {
                MessageBox.Show("Successfully Deleted.");
            }
            else
            {
                MessageBox.Show("This id does not exist.");
            }
            refresh();
        }




        private void addDpt() 
        {
            try
            {
                if (uml_dbms.State == ConnectionState.Closed)
                {
                    uml_dbms.Open();
                }

                string query1 = "SELECT department_name FROM department"; // Adjust table/column name as needed
                SqlCommand cmd = new SqlCommand(query1, uml_dbms);
                SqlDataReader reader = cmd.ExecuteReader();

                dptCombo.Items.Clear();

                while (reader.Read())
                {
                    dptCombo.Items.Add(reader["department_name"].ToString());
                }

                reader.Close();

                if (dptCombo.Items.Count > 0)
                {
                    dptCombo.SelectedIndex = 0; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading departments: " + ex.Message);
            }

        }

        private void addIntake() 
        {
            foreach(string item in intek_list)
            {
                intakeCombo.Items.Add(item);
            }

            intakeCombo.SelectedItem = intakeCombo.Items[0];
        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Student_Search_TextChanged(object sender, EventArgs e)
        {
            string query = "select * from Student where student_id like '%" + studentSearchBox.Text + "%' or student_name like '%" + studentSearchBox.Text + "%' ";
            SqlDataAdapter row = new SqlDataAdapter(query, uml_dbms);
            DataTable data = new DataTable();
            row.Fill(data);
            dataGridViewStudent.DataSource = data;
        }




        private void radioButton1_CheckedChanged(object sender, EventArgs e) // radio male button
        {
            
            gender = "Male";
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e) // radio female button
        {
            gender = "Female";
        }
        private void radioButton3_CheckedChanged(object sender, EventArgs e) // radio other button
        {
            gender = "Other";
        }


    }
}

