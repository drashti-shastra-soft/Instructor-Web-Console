using System;
using System.Collections.Generic;

namespace InstructorWebClient.Models;

public partial class Address
{
    public int Id { get; set; }

    public int StudentId { get; set; }

    public string Line1 { get; set; } = null!;

    public string? Line2 { get; set; }

    public string? Line3 { get; set; }

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string ZipCode { get; set; } = null!;

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}
