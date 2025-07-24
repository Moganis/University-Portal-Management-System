using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace UMS
{
    public partial class Department : Form
    {
        SqlConnection uml_dbms = new SqlConnection("Data Source=DESKTOP-8RGM6HO\\SQLEXPRESS;Initial Catalog=ums;Integrated Security=True;TrustServerCertificate=True");

        List<List<string>> list = new List<List<string>>();

        public Department()
        {
            InitializeComponent();
            refresh();
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

        private void coursebtn_Click(object sender, EventArgs e)
        {
            Courses course = new Courses();
            course.Show();
            this.Hide();
        }

        private void Reset() // reset all text-box 
        {
            dptIDTb.Text = "";
            dptNameTb.Text = "";
        }

        public void refresh()
        {
            string query = "select * from department ";
            SqlDataAdapter row = new SqlDataAdapter(query, uml_dbms);
            DataTable data = new DataTable();
            row.Fill(data);
            dataGridViewDepartment.DataSource = data;
        }

        // Department add button
        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                string login_id = dptIDTb.Text;
                string name = dptNameTb.Text;


                if (login_id == "" || name == "")
                {
                    if (login_id == "")
                    {
                        MessageBox.Show("Please provide department id.");
                    }
                    else if (name == "")
                    {
                        MessageBox.Show("Please provide department name.");
                    }
                }
                else
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

                    char[] newID = login_id.ToCharArray();
                    for (int i = 0; i < newID.Length; i++)
                    {
                        if (Char.IsLetter(newID[i]))
                        {
                            newID[i] = Char.ToUpper(newID[i]);
                        }
                    }
                    login_id = new string(newID);

                    if (uml_dbms.State == ConnectionState.Closed)
                    {
                        uml_dbms.Open();
                    }
                    string query = "select * from department where department_id = '" + login_id + "' ";
                    SqlDataAdapter row = new SqlDataAdapter(query, uml_dbms);
                    DataTable data = new DataTable();
                    row.Fill(data);

                    string query1 = "select * from department where department_name = '" + name  + "' ";
                    SqlDataAdapter row1 = new SqlDataAdapter(query1, uml_dbms);
                    DataTable data1 = new DataTable();
                    row1.Fill(data1);

                    if (data.Rows.Count == 0 && data1.Rows.Count == 0)
                    {
                        string insert_query_dpt = "INSERT INTO department VALUES('" + login_id + "','" + name + "' )";
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
                        if(data.Rows.Count > 0)
                        {
                            MessageBox.Show("This department id is already in use.");
                        }
                        else
                        {
                            MessageBox.Show("This department name is already in use.");
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

        private void CreateScrollableGrid()
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void resetBtn_Click(object sender, EventArgs e) // reset all textbox value to null
        {
            dptNameTb.Text = "";
            dptIDTb.Text = "";
        }


        private void deleteBtn_Click(object sender, EventArgs e)
        {
            string login_id = dptIDTb.Text;

            if (string.IsNullOrEmpty(dptIDTb.Text))
            {
                MessageBox.Show("Please provide an id to delete information.");
                return;
            }

            if (uml_dbms.State == ConnectionState.Closed)
            {
                uml_dbms.Open();
            }

            string query = "select * from department where department_id = '" + login_id + "' ";
            SqlDataAdapter login_row = new SqlDataAdapter(query, uml_dbms);

            DataTable login_data = new DataTable();
            login_row.Fill(login_data);

            if (login_data.Rows.Count == 0)
            {
                MessageBox.Show("This id does not exist");
                return;
            }

            string dptID = dptIDTb.Text;
            int result = 0;

            string delete_query_department = "delete from department where department_id = '" + dptID + "'";

            SqlCommand Department_table_delete_cmd = new SqlCommand(delete_query_department, uml_dbms);

            if (uml_dbms.State == ConnectionState.Closed)
            {
                uml_dbms.Open();
            }
            if (uml_dbms.State == ConnectionState.Open)
            {
                result += Department_table_delete_cmd.ExecuteNonQuery();
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

        private void searchBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Search_label_Click(object sender, EventArgs e)
        {

        }

        private void departmentSearchBox_TextChanged(object sender, EventArgs e)
        {
            string query = "select * from department where department_id like '%" + departmentSearchBox.Text + "%' or department_name like '%" + departmentSearchBox.Text + "%' ";
            SqlDataAdapter row = new SqlDataAdapter(query, uml_dbms);
            DataTable data = new DataTable();
            row.Fill(data);
            dataGridViewDepartment.DataSource = data;
        }

        private void departmentIDtb(object sender, EventArgs e)
        {

        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            if (dptIDTb.Text == "")
            {
                MessageBox.Show("Please Provide department Id.");
            }
            else if (dptNameTb.Text == "")
            {
                MessageBox.Show("Please Provide department Name.");
            }
            else
            {
                try
                {
                    string login_id = dptIDTb.Text;
                    string name = dptNameTb.Text;
                    int result = 0;

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

                        string update_query = "update department set department_name =  '" + name + "' where department_id = '" + login_id + "' ";
                        SqlCommand dpt_table_update_cmd = new SqlCommand(update_query, uml_dbms);

                        if (uml_dbms.State == ConnectionState.Closed)
                        {
                            uml_dbms.Open();
                        }
                        if (uml_dbms.State == ConnectionState.Open)
                        {
                            result += dpt_table_update_cmd.ExecuteNonQuery();
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

        private void Department_Load(object sender, EventArgs e)
        {

        }
    }
}
