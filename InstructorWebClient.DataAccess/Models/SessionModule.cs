using System;
using System.Collections.Generic;

namespace InstructorWebClient.DataAccess.Models;

public partial class SessionModule
{
    public int Id { get; set; }

    public int SessionId { get; set; }

    public int Module { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}
