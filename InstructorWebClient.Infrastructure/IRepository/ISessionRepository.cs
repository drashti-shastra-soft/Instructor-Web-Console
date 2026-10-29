using System;
using System.Linq;

namespace InstructorWebClient.Infrastructure.IRepository
{
    public interface ISessionRepository
    {
        Task<Domain.SessionLister> GetAllAsync(Domain.SessionLister mLister);

        Task<Domain.Session> UpsertAsync(Domain.Session mSession);

        Task<Domain.Session> GetByAsync(int id);

        Task DeleteAsync(int id);

        Task<(List<int> Years, List<int> Months)> FetchDistinctYearMonthForUserAsync(int userId);

        Task<(TimeSpan InYardTotal, TimeSpan InVehicleTotal, TimeSpan InClassroomTotal)> GetSessionHourSumsAsync(int userId, int year, int month);
    }
}
