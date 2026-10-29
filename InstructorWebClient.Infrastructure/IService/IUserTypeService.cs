namespace InstructorWebClient.Infrastructure.IService
{
    public interface IUserTypeService
    {
        Task<List<Domain.UserType>> GetAllAsync();
    }
}
