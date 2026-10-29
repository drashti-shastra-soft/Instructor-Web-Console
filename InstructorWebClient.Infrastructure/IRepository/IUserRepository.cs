using System;
using System.Linq;

namespace InstructorWebClient.Infrastructure.IRepository
{
    public interface IUserRepository
    {
        Task<Domain.User> GetByAsync(string emailAddress);

        Task<Domain.UserLister> GetAllAsync(Domain.UserLister mLister);

        Task<Domain.User> UpsertAsync(Domain.User mUser);

        Task<Domain.User> GetByAsync(int id);

        Task DeleteAsync(int id);
    }
}
