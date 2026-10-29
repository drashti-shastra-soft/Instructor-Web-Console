namespace InstructorWebClient.Domain
{
    public class StudentSessionLister
    {
        public List<StudentSession> List { get; set; } = new List<StudentSession>();

        public StudentSession SearchCriteria { get; set; } = new StudentSession();
    }
}
