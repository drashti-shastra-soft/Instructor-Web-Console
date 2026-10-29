using System;
using System.Linq;

namespace InstructorWebClient.Domain
{
    public class SessionSummary
    {
        public List<Domain.SelectListItem> Years { get; set; } = new List<Domain.SelectListItem>();
        public List<Domain.SelectListItem> Months { get; set; } = new List<Domain.SelectListItem>();
        public string Username { get; set; } = string.Empty;
        public string InYard { get; set; } = string.Empty;
        public string InVehicle { get; set; } = string.Empty;
        public string InClassroom { get; set; } = string.Empty;
        public string Total { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
