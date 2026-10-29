using InstructorWebClient.Infrastructure.IRepository;
using InstructorWebClient.Infrastructure.IService;

namespace InstructorWebClient.Service
{
    public class UserTypeService : IUserTypeService
    {
        private readonly IUserTypeRepository _repo;

        public UserTypeService(IUserTypeRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Domain.UserType>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }
    }
}
