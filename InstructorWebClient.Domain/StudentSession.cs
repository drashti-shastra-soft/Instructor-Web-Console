using System;
using System.Linq;

namespace InstructorWebClient.Domain
{
    public class StudentSession
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SessionId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
