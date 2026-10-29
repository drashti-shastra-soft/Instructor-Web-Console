using InstructorWebClient.Infrastructure.IRepository;
using InstructorWebClient.Infrastructure.IService;

namespace InstructorWebClient.Service
{
    public class StudentSessionService : IStudentSessionService
    {
        private readonly IStudentSessionRepository _repo;

        public StudentSessionService(IStudentSessionRepository repo)
        {
            _repo = repo;
        }

        public async Task<Domain.StudentSessionLister> GetAllAsync(Domain.StudentSessionLister mLister)
        {
            return await _repo.GetAllAsync(mLister);
        }

        public async Task<Domain.StudentSession> CreateAsync(Domain.StudentSession mStudent)
        {
            return await _repo.CreateAsync(mStudent);
        }

        public async Task<Domain.StudentSession> GetByAsync(int id)
        {
            return await _repo.GetByAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
