using System;
using System.Collections.Generic;

namespace InstructorWebClient.Models;

public partial class SessionActivity
{
    public int Id { get; set; }

    public int SessionId { get; set; }

    public int ActivityTypeId { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public virtual ActivityType ActivityType { get; set; } = null!;

    public virtual Session Session { get; set; } = null!;
}
