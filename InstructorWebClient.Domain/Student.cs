using System;
using System.Linq;

namespace InstructorWebClient.Domain
{
    public class Student
    {
        public int Id { get; set; }
        public int? AddressId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string LicenseNumber { get; set; }
        public DateOnly CourseStartDate { get; set; }
        public string CourseName { get; set; }
        public string Sin { get; set; }
        public bool IsActive { get; set; } = true;
        public string Status { get; set; }
    }
}
