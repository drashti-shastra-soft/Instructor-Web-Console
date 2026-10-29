using System;
using System.Collections.Generic;

namespace InstructorWebClient.Models;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? EmailAddress { get; set; }

    public string? MobileNumber { get; set; }

    public bool IsActive { get; set; }

    public int UserTypeId { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();

    public virtual UserType UserType { get; set; } = null!;
}
