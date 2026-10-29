namespace InstructorWebClient.Infrastructure.IRepository
{
    public interface IStudentRepository
    {
        Task<Domain.StudentLister> GetAllAsync(Domain.StudentLister mLister);

        Task<Domain.Student> UpsertAsync(Domain.Student mStudent);

        Task<Domain.Student> GetByAsync(int id);

        Task DeleteAsync(int id);

        Task<List<Domain.Student>> GetActiveStudent();

        Task<List<Domain.Student>> GetActiveStudents(List<int> mStudents);
    }
}
