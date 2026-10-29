using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstructorWebClient.Domain
{
    public class SessionSummaryLister
    {
        public List<SessionSummary> List { get; set; } = new List<SessionSummary>();

        public SessionSummary SearchCriteria { get; set; } = new SessionSummary();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
