using InstructorWebClient.Domain;

namespace InstructorWebClient.Infrastructure.IService
{
    public interface IStudentService
    {
        Task<Domain.StudentLister> GetAllAsync(Domain.StudentLister mLister);

        Task<Domain.Student> UpsertAsync(Domain.Student mStudent);

        Task<Domain.Student> GetByAsync(int id);

        Task DeleteAsync(int id);

        Task<List<SelectListItem>> GetActiveStudent();
    }
}
