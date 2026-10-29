using System;
using System.Linq;

namespace InstructorWebClient.Infrastructure.IRepository
{
    public interface ISessionModuleRepository
    {
        Task<int> CreateAsync(Domain.Module mModule);

        Task DeleteAsync(int sessionId);
    }
}
