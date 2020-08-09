using Diploma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Diploma
{
    public partial class EditPersonalData : Form
    {
        private readonly int staffId;
        private readonly string surname;
        private readonly string name;
        private readonly string secondname;

        public EditPersonalData(int staffId, string surname, string name, string secondname)
        {
            InitializeComponent();
            this.staffId = staffId;
            this.surname = surname;
            this.name = name;
            this.secondname = secondname;
            using (DBConnection.ApplicationContext db = new DBConnection.ApplicationContext())
            {
                PersonalData personaldata = db.PersonalData.Where(c => c.StaffId == staffId).FirstOrDefault();
                if (personaldata != null)
                {
                    if (personaldata.DateOfBirth != null) textBox1.Text = personaldata.DateOfBirth.ToString();
                    if (personaldata.Address != null) textBox2.Text = personaldata.Address.ToString();
                    if (personaldata.PhoneNumber != null) textBox3.Text = personaldata.PhoneNumber.ToString();
                }
            }
            label4.Text = surname;
            label5.Text = name;
            label6.Text = secondname;

        }

        private void EditPersonalData_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (DBConnection.ApplicationContext db = new DBConnection.ApplicationContext())
            {
                //List<PersonalData> list = db.PersonalData.ToList();
                PersonalData pdata = new PersonalData
                {
                    StaffId = staffId,
                    DateOfBirth = DateTime.Parse(textBox1.Text.ToString()),
                    Address = textBox2.Text.ToString(),
                    PhoneNumber = textBox3.Text.ToString()
                };
                db.PersonalData.Update(pdata);
                db.SaveChanges();
            }
        }

        private void EditPersonalData_FormClosed(object sender, FormClosedEventArgs e)
        {
            var staffForm = new StaffForm();
            staffForm.Show();
        }
    }
}
