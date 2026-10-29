using InstructorWebClient.Infrastructure.IRepository;
using InstructorWebClient.Infrastructure.IService;

namespace InstructorWebClient.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepo;

        public AuthenticationService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<Domain.User?> ValidateAsync(Domain.Credential mCredential)
        {
            var mUser = await _userRepo.GetByAsync(mCredential.Username);

            if (mUser is null || mUser.Id <= 0 || mUser.Password != mCredential.Password)
                return null;

            return mUser;
        }
    }
}
