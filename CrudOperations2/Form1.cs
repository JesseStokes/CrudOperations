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

namespace CrudOperations2
{
    public partial class CRUD_Operations : Form
    {
        public CRUD_Operations()
        {
            InitializeComponent();
        }

        SqlConnection con = new SqlConnection("Data Source =DESKTOP-XXXXXXX;Initial Catalog =CrudOperations2;Integrated Security = True");
        public int StudentID;

        private void CRUD_Operations_Load(object sender, EventArgs e)
        {
            GetStudentsRecord();
        }

        private void GetStudentsRecord()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Students", con);
            DataTable dt = new DataTable();

            con.Open();

            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();

            StudentRecordDataGridView.DataSource = dt;
        }

        private void btInsert_Click(object sender, EventArgs e)
        {
            if (IsValid())
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Students VALUES (@FirstName, @LastName, @RollNumber, @Address, @Phone)", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@FirstName", tbFirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", tbLastName.Text);
                cmd.Parameters.AddWithValue("@RollNumber", tbRollNumber.Text);
                cmd.Parameters.AddWithValue("@Address", tbAddress.Text);
                cmd.Parameters.AddWithValue("@Phone", tbPhone.Text);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("New Student is successfully saved in the database", "Entry Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GetStudentsRecord();
                ResetFormControls();
            }
        }

        private bool IsValid()
        {
            if(tbFirstName.Text == string.Empty)
            {
                MessageBox.Show("Student Name is required", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            ResetFormControls();
        }

        private void ResetFormControls()
        {
            StudentID = 0;
            tbFirstName.Clear();
            tbLastName.Clear();
            tbRollNumber.Clear();
            tbAddress.Clear();
            tbPhone.Clear();

            tbFirstName.Focus();
        }

        private void StudentRecordDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            StudentID = Convert.ToInt32(StudentRecordDataGridView.SelectedRows[0].Cells[0].Value);
            tbFirstName.Text = StudentRecordDataGridView.SelectedRows[0].Cells[1].Value.ToString();
            tbLastName.Text = StudentRecordDataGridView.SelectedRows[0].Cells[2].Value.ToString();
            tbRollNumber.Text = StudentRecordDataGridView.SelectedRows[0].Cells[3].Value.ToString();
            tbAddress.Text = StudentRecordDataGridView.SelectedRows[0].Cells[4].Value.ToString();
            tbPhone.Text = StudentRecordDataGridView.SelectedRows[0].Cells[5].Value.ToString();
        }

        private void btUpdate_Click(object sender, EventArgs e)
        {
            if(StudentID > 0)
            {
                SqlCommand cmd = new SqlCommand("UPDATE Students SET FirstName = @FirstName, LastName = @LastName, RollNumber = @RollNumber, Address = @Address," +
                    " Phone = @Phone WHERE StudentID = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@FirstName", tbFirstName.Text);
                cmd.Parameters.AddWithValue("@LastName", tbLastName.Text);
                cmd.Parameters.AddWithValue("@RollNumber", tbRollNumber.Text);
                cmd.Parameters.AddWithValue("@Address", tbAddress.Text);
                cmd.Parameters.AddWithValue("@Phone", tbPhone.Text);
                cmd.Parameters.AddWithValue("@ID", this.StudentID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Student information is updated", "Updated Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetStudentsRecord();
                ResetFormControls();
            }
            else
            {
                MessageBox.Show("Select a student to update their information", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if(StudentID > 0)
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM Students WHERE StudentID = @ID", con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", this.StudentID);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                MessageBox.Show("Student is deleted from the system", "Deletion Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                GetStudentsRecord();
                ResetFormControls();
            }
            else
            {
                MessageBox.Show("Select a student to delete", "Select Student", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
