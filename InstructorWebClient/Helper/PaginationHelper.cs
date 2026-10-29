using Microsoft.AspNetCore.Mvc.Rendering;

namespace InstructorWebClient.Helper
{
    public static class PaginationHelper
    {
        public static List<SelectListItem> GetPageSize()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Text = "5", Value = "5" },
                new SelectListItem { Text = "10", Value = "10" },
                new SelectListItem { Text = "20", Value = "20" },
                new SelectListItem { Text = "50", Value = "50" },
                new SelectListItem { Text = "100", Value = "100" },
                new SelectListItem { Text = "All", Value = "-1" }
            };
        }

        public static string GetPaginationSummary(Domain.Pagination pagination)
        {
            if (pagination.TotalRecord == 0)
                return "No entries to show.";

            int start = pagination.Skip + 1;
            int end = Math.Min(pagination.Skip + pagination.Take, pagination.TotalRecord);

            return $"Showing {start} to {end} of {pagination.TotalRecord} entries";
        }
    }
}
