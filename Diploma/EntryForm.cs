using System;
using System.Windows.Forms;

namespace Diploma
{
    public partial class EntryForm : Form
    {
        public EntryForm()
        {
            InitializeComponent();
            this.Text = "Entry Form";
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var staffForm = new EditPositionsList();
            staffForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var staffForm = new StaffForm();
            staffForm.Show();
        }
    }
}
