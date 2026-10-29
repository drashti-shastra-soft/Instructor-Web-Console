using System;
using System.Linq;

namespace InstructorWebClient.Domain
{
    public class Module
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int ModuleNumber { get; set; }
        public List<HourLocation> HourLocations { get; set; } = new List<HourLocation>();
    }
}
