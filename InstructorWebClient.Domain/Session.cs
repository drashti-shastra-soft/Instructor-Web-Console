using Newtonsoft.Json;

namespace InstructorWebClient.Domain
{
    public class Session
    {
        public int Id { get; set; }        
        public int UserId { get; set; }        
        public int ClassType { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Student { get; set; } = null!;
        [JsonProperty("Modules")]
        public List<Module> Modules { get; set; } = new List<Module>();
        public List<Domain.StudentSession> StudentSessions { get; set; } = new List<StudentSession>();
    }
}
