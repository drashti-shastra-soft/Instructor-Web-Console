using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstructorWebClient.Domain
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        public string Status { get; set; } = null!;

        public string? Description { get; set; }

        public int TotalCumulativeHours { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}
