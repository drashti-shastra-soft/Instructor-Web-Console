using InstructorWebClient.Infrastructure.IRepository;
using InstructorWebClient.Infrastructure.IService;

namespace InstructorWebClient.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<Domain.UserLister> GetAllAsync(Domain.UserLister mLister)
        {
            return await _repo.GetAllAsync(mLister);
        }

        public async Task<Domain.User> UpsertAsync(Domain.User mUser)
        {
            return await _repo.UpsertAsync(mUser);
        }

        public async Task<Domain.User> GetByAsync(int id)
        {
            return await _repo.GetByAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
