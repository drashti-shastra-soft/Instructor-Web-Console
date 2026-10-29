using System;
using System.Linq;

namespace InstructorWebClient.Infrastructure.IService
{
    public interface IStudentSessionService
    {
        Task<Domain.StudentSessionLister> GetAllAsync(Domain.StudentSessionLister mLister);

        Task<Domain.StudentSession> CreateAsync(Domain.StudentSession mStudent);

        Task<Domain.StudentSession> GetByAsync(int id);

        Task DeleteAsync(int id);
    }
}
