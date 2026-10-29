namespace InstructorWebClient.Infrastructure.IService
{
    public interface ISessionManagerService
    {
        Domain.User? GetLoginUser();

        void SetLoginUser(Domain.User user);

        void ClearLoginUser();
    }
}
