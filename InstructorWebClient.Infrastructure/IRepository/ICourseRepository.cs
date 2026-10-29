using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstructorWebClient.Infrastructure.IRepository
{
    public interface ICourseRepository
    {
        Task<Domain.CourseLister> GetAllAsync(Domain.CourseLister mLister);
        Task<Domain.Course> UpsertAsync(Domain.Course mCourse);
        Task<Domain.Course> GetByAsync(int id);
        Task DeleteAsync(int id);
        Task<List<Domain.Course>> GetActiveCourses();
    }
}
