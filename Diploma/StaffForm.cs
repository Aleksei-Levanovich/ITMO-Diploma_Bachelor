using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Diploma.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using static Diploma.MongoConnect;

namespace Diploma
{
    public partial class StaffForm : Form
    {
        List<Staff> stafflist;
        List<Staff> oldstaff = new List<Staff>();
        BindingSource source;
        public StaffForm()
        {
            InitializeComponent();
            this.Text = "Staff List";
            using (DBConnection.ApplicationContext db = new DBConnection.ApplicationContext())
            {
                stafflist = db.Staff.ToList();
                int len = stafflist.Count;
                for (int i = 0; i < len; i++)
                {
                    Staff staff = new Staff
                    {
                        Id = stafflist[i].Id,
                        Surname = stafflist[i].Surname,
                        Name = stafflist[i].Name,
                        SecondName = stafflist[i].SecondName,
                        PositionId = stafflist[i].PositionId,
                        DateOfAppointment = stafflist[i].DateOfAppointment
                    };
                    oldstaff.Add(staff);
                }
                BindingSource bindingSource = new BindingSource
                {
                    DataSource = stafflist
                };
                source = bindingSource;
                dataGridView1.DataSource = source;
                dataGridView1.Columns[0].ReadOnly = true;
                DataGridViewButtonColumn PersonalDataColumn = new DataGridViewButtonColumn
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    Visible = true,
                    ToolTipText = "Personal Data",
                    Text = "Edit Personal Data",
                    Name = "Personal Data",
                    HeaderText = "Personal Data",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Popup
                };
                dataGridView1.Columns.Add(PersonalDataColumn);
                DataGridViewButtonColumn PositionColumn = new DataGridViewButtonColumn
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    Visible = true,
                    ToolTipText = "Position",
                    Text = "Edit Position",
                    Name = "Position",
                    HeaderText = "Position",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Popup
                };
                dataGridView1.Columns.Add(PositionColumn);
                DataGridViewButtonColumn BusinessTrips = new DataGridViewButtonColumn
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    Visible = true,
                    ToolTipText = "Busines Trips",
                    Text = "Busines Trips",
                    Name = "BusinessTripsList",
                    HeaderText = "Busines Trips",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Popup
                };
                dataGridView1.Columns.Add(BusinessTrips);
                DataGridViewButtonColumn Vacations = new DataGridViewButtonColumn
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    Visible = true,
                    ToolTipText = "Vacations",
                    Text = "Vacations",
                    Name = "VacationsList",
                    HeaderText = "Vacations",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Popup
                };
                dataGridView1.Columns.Add(Vacations);
                DataGridViewButtonColumn EntryExitMarks = new DataGridViewButtonColumn
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    Visible = true,
                    ToolTipText = "Entry and Exit Marks",
                    Text = "Entry and Exit Marks",
                    Name = "EntryExitMarks",
                    HeaderText = "Entry and Exit Marks",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Popup
                };
                dataGridView1.Columns.Add(EntryExitMarks);
                DataGridViewButtonColumn DeleteRow = new DataGridViewButtonColumn
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    Visible = true,
                    ToolTipText = "Delete",
                    Text = "Delete",
                    Name = "Delete",
                    HeaderText = "Delete",
                    UseColumnTextForButtonValue = true,
                    FlatStyle = FlatStyle.Popup
                };
                dataGridView1.Columns.Add(DeleteRow);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                button1_Click(this, new EventArgs());
                using (DBConnection.ApplicationContext db = new DBConnection.ApplicationContext())
                {
                    int staffId = (int)dataGridView1[6, e.RowIndex].Value;
                    string surname = dataGridView1[7, e.RowIndex].Value.ToString();
                    string name = dataGridView1[8, e.RowIndex].Value.ToString();
                    string secondname = dataGridView1[9, e.RowIndex].Value.ToString();
                    var personalDataForm = new EditPersonalData(staffId, surname, name, secondname);
                    personalDataForm.Show();
                    this.Close();
                }
            }
            else if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0 && e.ColumnIndex == 1)
            {
                button1_Click(this, new EventArgs());
                using (DBConnection.ApplicationContext db = new DBConnection.ApplicationContext())
                {
                    int staffId = (int)dataGridView1[6, e.RowIndex].Value;
                    var positionForm = new EditPositionID(staffId);
                    positionForm.Show();
                    this.Close();
                }
            }
            else if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0 && e.ColumnIndex >=2 && e.ColumnIndex <=4)
            {
                string collection = "";
                switch (e.ColumnIndex)
                {
                    case 2:
                        collection = "BusinessTrips";
                        break;
                    case 3:
                        collection = "Vacations";
                        break;
                    case 4:
                        collection = "EntryExitMarks";
                        break;
                }
                var MongoCollection = ConnectMongoDB(collection);
                int staffId = (int) dataGridView1[6, e.RowIndex].Value;
                Staff selectedStaff = new Staff();
                foreach (Staff staff in stafflist) {
                    if (staff.Id == staffId)
                    {
                        selectedStaff = staff;
                        break;
                    }
                }
                string elementId="";
                if (e.ColumnIndex == 2 && selectedStaff.BusinessTrips == null)
                {
                    var doc1 = new BsonDocument();
                    MongoCollection.InsertOne(doc1);
                    var newdoc = MongoCollection.Find(doc1).First();
                    selectedStaff.BusinessTrips = newdoc.GetElement("_id").Value.ToString();
                    using (DBConnection.ApplicationContext db = new DBConnection.ApplicationContext())
                    {
                        db.Staff.Update(selectedStaff);
                        db.SaveChanges();
                        stafflist = db.Staff.ToList();
                    }
                }
                else if (e.ColumnIndex == 3 && selectedStaff.Vacations == null)
                {
                    var doc1 = new BsonDocument();
                    MongoCollection.InsertOne(doc1);
                    var newdoc = MongoCollection.Find(doc1).First();
                    selectedStaff.Vacations = newdoc.GetElement("_id").Value.ToString();
                    using (DBConnection.ApplicationContext db = new DBConnection.ApplicationContext())
                    {
                        db.Staff.Update(selectedStaff);
                        db.SaveChanges();
                        stafflist = db.Staff.ToList();
                    }
                }
                else if (e.ColumnIndex == 4 && selectedStaff.EntryExitMarks == null)
                {
                    var doc1 = new BsonDocument();
                    MongoCollection.InsertOne(doc1);
                    var newdoc = MongoCollection.Find(doc1).First();
                    selectedStaff.EntryExitMarks = newdoc.GetElement("_id").Value.ToString();
                    using (DBConnection.ApplicationContext db = new DBConnection.ApplicationContext())
                    {
                        db.Staff.Update(selectedStaff);
                        db.SaveChanges();
                        stafflist = db.Staff.ToList();
                    }
                }
                switch (e.ColumnIndex)
                {
                    case 2:
                        elementId = selectedStaff.BusinessTrips;
                        break;
                    case 3:
                        elementId = selectedStaff.Vacations;
                        break;
                    case 4:
                        elementId = selectedStaff.EntryExitMarks;
                        break;
                }
                var objId = new ObjectId(elementId);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", objId);
                var doc = MongoCollection.Find(filter).First();
                var editElementForm = new MongoEdit(objId, doc, e.ColumnIndex);
                editElementForm.Show();
                this.Close();
            }
            else if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0 && e.ColumnIndex == 5) {
                button1_Click(this, new EventArgs());
                int staffId = (int)dataGridView1[e.ColumnIndex + 1, e.RowIndex].Value;
                Staff deletedStaff = new Staff();
                foreach (Staff staff in source)
                {
                    if (staff.Id == staffId)
                    {
                        deletedStaff = staff;
                        break;
                    }
                }
                using (DBConnection.ApplicationContext db = new DBConnection.ApplicationContext())
                {
                    db.Staff.Remove(deletedStaff);
                    db.SaveChanges();
                    source.Remove(deletedStaff);
                    stafflist = db.Staff.ToList();
                }
                if (deletedStaff.BusinessTrips != null) {
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(deletedStaff.BusinessTrips));
                    var MongoCollection = ConnectMongoDB("BusinessTrips");
                    MongoCollection.DeleteOne(filter);
                }
                if (deletedStaff.Vacations != null) {
                    var MongoCollection = ConnectMongoDB("Vacations");
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(deletedStaff.Vacations));
                    MongoCollection.DeleteOne(filter);
                }
                if (deletedStaff.EntryExitMarks != null) {
                    var MongoCollection = ConnectMongoDB("EntryExitMarks");
                    var filter = Builders<BsonDocument>.Filter.Eq("_id", new ObjectId(deletedStaff.EntryExitMarks));
                    MongoCollection.DeleteOne(filter);
                }
                int len = stafflist.Count;
                oldstaff = new List<Staff>();
                for (int i = 0; i < len; i++)
                {
                    Staff staff = new Staff
                    {
                        Id = stafflist[i].Id,
                        Surname = stafflist[i].Surname,
                        Name = stafflist[i].Name,
                        SecondName = stafflist[i].SecondName,
                        PositionId = stafflist[i].PositionId,
                        DateOfAppointment = stafflist[i].DateOfAppointment
                    };
                    oldstaff.Add(staff);
                }
            }

        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (DBConnection.ApplicationContext db = new DBConnection.ApplicationContext())
            {
                List<Staff> newPositions = (List<Staff>)source.DataSource;
                int length1 = oldstaff.Count;
                int length2 = stafflist.Count;
                if (length2 > length1)
                {
                    for (int i = length1; i < length2; i++)
                    {
                        db.Staff.Add(stafflist[i]);
                    }
                    for (int i = 0; i < length1; i++)
                    {
                        if (!newPositions[i].Equals(oldstaff[i]))
                        {
                            db.Staff.Update(stafflist[i]);
                        }
                    }
                }
                else if (length1 == length2)
                {
                    for (int i = 0; i < length1; i++)
                    {
                        if (!newPositions[i].Equals(oldstaff[i]))
                        {
                            db.Staff.Update(stafflist[i]);
                        }
                    }
                }
                db.SaveChanges();
            }
        }
    }
}