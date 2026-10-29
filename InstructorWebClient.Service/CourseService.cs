using InstructorWebClient.Domain;
using InstructorWebClient.Infrastructure.IRepository;
using InstructorWebClient.Infrastructure.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstructorWebClient.Service
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;

        public CourseService(ICourseRepository repo)
        {
            _repo = repo;
        }

        public async Task<Domain.CourseLister> GetAllAsync(Domain.CourseLister mLister)
        {
            return await _repo.GetAllAsync(mLister);
        }

        public async Task<Domain.Course> UpsertAsync(Domain.Course mCourse)
        {
            return await _repo.UpsertAsync(mCourse);
        }

        public async Task<Domain.Course> GetByAsync(int id)
        {
            return await _repo.GetByAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<List<SelectListItem>> GetActiveCourses()
        {
            var courses = await _repo.GetActiveCourses();

            return courses.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();
        }
    }
}
