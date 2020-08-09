using Diploma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Diploma
{
    public partial class EditPositionsList : Form
    {
        List<Positions> positionlist;
        List<Positions> oldPositions = new List<Positions>();
        BindingSource source;
        public EditPositionsList()
        {
            InitializeComponent();
            this.Text = "Positions List";
            using (DBConnection.ApplicationContext db = new DBConnection.ApplicationContext())
            {
                positionlist = db.Positions.ToList();
                int len = positionlist.Count;
                for (int i = 0; i < len; i++) {
                    Positions pos = new Positions {
                        Id=positionlist[i].Id,
                        Title = positionlist[i].Title,
                        SalaryRub = positionlist[i].SalaryRub
                    };
                    oldPositions.Add(pos);
                }
                BindingSource bindingSource = new BindingSource
                {
                    DataSource = positionlist
                };
                source = bindingSource;
                dataGridView1.DataSource = source;
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
                    int positionId = (int) dataGridView1[e.ColumnIndex + 1, e.RowIndex].Value;
                    Positions deletedPosition = new Positions();
                    foreach (Positions pos in source)
                    {
                        if (pos.Id == positionId)
                        {
                            deletedPosition = pos;
                            break;
                        }
                    }
                    try
                    {
                        db.Positions.Remove(deletedPosition);
                        db.SaveChanges();
                        source.Remove(deletedPosition);
                        positionlist = db.Positions.ToList();
                        int len = positionlist.Count;
                        oldPositions = new List<Positions>();
                        for (int i = 0; i < len; i++)
                        {
                            Positions pos = new Positions
                            {
                                Id = positionlist[i].Id,
                                Title = positionlist[i].Title,
                                SalaryRub = positionlist[i].SalaryRub
                            };
                            oldPositions.Add(pos);
                        }
                        
                    } catch (Exception)
                    {
                        
                        MessageBox.Show("There are some staff with this position");
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (DBConnection.ApplicationContext db = new DBConnection.ApplicationContext())
            {
                List<Positions> newPositions = (List<Positions>)source.DataSource;
                int length1 = oldPositions.Count;
                int length2 = newPositions.Count;
                if (length2 > length1)
                {
                    for (int i = length1; i < length2; i++)
                    {
                        db.Positions.Add(newPositions[i]);
                    }
                    for (int i = 0; i < length1; i++)
                    {
                        if (!newPositions[i].Equals(oldPositions[i]))
                        {
                            db.Positions.Update(newPositions[i]);
                        }
                    }
                }
                else if (length1 == length2)
                {
                    for (int i = 0; i < length1; i++)
                    {
                        if (!newPositions[i].Equals(oldPositions[i]))
                        {
                            db.Positions.Update(newPositions[i]);
                        }
                    }
                }
                db.SaveChanges();
            }
        }

        private void EditPositionsList_Load(object sender, EventArgs e)
        {

        }
    }
}