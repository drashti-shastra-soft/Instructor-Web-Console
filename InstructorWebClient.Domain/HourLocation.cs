using System;
using System.Linq;

namespace InstructorWebClient.Domain
{
    public class HourLocation
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int ModuleId { get; set; }
        public string Location { get; set; }
        public string Hour { get; set; }
    }
}
