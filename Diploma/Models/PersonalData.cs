using System;

namespace Diploma.Models
{
    public class PersonalData
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public Staff Staff { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
