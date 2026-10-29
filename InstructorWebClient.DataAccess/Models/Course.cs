using System;
using System.Collections.Generic;

namespace InstructorWebClient.DataAccess.Models;

public partial class Course
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int TotalCumulativeHours { get; set; }

    public bool IsActive { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}
