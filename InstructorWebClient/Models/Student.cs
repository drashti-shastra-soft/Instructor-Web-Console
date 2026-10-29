using System;
using System.Collections.Generic;

namespace InstructorWebClient.Models;

public partial class Student
{
    public int Id { get; set; }

    public int? AddressId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string MobileNumber { get; set; } = null!;

    public string LicenseNumber { get; set; } = null!;

    public string Sin { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public virtual ICollection<StudentSession> StudentSessions { get; set; } = new List<StudentSession>();
}
