using System;
using System.Collections.Generic;

namespace InstructorWebClient.DataAccess.Models;

public partial class ModuleHourLocation
{
    public int Id { get; set; }

    public int SessionId { get; set; }

    public int ModuleId { get; set; }

    public string Location { get; set; } = null!;

    public string Hour { get; set; } = null!;

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}
