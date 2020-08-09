using System;

namespace Diploma.Models
{
    public class Staff
    {
        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }
        public int PositionId { get; set; }
        [System.ComponentModel.Browsable(false)]
        public Positions Position { get; set; }
        public DateTime DateOfAppointment { get; set; }
        [System.ComponentModel.Browsable(false)] 
        public string BusinessTrips { get; set; }
        [System.ComponentModel.Browsable(false)] 
        public string Vacations { get; set; }
        [System.ComponentModel.Browsable(false)] 
        public string EntryExitMarks { get; set; }
    }
}
