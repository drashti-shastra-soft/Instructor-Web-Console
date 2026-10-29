using System;
using System.Linq;

namespace InstructorWebClient.Infrastructure.IRepository
{
    public interface IUserTypeRepository
    {
        Task<List<Domain.UserType>> GetAllAsync();
    }
}
