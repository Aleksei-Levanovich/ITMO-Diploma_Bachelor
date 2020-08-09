using Diploma.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Diploma
{
    public partial class EditPositionID : Form
    {
        Staff staff;
        public EditPositionID(int staffId)
        {
            InitializeComponent();
            this.Text = "Edit position";
            StaffId = staffId;
            using (DBConnection.ApplicationContext db = new DBConnection.ApplicationContext()) {
                IEnumerable<Positions> positionlist = db.Positions.ToList();
                dataGridView1.DataSource = positionlist;
                IEnumerable<Staff> staff1 = db.Staff.Where(c => c.Id == StaffId).ToList();
                staff = staff1.First();
                textBox1.Text = staff.PositionId.ToString();
                label2.Text = staff.Surname + " " + staff.Name;
            }
        }

        public int StaffId { get; set; }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int pid)) {
                staff.PositionId = pid;
                using (DBConnection.ApplicationContext db = new DBConnection.ApplicationContext())
                {
                    db.Entry(staff).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private void EditPositionID_FormClosed(object sender, FormClosedEventArgs e)
        {
            var staffForm = new StaffForm();
            staffForm.Show();
        }
    }
}
