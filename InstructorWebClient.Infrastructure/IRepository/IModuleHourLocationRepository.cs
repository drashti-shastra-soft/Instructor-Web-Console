using System;
using System.Linq;

namespace InstructorWebClient.Infrastructure.IRepository
{
    public interface IModuleHourLocationRepository
    {
        Task CreateAsync(Domain.HourLocation mHourLocation);

        Task DeleteAsync(int sessionId);
    }
}
