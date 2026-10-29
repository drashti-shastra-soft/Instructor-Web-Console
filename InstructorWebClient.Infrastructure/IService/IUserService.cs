
namespace InstructorWebClient.Infrastructure.IService
{
    public interface IUserService
    {
        Task<Domain.UserLister> GetAllAsync(Domain.UserLister mLister);

        Task<Domain.User> UpsertAsync(Domain.User mUser);

        Task<Domain.User> GetByAsync(int id);

        Task DeleteAsync(int id);
    }
}
