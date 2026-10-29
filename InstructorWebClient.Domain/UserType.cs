using System;
using System.Linq;

namespace InstructorWebClient.Domain
{
    public class UserType
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }
        public string Status { get; set; }

        public virtual ICollection<User> Users { get; set; } = new List<User>();

    }
}
