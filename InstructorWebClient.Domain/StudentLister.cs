using System;
using System.Linq;

namespace InstructorWebClient.Domain
{
    public class StudentLister
    {
        public List<Student> List { get; set; } = new List<Student>();

        public Student SearchCriteria { get; set; } = new Student();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
