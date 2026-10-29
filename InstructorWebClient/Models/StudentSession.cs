using System;
using System.Collections.Generic;

namespace InstructorWebClient.Models;

public partial class StudentSession
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public int Module { get; set; }

    public string Description { get; set; } = null!;

    public string? InClassroom { get; set; }

    public string? InVehicle { get; set; }

    public string? InYard { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public virtual Student Student { get; set; } = null!;
}
