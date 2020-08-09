using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static Diploma.JsonHelper;
using static Diploma.MongoConnect;
using Diploma.Models;

namespace Diploma
{
    public partial class MongoEdit : Form
    {
        private readonly ObjectId objId;
        private readonly BsonDocument doc;
        private readonly int columnIndex;

        public MongoEdit(ObjectId objId, BsonDocument doc, int columnIndex)
        {
            InitializeComponent();
            this.Text = "Edit Mongo Document";
            this.objId = objId;
            this.doc = doc;
            this.columnIndex = columnIndex;
            string str = FormatJson(doc.ToString());
            richTextBox1.Text = str;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string collection = "";
            switch (columnIndex)
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
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objId);
            var newDoc = BsonDocument.Parse(richTextBox1.Text.ToString());
            try { MongoCollection.ReplaceOne(filter, newDoc); } catch (Exception) {
                MongoCollection.DeleteOne(filter);
                MongoCollection.InsertOne(newDoc);
            }
            if (newDoc.GetElement("_id").Value != objId)
            {
                using (DBConnection.ApplicationContext db = new DBConnection.ApplicationContext())
                {
                    List<Staff> staff1 = new List<Staff>();
                    Staff staff = new Staff();
                    switch (columnIndex)
                    {
                        case 2:
                            staff1 = db.Staff.Where(c => c.BusinessTrips == objId.ToString()).ToList();
                            staff = staff1[0];
                            staff.BusinessTrips = newDoc.GetElement("_id").Value.ToString();
                            break;
                        case 3:
                            staff1 = db.Staff.Where(c => c.Vacations == objId.ToString()).ToList();
                            staff = staff1[0];
                            staff.Vacations = newDoc.GetElement("_id").Value.ToString();
                            break;
                        case 4:
                            staff1 = db.Staff.Where(c => c.EntryExitMarks == objId.ToString()).ToList();
                            staff = staff1[0];
                            staff.EntryExitMarks = newDoc.GetElement("_id").Value.ToString();
                            break;
                    }
                    
                    db.Staff.Update(staff);
                    db.SaveChanges();
                }
            }
        }

        private void BusinessTripsEdit_FormClosed(object sender, FormClosedEventArgs e)
        {
            var staffForm = new StaffForm();
            staffForm.Show();
        }
    }
}
