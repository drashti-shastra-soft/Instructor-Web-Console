using InstructorWebClient.Domain;
using InstructorWebClient.Infrastructure.IRepository;
using InstructorWebClient.Infrastructure.IService;

namespace InstructorWebClient.Service
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repo;

        public StudentService(IStudentRepository repo)
        {
            _repo = repo;
        }

        public async Task<Domain.StudentLister> GetAllAsync(Domain.StudentLister mLister)
        {
            return await _repo.GetAllAsync(mLister);
        }


        public async Task<Domain.Student> UpsertAsync(Domain.Student mStudent)
        {
            return await _repo.UpsertAsync(mStudent);
        }

        public async Task<Domain.Student> GetByAsync(int id)
        {
            return await _repo.GetByAsync(id);
        }


        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task<List<SelectListItem>> GetActiveStudent()
        {
            var courses = await _repo.GetActiveStudent();

            return courses.Select(c => new SelectListItem
            {
                Text = $"{c.FirstName} {c.LastName} ({c.LicenseNumber})",
                Value = c.Id.ToString()
            }).ToList();
        }

    }
}
