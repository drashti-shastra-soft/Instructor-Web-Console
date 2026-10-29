using InstructorWebClient.Infrastructure.IService;

namespace InstructorWebClient.Service
{
    public class SummaryService : ISummaryService
    {
        private readonly IUserService _instructorService;
        private readonly ISessionService _sessionService;

        public SummaryService(IUserService instructorService, ISessionService sessionService)
        {
            _instructorService = instructorService;
            _sessionService = sessionService;
        }

        public async Task<Domain.SessionSummary> GetSessionSummaryAsync(int userId, int year = default(int), int month = default(int))
        {
            var mModel = new Domain.SessionSummary();
            int currentYear = DateTime.UtcNow.Year;
            int currentMonth = DateTime.UtcNow.Month;

            var availableYearsAndMonths = _sessionService.FetchDistinctYearMonthForUserAsync(userId).GetAwaiter().GetResult();
            mModel = _sessionService.GetSessionHourSumsAsync(userId, (year != 0 ? year : currentYear), (month != 0 ? month : currentMonth)).GetAwaiter().GetResult();

            if (availableYearsAndMonths.Years != null)
            {

                mModel.Years = availableYearsAndMonths.Years.Select(y => new Domain.SelectListItem
                {
                    Text = y.ToString(),
                    Value = y.ToString(),
                    Selected = (y == currentYear)
                }).ToList();
            }

            if (availableYearsAndMonths.Months != null)
            {

                mModel.Months = availableYearsAndMonths.Months.Select(m => new Domain.SelectListItem
                {
                    Text = new DateTime(1, m, 1).ToString("MMMM"),
                    Value = m.ToString(),
                    Selected = (m == currentMonth)
                }).ToList();
            }

            return mModel;
        }
    }
}
