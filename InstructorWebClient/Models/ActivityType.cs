using System;
using System.Collections.Generic;

namespace InstructorWebClient.Models;

public partial class ActivityType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public virtual ICollection<SessionActivity> SessionActivities { get; set; } = new List<SessionActivity>();
}
