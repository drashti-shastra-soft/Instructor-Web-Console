using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstructorWebClient.Infrastructure.IRepository
{
    public interface IStudentCourseRepository
    {
        Task<Domain.StudentCourseLister> GetAllAsync(Domain.StudentCourseLister mLister);
        Task<Domain.StudentCourse> UpsertAsync(Domain.StudentCourse mStudentCourse);
        Task<Domain.StudentCourse> GetByAsync(int id);
        Task DeleteAsync(int id);
        Task<Domain.StudentCourse> UpdateActiveAsync(Domain.StudentCourse mStudentCourse);
    }
}
