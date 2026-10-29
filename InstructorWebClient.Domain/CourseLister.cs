using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstructorWebClient.Domain
{
    public class CourseLister
    {
        public List<Course> List { get; set; } = new List<Course>();

        public Course SearchCriteria { get; set; } = new Course();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
