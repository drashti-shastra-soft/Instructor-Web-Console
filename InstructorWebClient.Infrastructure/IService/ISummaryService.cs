using System;
using System.Linq;

namespace InstructorWebClient.Infrastructure.IService
{
    public interface ISummaryService
    {
        Task<Domain.SessionSummary> GetSessionSummaryAsync(int userId, int year = default(int), int month = default(int));
    }
}
