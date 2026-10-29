using System;
using System.Collections.Generic;

namespace InstructorWebClient.DataAccess.Models;

public partial class Session
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int UserId { get; set; }

    public int ClassType { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly StartTime { get; set; }

    public TimeOnly EndTime { get; set; }

    public string? Student { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public virtual User User { get; set; } = null!;
}
