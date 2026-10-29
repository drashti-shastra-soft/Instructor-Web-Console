namespace InstructorWebClient.Infrastructure.IService
{
    public interface ISessionService
    {
        Task<Domain.SessionLister> GetAllAsync(Domain.SessionLister mLister);

        Task<Domain.Session> UpsertAsync(Domain.Session mSession);

        Task<Domain.Session> GetByAsync(int id);

        Task DeleteAsync(int id);

        Task<(List<int> Years, List<int> Months)> FetchDistinctYearMonthForUserAsync(int userId);

        Task<Domain.SessionSummary> GetSessionHourSumsAsync(int userId, int year, int month);
    }
}
