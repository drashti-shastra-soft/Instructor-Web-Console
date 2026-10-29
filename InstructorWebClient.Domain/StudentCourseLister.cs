using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstructorWebClient.Domain
{
    public class StudentCourseLister
    {
        public List<StudentCourse> List { get; set; } = new List<StudentCourse>();

        public StudentCourse SearchCriteria { get; set; } = new StudentCourse();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
