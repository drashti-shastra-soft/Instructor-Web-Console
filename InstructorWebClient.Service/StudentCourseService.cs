using InstructorWebClient.Infrastructure.IRepository;
using InstructorWebClient.Infrastructure.IService;


namespace InstructorWebClient.Service
{
    public class StudentCourseService : IStudentCourseService
    {
        private readonly IStudentCourseRepository _repo;

        public StudentCourseService(IStudentCourseRepository repo)
        {
            _repo = repo;
        }

        public async Task<Domain.StudentCourseLister> GetAllAsync(Domain.StudentCourseLister mLister)
        {
            return await _repo.GetAllAsync(mLister);
        }

        public async Task<Domain.StudentCourse> UpsertAsync(Domain.StudentCourse mStudentCourse)
        {
            if (mStudentCourse.IsActive)
            {
                await _repo.UpdateActiveAsync(mStudentCourse);
            }

            return await _repo.UpsertAsync(mStudentCourse);
        }

        public async Task<Domain.StudentCourse> GetByAsync(int id)
        {
            return await _repo.GetByAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        
    }
}
