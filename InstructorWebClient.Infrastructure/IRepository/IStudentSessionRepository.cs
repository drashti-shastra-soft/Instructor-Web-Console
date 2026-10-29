namespace InstructorWebClient.Infrastructure.IRepository
{
    public interface IStudentSessionRepository
    {
        Task<Domain.StudentSessionLister> GetAllAsync(Domain.StudentSessionLister mLister);

        Task<Domain.StudentSession> CreateAsync(Domain.StudentSession mStudent);

        Task<Domain.StudentSession> GetByAsync(int id);

        Task DeleteAsync(int id);
    }
}
