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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace UMS
{
    public partial class Courses : Form
    {
        SqlConnection uml_dbms = new SqlConnection("Data Source=DESKTOP-8RGM6HO\\SQLEXPRESS;Initial Catalog=ums;Integrated Security=True;TrustServerCertificate=True");
        List<string> dept_list = new List<string> { "Faculty of Engineering", "Faculty of Science and Techonology", "Faculty of Arts", "Faculty of Business" }; // dept list
        List<List<string>> list = new List<List<string>>();
        List<string> dayList = new List<string> { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday" };
        List<string> statusList = new List<string> { "Approved", "Pending" };
        List<string> timeList = new List<string> { "08 - 10", "10 - 12", "12 - 02", "02 - 04","04 - 06" };




        public Courses()
        {
            InitializeComponent();
            addDay();
            addTime();
            addStatus();
            refresh();
        }


        public void addTime()
        {
            foreach (string time in timeList)
            {
                comboBoxTime.Items.Add(time);
            }
            comboBoxTime.DataSource = timeList;
        }

        public void addDay()
        {
            foreach (string day in dayList)
            {
                intakeComboCourse.Items.Add(day);
            }
            intakeComboCourse.DataSource = dayList;
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

        private void Reset()
        {
            courseIDTb.Text = "";
            courseNameTb.Text = "";
            courseCapacityTb.Text = "";
            courseSectionTb.Text = "";
            intakeComboCourse.SelectedItem = intakeComboCourse.Items[0];
            comboBoxTime.SelectedItem = comboBoxTime.Items[0];
            comboBoxStatus.SelectedItem = comboBoxStatus.Items[0];
        }

        public bool goodCapacity( string capacity)
        {
            if (capacity.StartsWith("-") == true)
            {
                MessageBox.Show("Capacity must be a positive value.");
                return false;
            }

            if (capacity.All(char.IsDigit) == false)
            {
                MessageBox.Show("Capacity must be a integer value.");
                return false;
            }
            capacity = capacity.TrimStart('0');

            if (string.IsNullOrEmpty(capacity) == true || capacity.Length > 4)
            {
                MessageBox.Show("Capacity must be greater than 0 and less than 500.");
                return false;
            }

            int capacityNum;
            if (!int.TryParse(capacity, out capacityNum) || capacityNum < 1 || capacityNum > 500)
            {
                MessageBox.Show("Capacity must be greater than 0 and less than 500.");
                return false;
            }

            return true;
        }

        public void refresh()
        {
            string query = "select * from course ";
            SqlDataAdapter row = new SqlDataAdapter(query, uml_dbms);
            DataTable data = new DataTable();
            row.Fill(data);
            dataGridViewCourse.DataSource = data;
        }

        //Add Button
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string  login_id = courseIDTb.Text;
                string  name = courseNameTb.Text;
                string  section = courseSectionTb.Text;
                string  capacity = courseCapacityTb.Text;
                string  time = comboBoxTime.Text;
                string  day = intakeComboCourse.Text;
                string status = comboBoxStatus.Text;


                if (login_id == "" || name == "" || section == "" || capacity == "" )
                {
                    if (login_id == "")
                    {
                        MessageBox.Show("Please provide course id.");
                    }
                    else if (name == "")
                    {
                        MessageBox.Show("Please provide course name.");
                    }
                    else if (section == "")
                    {
                        MessageBox.Show("Please provide course section.");
                    }
                    else
                    {
                        MessageBox.Show("Please provide course capacity.");
                    }
                }
                else
                {
                    if (goodCapacity(capacity) == false)
                    {
                        return;
                    }

                    char[] newName = name.ToCharArray();
                    for (int i = 0; i < newName.Length; i++)
                    {
                        if (Char.IsLetter(newName[i]))
                        {
                            newName[i] = Char.ToUpper(newName[i]);
                        }
                    }
                    name = new string(newName);

                    char[] newID = login_id.ToCharArray();
                    for (int i = 0; i < newID.Length; i++)
                    {
                        if (Char.IsLetter(newID[i]))
                        {
                            newID[i] = Char.ToUpper(newID[i]);
                        }
                    }
                    login_id = new string(newID);

                    char[] newSection = section.ToCharArray();
                    for (int i = 0; i < newSection.Length; i++)
                    {
                        if (Char.IsLetter(newSection[i]))
                        {
                            newSection[i] = Char.ToUpper(newSection[i]);
                        }
                    }
                    section= new string(newSection);


                    if (uml_dbms.State == ConnectionState.Closed)
                    {
                        uml_dbms.Open();
                    }
                    string query = "select * from course where course_id = '" + login_id + "' ";
                    SqlDataAdapter row = new SqlDataAdapter(query, uml_dbms);
                    DataTable data = new DataTable();
                    row.Fill(data);

                    string query1 = "select * from course where course_name = '" + name + "' and section = '"+section+"' " ;
                    SqlDataAdapter row1 = new SqlDataAdapter(query1, uml_dbms);
                    DataTable data1 = new DataTable();
                    row1.Fill(data1);

                    if (data.Rows.Count == 0 && data1.Rows.Count == 0)
                    {
                        string insert_query_dpt = "INSERT INTO course VALUES('" + login_id + "','" + name + "' , '"+section+"','"+capacity+"','0','"+ time +"','"+day+"','"+status+"' )";
                        SqlCommand dpt_table_insert_cmd = new SqlCommand(insert_query_dpt, uml_dbms);

                        if (uml_dbms.State == ConnectionState.Closed)
                        {
                            uml_dbms.Open();
                        }
                        if (uml_dbms.State == ConnectionState.Open)
                        {
                            int result = dpt_table_insert_cmd.ExecuteNonQuery();

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
                        if (data.Rows.Count > 0)
                        {
                            MessageBox.Show("This course id is already in use.");
                        }
                        else
                        {
                            MessageBox.Show("This Section is already in use.");
                        }
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
            if (courseIDTb.Text == "")
            {
                MessageBox.Show("Please Provide course Id.");
            }

            else
            {
                try
                {
                    string login_id = courseIDTb.Text;
                    string name = courseNameTb.Text;
                    string section = courseSectionTb.Text;
                    string capacity = courseCapacityTb.Text;
                    string day = intakeComboCourse.Text;
                    string status = comboBoxStatus.Text;
                    string time = comboBoxTime.Text;


                    int result = 0;

                    if (capacity != "")
                    {
                        if (goodCapacity(capacity) == false)
                        {
                            return;
                        }

                        string update_query = "update course set capacity =  '" + capacity + "'  where course_id = '" + login_id + "' ";
                        SqlCommand course_table_update_cmd = new SqlCommand(update_query, uml_dbms);

                        if (uml_dbms.State == ConnectionState.Closed)
                        {
                            uml_dbms.Open();
                        }
                        if (uml_dbms.State == ConnectionState.Open)
                        {
                            result += course_table_update_cmd.ExecuteNonQuery();
                        }
                    }
                    if (name != "")
                    {
                        char[] newName = name.ToCharArray();
                        for (int i = 0; i < newName.Length; i++)
                        {
                            if (Char.IsLetter(newName[i]))
                            {
                                newName[i] = Char.ToUpper(newName[i]);
                            }
                        }
                        name = new string(newName);

                        string update_query = "update course set course_name =  '" + name + "' where course_id = '" + login_id + "' ";
                        SqlCommand course_table_update_cmd = new SqlCommand(update_query, uml_dbms);

                        if (uml_dbms.State == ConnectionState.Closed)
                        {
                            uml_dbms.Open();
                        }
                        if (uml_dbms.State == ConnectionState.Open)
                        {
                            result += course_table_update_cmd.ExecuteNonQuery();
                        }
                    }

                    if (section != "")
                    {
                        string update_query = "update course set section =  '" + section + "'  where course_id = '" + login_id + "' ";
                        SqlCommand course_table_update_cmd = new SqlCommand(update_query, uml_dbms);

                        if (uml_dbms.State == ConnectionState.Closed)
                        {
                            uml_dbms.Open();
                        }
                        if (uml_dbms.State == ConnectionState.Open)
                        {
                            result += course_table_update_cmd.ExecuteNonQuery();
                        }
                    }

                    if (time != "")
                    {
                        string update_query = "update course set time =  '" + time + "'  where course_id = '" + login_id + "' ";
                        SqlCommand course_table_update_cmd = new SqlCommand(update_query, uml_dbms);

                        if (uml_dbms.State == ConnectionState.Closed)
                        {
                            uml_dbms.Open();
                        }
                        if (uml_dbms.State == ConnectionState.Open)
                        {
                            result += course_table_update_cmd.ExecuteNonQuery();
                        }
                    }

                    if (day != "")
                    {
                        string update_query = "update course set day =  '" + day + "'  where course_id = '" + login_id + "' ";
                        SqlCommand course_table_update_cmd = new SqlCommand(update_query, uml_dbms);

                        if (uml_dbms.State == ConnectionState.Closed)
                        {
                            uml_dbms.Open();
                        }
                        if (uml_dbms.State == ConnectionState.Open)
                        {
                            result += course_table_update_cmd.ExecuteNonQuery();
                        }
                    }

                    if (status != "")
                    {
                        string update_query = "update course set status =  '" + status + "'  where course_id = '" + login_id + "' ";
                        SqlCommand course_table_update_cmd = new SqlCommand(update_query, uml_dbms);

                        if (uml_dbms.State == ConnectionState.Closed)
                        {
                            uml_dbms.Open();
                        }
                        if (uml_dbms.State == ConnectionState.Open)
                        {
                            result += course_table_update_cmd.ExecuteNonQuery();
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

        private void deleteBtn_Click(object sender, EventArgs e)
        {
            string login_id = courseIDTb.Text;

            if (string.IsNullOrEmpty(courseIDTb.Text))
            {
                MessageBox.Show("Please provide an id to delete information.");
                return;
            }

            if (uml_dbms.State == ConnectionState.Closed)
            {
                uml_dbms.Open();
            }

            string query = "select * from course where course_id = '" + login_id + "' ";
            SqlDataAdapter login_row = new SqlDataAdapter(query, uml_dbms);

            DataTable login_data = new DataTable();
            login_row.Fill(login_data);

            if (login_data.Rows.Count == 0)
            {
                MessageBox.Show("This id does not exist");
                return;
            }

            string courseID = courseIDTb.Text;
            int result = 0;

            string delete_query_course = "delete from course where course_id = '" + courseID + "'";

            SqlCommand course_table_delete_cmd = new SqlCommand(delete_query_course, uml_dbms);

            if (uml_dbms.State == ConnectionState.Closed)
            {
                uml_dbms.Open();
            }
            if (uml_dbms.State == ConnectionState.Open)
            {
                result += course_table_delete_cmd.ExecuteNonQuery();
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

        private void resetBtn_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void courseSearchBox_TextChanged(object sender, EventArgs e)
        {
            string query = "select * from course where course_id like '%" + courseSearchBox.Text + "%' or course_name like '%" + courseSearchBox.Text + "%' ";
            SqlDataAdapter row = new SqlDataAdapter(query, uml_dbms);
            DataTable data = new DataTable();
            row.Fill(data);
            dataGridViewCourse.DataSource = data;
        }


    }
}
