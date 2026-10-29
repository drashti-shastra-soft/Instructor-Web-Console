using System;
using System.Linq;

namespace InstructorWebClient.Domain
{
    public class SessionLister
    {
        public List<Session> List { get; set; } = new List<Session>();

        public Session SearchCriteria { get; set; } = new Session();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
